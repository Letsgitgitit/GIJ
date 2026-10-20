# WAR DB — P1 Overview: step-by-step build guide (step 7)
BUILD 2026-10-20b · Power BI Desktop May 2026 (.pbix) · follows `P1_LAYOUT_AND_CONTRACT.md` v1.4 (positions, colours,
measure names). About 2 hours. Work on a **copy** of the .pbix until the last step.

How to read this guide
- Every visual gets: **where** (X · Y · W · H, typed in Format ▸ General ▸ Properties ▸ Size and position — never
  drag), **what** (fields), **how** (only the settings the theme does not already set), **name** (Selection pane).
- **Checkpoint** = stop and check that part of the page; flag anything that looks off before going on.
- Menu names can differ slightly between builds. If a setting is not where written, use the search box at the top of
  the Format pane (type the setting name).
- The theme already sets: page and visual backgrounds, borders (1 px, radius 10), fonts, title colours, table grid,
  slicer colours, chart axes and gridlines. Do not re-set those per visual.

---------------------------------------------------------------------------------------------------------------------
## 0. Before you start (10 min)

0.1 **Measures up to date.** Tabular Editor ▸ run `P1_PATCH_05_TE2.csx` (revenue client root, CORTEX 06 threshold,
    24 Jul note) ▸ Ctrl+S ▸ back in Desktop File ▸ Save. Patches 01–04 must already be in (they are).

0.2 **Theme.** View ▸ Themes ▸ (drop-down arrow) ▸ **Browse for themes** ▸ `WAR_DB_Theme_v1.json`. Every existing page
    changes colour; that is expected.

0.3 **New page.** Right-click the page tabs ▸ New page ▸ rename `P1 Overview`. Build everything there; the old
    `OVERVIEW` page stays untouched until step 12.

0.4 **Canvas.** Click the empty canvas ▸ Format ▸ Canvas settings: Type **16:9**, (1280 × 720). Canvas background and
    wallpaper come from the theme (check: page `#141C27`, wallpaper `#0E141D`).

0.5 **Page filters.** Filters pane ▸ "Filters on this page": must be empty. "Filters on all pages": remove any date
    filter there (the period slicer does that job now).

0.6 **Selection pane** on (View ▸ Selection). Every visual gets renamed there (double-click the name) as you go.

0.7 **Sort-by columns** (Table view, once):
    - `P1 Metric[Metric]` ▸ Column tools ▸ Sort by column ▸ `Order`.
    - `P1 Exposure Line[Line]` ▸ Sort by column ▸ `Order`.
    - `P1 Unit[Booking Centre]` ▸ Sort by column ▸ `Order`.

---------------------------------------------------------------------------------------------------------------------
## 1. Rail and header shapes (10 min)

| Name | Insert | X | Y | W | H | Settings |
|---|---|---|---|---|---|---|
| `A0 Rail` | Insert ▸ Shapes ▸ Rectangle | 0 | 0 | 52 | 720 | Style ▸ Fill `#1D2735`, Border off |
| `B0 Header Back` | Insert ▸ Shapes ▸ Rectangle | 60 | 8 | 1212 | 60 | Fill `#1D2735`, Border on `#2C3A4D` 1 px, Shape ▸ Rounded corners 10 |

Then the navigation buttons (Insert ▸ Buttons ▸ **Blank**), X 13, W 26, H 26:

| Name | Y | Icon (Format ▸ Button ▸ Icon ▸ shape) | Action |
|---|---|---|---|
| `A1 Nav Overview` | 12 | any simple shape (e.g. "Home" / rectangle), fill `#46B6C9` (active page) | Type Page navigation ▸ `P1 Overview` |
| `A2 Nav P2` … `A5 Nav P5` | 48 / 84 / 120 / 156 | same shape, fill `#5E6E84` | Page navigation ▸ the page (leave blank until P2/P5 exist) |
| `A6 Filters Open` | 676 | "filter" icon if offered, else a gear-like shape, colour `#8C9BB0` | Type **Bookmark** ▸ `P1 Panel Open` (set in step 10) |

