# CORTEX FX — FACT_CORTEX file currency + FxToEUR (BUILD 2026-10-22a)

Converts the USD CORTEX (GOLDEN) files already collected to EUR, at the risk system's own daily rate.
New files are pulled in EUR and convert at x1. No history re-pull.

## Rules
- File currency comes from the file name: `GOLDEN_YYYYMMDD_EUR.csv` = EUR; any other `GOLDEN_YYYYMMDD.csv` = USD.
- Rate for a date = USD total lines in `FACT_RECONC`: `AuthoOrigCcy / AuthoEUR` (limits >= EUR 100k), 4 decimals.
  This matches the rate the risk system uses between its USD and EUR reports (checked). ECB / market rates are not used.
- Exact date only. A USD file whose date has no RECONC rate gets a blank `FxToEUR` (listed by check FX2) — never carried
  forward from another day.
- One file per date: a USD and an EUR copy of the same date fail the refresh (M guard).
- EUR amount = amount x `FxToEUR`. No converted copy of each amount column.

## Files
| File | What |
|---|---|
| `FACT_CORTEX_STEPS.m` | 3 steps to insert into the live `FACT_CORTEX` query |
| `CORTEX_FX.dax` | `DIM_FX_EURUSD` calculated table, `FxToEUR` calculated column, measure pattern |
| `CORTEX_FX_CHECKS.dax` | FX1, FX2 (DAX query view) |

## Steps (Power BI Desktop)
1. **M — FACT_CORTEX.** Transform data > `FACT_CORTEX` > Advanced Editor. Insert the 3 steps of `FACT_CORTEX_STEPS.m`
   right after the step that filters the folder to the `GOLDEN_*.csv` files (the table still has `[Name]` and `[Content]`).
   - Replace `PreviousStep` with that step's name.
   - Point the step that used to read it at `Checked`.
   - Make sure `FileCurrency` survives to the final table (if the query expands the file contents with a column list,
     keep `FileCurrency` beside the expanded columns; if it removes columns, do not remove it).
   - Close & Apply. `FACT_CORTEX` now has a text column `FileCurrency` (`USD` on all current files).
2. **Table.** Modeling > New table > paste `DIM_FX_EURUSD` from `CORTEX_FX.dax`.
3. **Relationship.** Model view: `FACT_CORTEX[SnapshotDate]` (many) -> `DIM_FX_EURUSD[FxDate]` (one), single direction,
   active. `DIM_FX_EURUSD` has no other relationship (no path clash with `DIM_DATE`).
4. **Column.** On `FACT_CORTEX`: New column > paste `FxToEUR` from `CORTEX_FX.dax`. Format: decimal, 6 places.
5. **Checks.** DAX query view, run `CORTEX_FX_CHECKS.dax`:
   - FX1: expect **no rows** (one ratio per date). Any row = a date where RECONC USD lines disagree — flag it.
   - FX2: one row per CORTEX date. Expect `NoRate = 0` on every USD date and `Rate = 1` on EUR dates.
     A USD date with `NoRate > 0` = no RECONC file that day: the date stays unconverted (visible gap).
6. Save.

## Going forward
- Save new pulls as `GOLDEN_YYYYMMDD_EUR.csv`. Nothing else changes.
- Month-end history pulls: in EUR, same naming.
- To retire the FX layer later: delete the USD files, then `FxToEUR`, the relationship and `DIM_FX_EURUSD`
  (and the `FileCurrency` steps if wanted).

## Cost
`DIM_FX_EURUSD` scans `FACT_RECONC` once per refresh and keeps ~1k USD total lines per date (~965 output rows);
`FxToEUR` is a RELATED lookup per CORTEX row. Both are seconds; recomputed when `FACT_RECONC` or `FACT_CORTEX` refreshes.