For each: Format ▸ Button ▸ Action ▸ **Tooltip** = the page name. Copy `A2` to make `A3`–`A5` (Ctrl+C / Ctrl+V, then
retype Y).
*Fallback:* Insert ▸ Buttons ▸ Navigator ▸ **Page navigator**, vertical, at 4 · 8 · 44 · 180, text 9 pt.

---------------------------------------------------------------------------------------------------------------------
## 2. Header slicers (15 min)

### B1 `B1 CPH` — 70 · 10 · 300 · 56
- Insert ▸ Slicer. Field: `DIM_COUNTERPARTY[CPH_Name]`.
- Format ▸ Slicer settings ▸ Options ▸ Style **Dropdown**. Selection: Single select **off**, "Select all" **off**.
- Slicer header: on, text `Client group (CPH)`. Values: font **14 pt**, Segoe UI Semibold (theme gives 11 pt).
- Visual header ▸ ⋯ ▸ **Search** (turns on the search box in the drop-down).

### B2 `B2 CPY` — 380 · 10 · 330 · 56
- Copy B1, paste, retype position. Swap the field to `DIM_COUNTERPARTY[CPY Label]`. Header text `Entity (CPY)`.
- It narrows to the selected CPH automatically.

### B3 `B3 Period` — 720 · 10 · 250 · 56
- Insert ▸ Slicer. Field: `DIM_DATE[Date]`. Style **Relative date**: **This** · **Year**. Header `Reporting period`.
- Leave it on This Year when you save = the default (year to date).

### B4 `B4 Data As Of` — 980 · 10 · 284 · 56
- Insert ▸ **Card** (the new card; it may be called "Card (new)"). Data: `Asof 16A`, `Asof 06`, `Asof 17A-17B`,
  `Asof Sales`. In the Data well, rename them `16A`, `06`, `17A/B`, `Sales` (double-click each).
- Format ▸ Layout: 4 cards, **1 row** (Layout ▸ Arrangement / Max cards per row = 4).
- Callout value 10 pt. Label 9 pt. Title on: `Data as of`, 9 pt.
- Per card colour: Format ▸ **Callout value** ▸ "Apply settings to" = `16A` ▸ Font colour ▸ **fx** ▸ Format style
  *Field value* ▸ `Asof Colour 16A`. Repeat for `06` → `Asof Colour 06`, `17A/B` → `Asof Colour 17A-17B`,
  `Sales` → `Asof Colour Sales`.
- Card shape (Format ▸ Cards ▸ Shape): rounded 50 %, border 1 px `#3A4C66`.
- *Fallback* (per-card colour not offered): four separate Cards, X 980 / 1052 / 1124 / 1196, Y 26, W 68, H 38, one
  measure each + a text box `Data as of` at 980 · 10 · 200 · 16.

**Checkpoint 1:** the header row, with B1/B2 drop-downs open once.

---------------------------------------------------------------------------------------------------------------------
## 3. KPI cards C1–C4 (25 min) — build C1 completely, then copy ×3

### C1 `C1 Card 16A` — 60 · 76 · 213 · 96
1. Insert ▸ Card (new). Data: `16A Util %`. Rename in the well: `16A · MtM`.
2. **Callout value:** 24 pt (theme). Font colour ▸ fx ▸ Field value ▸ `Fmt RAG Colour 16A`.
3. **Reference labels** (Format ▸ Reference labels ▸ + Add / drag into the "Reference labels" well):
   - `P1 Card Detail 16A` → reads "€404.5m of €995.0m". Title off, value 9 pt.
   - `P1 Card Delta 16A` → "▲x.xpp since 2 Jan". Value colour ▸ fx ▸ Field value ▸ `P1 Delta Colour 16A`.
   - `P1 Structure 16A` → "Standalone" / "Envelope" / "6 entities". Horizontal alignment right.
   Layout of reference labels: position **below** the callout, arrangement **horizontal** if offered.
4. **Sparkline:** Format ▸ **Image** (in the card, "Cards ▸ Image") ▸ on ▸ Image type **Image URL** ▸ fx ▸ Field value
   ▸ `P1 Sparkline 16A`. Position bottom, size 186 × 24 (or "Image area size" ~25 %).
   *Fallback:* drop the sparkline (D1 below shows the trend).
5. **Subtitle** (data guard): Format ▸ General ▸ Title ▸ Subtitle **on** ▸ Text ▸ fx ▸ Field value ▸
   `P1 Data Check 16A`. 8 pt, colour `#E0A44B`. Normally empty.
6. Title: off (the label "16A · MtM" is the title).

Check: CPY-A selected → 41 % (green), "€404.5m of €995.0m", "Standalone".

### C2 `C2 Card 06` — 281 · 76 · 213 · 96
Copy C1 ▸ paste ▸ retype position ▸ in the Data well replace each field by its `06` twin: `06 Util %` (rename
`06 · FX delivery`), `P1 Card Detail 06`, `P1 Card Delta 06`, `P1 Structure 06`, colours `Fmt RAG Colour 06` and
`P1 Delta Colour 06`, image `P1 Sparkline 06`, subtitle `P1 Data Check 06`.
Check: CPY-A → 44 %, "€2,746.8m of €6,293.9m".

### C3 `C3 Card 17A` — 502 · 76 · 213 · 96 and C4 `C4 Card 17B` — 723 · 76 · 213 · 96
Copy C1 ▸ repoint to the `17A` / `17B` twins (labels `17A · Repo`, `17B · Reverse repo`).
**Subtitle differs:** fx ▸ Field value ▸ `Unit Label` ("Booking centre CA-CIB (10)"), 8 pt, colour ▸ fx ▸
`Unit Label Colour` (grey on 10, amber on any other booking centre).
Check: CPY-A → 0 %, "€0.0m of €1,500.0m" (17A) and "€0.0m of €600.0m" (17B).

**Checkpoint 2:** the four cards with CPY-A selected.

---------------------------------------------------------------------------------------------------------------------
## 4. Main trend D1 + Metric Focus D2 (20 min)

### D1 `D1 Main Trend` — 60 · 180 · 876 · 270
1. Insert ▸ Line chart. X-axis: `DIM_DATE[Date]` ▸ in the well, click the arrow ▸ **Date** (not Date Hierarchy).
   Format ▸ X-axis ▸ Type **Continuous**, date format `MMM yy` (Values ▸ display format).
2. Y-axis, in this order: `Sel Limit`, `Sel Util`, `Sel Peak Marker`, `Sel Last Marker`, `Sel Check Marker`.
3. **Lines** (Format ▸ Lines ▸ "Apply settings to" each series):
   `Sel Limit` coral `#E8597A` 1.5 px · `Sel Util` teal `#46B6C9` 2 px · the three marker series: stroke width **0**.
4. **Markers** (Format ▸ Markers): on for `Sel Peak Marker` (teal, 6 px, circle), `Sel Last Marker` (teal, 6 px,
   circle), `Sel Check Marker` (amber `#E0A44B`, 6 px, **diamond**); off for the two lines.
5. **Data labels:** on ▸ Apply settings to `Sel Limit` and `Sel Util` → off; `Sel Peak Marker` and `Sel Last Marker`
   → on (value, 9 pt); `Sel Check Marker` → off.
   Peak date in the label: Data labels ▸ series `Sel Peak Marker` ▸ **Detail** ▸ on ▸ field `Sel Peak Date Label`
   → "€1,027.0m / 12 Mar". *Fallback:* value only (date in the tooltip).
6. **Legend:** on, Top (theme). Rename the marker series in the Data well to a single space " " so the legend shows
   only "Limit" and "Utilisation" (rename `Sel Limit` → `Limit`, `Sel Util` → `Utilisation`), except
   `Sel Check Marker` → `⚠ carried forward`. *Fallback:* legend off, subtitle "teal = utilisation, coral = limit".
7. **Title:** fx ▸ Field value ▸ `Sel Trend Title`. **Subtitle:** on ▸ fx ▸ Field value ▸ `P1 Trend Note`, 8 pt,
   amber. (Empty except for 16A with envelopes over 24 Jul.)
8. **Y-axis:** start 0, display units Millions, title off. **Tooltips** well: `Sel Util %`, `Sel Headroom`.
9. Leave the top-right 240 × 32 of the chart free (D2 sits there): Format ▸ General ▸ Padding top 34 if the title
   collides.

### D2 `D2 Metric Focus` — 700 · 186 · 228 · 28
- Insert ▸ **Button slicer**. Field: `P1 Metric[Metric]`. Selection: Single select **on**, **Force selection** on.
  Layout: 4 columns × 1 row.
- Selected state (Format ▸ Buttons ▸ State **Selected**): fill `#46B6C9`, text `#0E141D`. Default state: fill
  `#26344A`, text `#8C9BB0`. Text 9 pt. Click **16A** before saving (= default).
- *Fallback:* classic Slicer, Style **Tile**, horizontal, single select — same field.
- Selection pane: drag D2 **above** D1.

**Checkpoint 3:** D1 with 16A selected, then with 06 selected.

---------------------------------------------------------------------------------------------------------------------
## 5. Lower charts E1, F1 (15 min)

### E1 `E1 Repo EUR` — 60 · 458 · 434 · 254
- Copy D1 ▸ paste ▸ retype position ▸ **delete D2's interaction later** (step 9). Replace the Y fields with:
  `17A Util` (teal, 2 px), `17B Util` (slate `#8A97AB`, 2 px), `17A Peak Marker`, `17B Peak Marker` (markers 6 px, line
  0, labels on; data label detail off). Remove `Sel …` fields.
- Title: fx off ▸ text `17A / 17B utilisation (EUR)`. Subtitle off. Tooltips: `17A Limit`, `17B Limit`.

### F1 `F1 Util Pct` — 502 · 458 · 434 · 254
- Copy E1 ▸ retype position. Y fields: `06 Util %` (amber), `17A Util %` (teal), `17B Util %` (slate),
  `06 Peak Marker %`, `17A Peak Marker %`, `17B Peak Marker %` (markers + labels only), `06 Check Marker %` (amber ◆,
  no label, legend name `⚠ carried forward`).
- Y-axis: Range min **0**, max **1.1**, display format `0%`.
- **Amounts on the peaks:** Data labels ▸ series `06 Peak Marker %` ▸ Detail ▸ field `06 Peak Label EUR`
  (→ "34% / €3.4m"); same for 17A / 17B with their `Peak Label EUR`. *Fallback:* amounts in the tooltip (next line).
- Tooltips well: `06 Util`, `06 Limit`, `17A Util`, `17A Limit`, `17B Util`, `17B Limit`.
- Title: `Utilisation % of limit · 06 · 17A · 17B`.

**Checkpoint 4:** E1 and F1 for the whole book (clear B1/B2).

---------------------------------------------------------------------------------------------------------------------
## 6. Right column G1, H1 (20 min)

### G1 `G1 Exposure` — 944 · 76 · 328 · 176
- Insert ▸ **Table**. Columns: `P1 Exposure Line[Line]`, `P1 Exposure Bar`, `P1 Exposure Value`.
- `P1 Exposure Bar` must render as a picture: it is set as Image URL by the script. Format ▸ Grid (or Image size):
  height 12, width 150.
- Column headers **off** (Format ▸ Column headers ▸ off, or text colour = background if no switch).
- Sort by `Line` ascending (column header ⋯ ▸ Sort ascending; it follows `Order`).
- Title fx ▸ `Sel Exposure Title`. "Show items with no data" stays **off** (the bride row hides itself).
- *Fallback* (bar shows as text): remove `P1 Exposure Bar`; on `P1 Exposure Value` ▸ Cell elements ▸ **Data bars**,
  maximum = Field value `Sel Limit`.

### H1 `H1 Entities` — 944 · 260 · 328 · 270
- Insert ▸ Table. Columns, in order, with header text (rename in the well) and width (Format ▸ Specific column ▸ Width,
  or drag and read): `DIM_COUNTERPARTY[Short Name]` "Entity" 108 · `H Structure` "S" 18 · `16A Util %` "16A" 41 ·
  `06 Util %` "06" 41 · `17A Util %` "17A" 41 · `17B Util %` "17B" 41 · `H Flags` " " 24.
- Values and headers 9 pt (theme). Text wrap off. Totals off. Row padding 2 (theme).
- **Data bars** on each of the four Util % columns (Cell elements ▸ Data bars ▸ on ▸ min 0 *Number*, max 1 *Number*,
  positive colour `#46B6C9`, transparency 60 %). **Font colour** ▸ fx ▸ Field value ▸ `Fmt RAG Colour 16A` (… `06`,
  `17A`, `17B` for the other three).
- **Filter** (Filters pane ▸ this visual): `H Has Limit` is **1**.
- Sort: `16A Util %` descending.
- Tooltips well: `DIM_COUNTERPARTY[CPY Label]` (full name + Ricos on hover).
- Footer text box `H Footer` at 952 · 510 · 312 · 16, 8 pt `#8C9BB0`:
  `S standalone · E envelope · B bride · ● CORTEX ≠ facts · ⚠ carried forward`.

**Checkpoint 5:** G1 and H1 with CPY-A's CPH selected.

---------------------------------------------------------------------------------------------------------------------
## 7. Revenue I1, I2 (10 min)

### I1 `I1 Revenue` — 944 · 538 · 328 · 88
- Insert ▸ Card (new). Data: `Rev Revenue EUR` (rename `Revenue`), `Rev RpR` (rename `RpR (rev ÷ avg 16A util)`).
  Layout 2 cards in 1 row. Callout 18 pt.
- Reference labels for `Revenue`: `Rev vs LY Label` (value colour fx ▸ `Rev vs LY Colour`), then `Rev Root Label`
  (8 pt, `#8C9BB0`) → "Sales client root €15.1m".
- Reference label for `RpR`: `Rev Avg 16A Util Label`.
- Check: CPY-A YTD → €0.2m, root line €15.1m; its CPH → €5.4m.

### I2 `I2 Revenue Months` — 944 · 626 · 328 · 86
- Insert ▸ Clustered column chart. X: `DIM_DATE[YearMonth]` (sort ascending by it). Y: `Rev Revenue EUR`.
- Columns teal (theme colour 1). Y-axis off, legend off, data labels off (theme), title off.

---------------------------------------------------------------------------------------------------------------------
## 8. Hidden filter panel A7 (15 min)

| Name | Insert | X | Y | W | H | Content |
|---|---|---|---|---|---|---|
| `A7a Panel Back` | Shape ▸ Rectangle | 60 | 76 | 300 | 268 | fill `#1D2735`, border `#3A4C66` 1 px, radius 10, shadow on |
| `A7b Panel Title` | Text box | 72 | 82 | 200 | 24 | "Filters", 11 pt Semibold |
| `A7c Close` | Button ▸ Blank | 324 | 84 | 28 | 24 | icon ✕ (X shape), action Bookmark ▸ `P1 Panel Closed` |
| `A7d MCG` | Slicer | 72 | 112 | 276 | 64 | `DIM_COUNTERPARTY[MCG_Code]`, Dropdown, header "MCG" |
| `A7e Country` | Slicer | 72 | 188 | 276 | 64 | `DIM_COUNTERPARTY[CountryName]`, Dropdown, header "Country" |
| `A7f Booking Centre` | Slicer | 72 | 264 | 276 | 64 | `P1 Unit[Booking Centre]`, Dropdown, **Single select on**, header "17A/17B booking centre"; select **CA-CIB (10)** |

- Select A7a–A7f in the Selection pane (Ctrl+click) ▸ right-click ▸ **Group** ▸ rename `A7 Panel`. Drag the group to
  the **top** of the Selection pane.

---------------------------------------------------------------------------------------------------------------------
## 9. Interactions (10 min)

Format ▸ **Edit interactions** on. Click the source visual, then on each target pick Filter (funnel) or None (⊘).

| Source → | D1 | G1 | H1 | C1–C4 | E1 | F1 | I1–I2 | B4 |
|---|---|---|---|---|---|---|---|---|
| B1 CPH | F | F | F | F | F | F | F | **None** |
| B2 CPY | F | F | **None** | F | F | F | F | **None** |
| B3 Period | F | F | F | F | F | F | F | **None** |
| A7d MCG, A7e Country | F | F | F | F | F | F | F | **None** |
| D2 Metric | F | F | **None** | **None** | **None** | **None** | **None** | **None** |
| H1 row click | F | F | – | F | F | F | F | **None** |
| D1, E1, F1 (point click) | **None** on every target (a click on a date would silently move the as-of date) |

A7f needs nothing (its table has no relationship). Edit interactions off.

---------------------------------------------------------------------------------------------------------------------
## 10. Bookmarks for the panel (10 min)

1. View ▸ **Bookmarks**. Selection pane: make `A7 Panel` **visible** (eye on).
2. Bookmarks ▸ Add ▸ rename `P1 Panel Open`. ⋯ ▸ untick **Data**, keep **Display** and **Current page** ticked,
   choose **Selected visuals** — with only `A7 Panel` selected in the Selection pane — then ⋯ ▸ **Update**.
3. Hide `A7 Panel` (eye off). Add ▸ `P1 Panel Closed`. Same four settings, Update.
4. `A6 Filters Open` ▸ Action ▸ Bookmark ▸ `P1 Panel Open`. `A7c Close` ▸ Action ▸ Bookmark ▸ `P1 Panel Closed`.
5. Test in Desktop with **Ctrl+click** on A6 and A7c. Change B1 while the panel is open: the selection must survive
   closing it (that is what unticking Data does).
6. Leave the panel **closed** before saving.

**Checkpoint 6:** the full page, panel closed; then panel open.

---------------------------------------------------------------------------------------------------------------------
## 11. Final checks (15 min)

11.1 **Layer order** (Selection pane, top first): `A7 Panel` · `D2 Metric Focus` · everything else · `B0 Header Back` ·
     `A0 Rail`.
11.2 **Numbers** (CPY-A, This Year): C1 41 % · C2 44 % · C3/C4 0 % on €1,500.0m / €600.0m · I1 €0.2m with root line
     €15.1m · H1 shows CPY-A's entities with ● on CPY-A only if its 06 gap is above 2pp (it was 1pp → no dot now).
11.3 **Whole book** (clear B1/B2): D1 16A line never above ~€105bn in 2023–2026 (the guard), diamonds on the
     guarded days.
11.4 **Speed:** View ▸ **Performance analyzer** ▸ Start recording ▸ Refresh visuals. Target: no visual above 2,000 ms.
     Flag the list if any is above. (DAX Studio ▸ Server Timings can then pinpoint the measure.)
11.5 **Text size:** nothing below 8 pt (subtitles) / 9 pt (everything else).

**Checkpoint 7:** Performance analyzer list + the page for CPY-A.

---------------------------------------------------------------------------------------------------------------------
## 12. Switch over (5 min)

- When P1 is signed off: delete the old `OVERVIEW` page, rename `P1 Overview` → `OVERVIEW`, drag it to the first tab.
- File ▸ Save. Keep the pre-build copy for one week.

---------------------------------------------------------------------------------------------------------------------
## If something does not work
| Symptom | Likely cause | Fix |
|---|---|---|
| A card shows "(Blank)" | measure blank in this selection (e.g. no limit) | expected for entities without that limit; for CPY-A, flag it |
| SVG bar / sparkline shows long text | Data category not Image URL | Tabular Editor ▸ measure ▸ Data Category = ImageUrl ▸ Ctrl+S |
| Error icon on a visual | a measure broke | run the `INFO.MEASURES()` query (State ≠ 1) and flag the result |
| Panel closes but CPH resets | bookmark has Data ticked | ⋯ ▸ untick Data ▸ Update (both bookmarks) |
| D2 click changes the cards | interactions | step 9: D2 → C1–C4 = None |
| Slow visual (> 2 s) | — | flag the Performance analyzer list |
