// =====================================================================================
// P1_PATCH_05_TE2.csx  -  BUILD 2026-10-20a
// Decisions of 2026-10-20: (1) revenue follows the page selection + new second line "sales client root"
// ([Rev Root EUR], [Rev Root Label]); (2) CORTEX dot: 06 threshold widened to 2pp (16A stays 0.5pp);
// (3) [P1 Trend Note] for the 24 Jul 2026 envelope change (D1 subtitle).
// Replaces Rev Revenue EUR (comment only), H CORTEX Flag; adds Rev Root EUR, Rev Root Label, P1 Trend Note.
// RUN: Tabular Editor > Advanced Scripting > paste > Run > Ctrl+S > File > Save in Power BI Desktop. Safe to run twice.
// =====================================================================================
var home = Model.Tables["_Measures"];
var missing = new List<string>();
if (!Model.Tables["FACT_SALES"].Columns.Contains("ClientRootName")) missing.Add("FACT_SALES[ClientRootName]");
foreach (var m0 in new[] { "Sel Metric", "16A Util %", "06 Util %" })
    if (!home.Measures.Contains(m0)) missing.Add("measure [" + m0 + "]");
if (missing.Count > 0) {
    Info("STOPPED - nothing changed. Missing:\n" + string.Join("\n", missing));
    return;
}
var patch = new List<string[]>();
patch.Add(new[] { @"Rev Revenue EUR", @"v2\5 Revenue", @"""€""#,0.0,,""m""", @"-- Revenue is a flow: it sums over the period (unlike limits). Follows the page selection (CPY or CPH), like the
-- limits, so RpR compares like with like (decision 2026-10-20). The wider sales client root is in [Rev Root EUR].
SUM( FACT_SALES[TotalGainEUR] )" });
patch.Add(new[] { @"Rev Root EUR", @"v2\5 Revenue", @"""€""#,0.0,,""m""", @"-- Revenue of the sales client root(s) of the selected entities (FACT_SALES[ClientRootName]), same period, ALL entities
-- of that root whatever their CPH. Wider than the CPH (check R6, 19 Oct: one root = 1,231 Ricos across several CPHs).
-- Second line under the revenue card only; never used in RpR.
VAR roots =
    FILTER(
        CALCULATETABLE( VALUES( FACT_SALES[ClientRootName] ), REMOVEFILTERS( DIM_DATE ) ),
        FACT_SALES[ClientRootName] <> """"
    )
RETURN
    IF( NOT ISEMPTY( roots ),
        CALCULATE(
            SUM( FACT_SALES[TotalGainEUR] ),
            REMOVEFILTERS( DIM_COUNTERPARTY ),
            TREATAS( roots, FACT_SALES[ClientRootName] )
        ) )" });
patch.Add(new[] { @"Rev Root Label", @"v2\5 Revenue", @"", @"-- Reference label under the revenue card: ""Sales client root €15.1m"" (or ""roots (2)""). Blank on the whole book.
VAR roots =
    FILTER(
        CALCULATETABLE( VALUES( FACT_SALES[ClientRootName] ), REMOVEFILTERS( DIM_DATE ) ),
        FACT_SALES[ClientRootName] <> """"
    )
VAR n = COUNTROWS( roots )
VAR v = [Rev Root EUR]
RETURN
    IF( ISFILTERED( DIM_COUNTERPARTY ) && NOT ISBLANK( v ),
        IF( n = 1, ""Sales client root "", ""Sales client roots ("" & n & "") "" )
            & FORMAT( v / 1000000, ""€#,0.0"" ) & ""m"" )" });
patch.Add(new[] { @"H CORTEX Flag", @"v2\7 P1 visuals", @"", @"-- Interim rule while CORTEX is in USD: compare utilisation % (currency-free) on the latest CORTEX date in the period,
-- for 16A and 06, for an entity that holds its own line (standalone, bride or envelope holder). Envelope members are
-- skipped: CORTEX carries them on the holder's row. Dot when |facts % - CORTEX %| > 0.5pp (16A) or > 2pp (06, moves intraday; decision 2026-10-20), or when
-- the entity holds a 16A or 06 line in the facts but has no CORTEX row that day. One entity at a time (table H).
IF(
    HASONEVALUE( DIM_COUNTERPARTY[HeadOfficeSirisId] ),
    VAR ricosSel = SELECTEDVALUE( DIM_COUNTERPARTY[HeadOfficeSirisId] )
    VAR dc = CALCULATE( MAX( FACT_CORTEX[SnapshotDate] ), REMOVEFILTERS( DIM_COUNTERPARTY ) )
    VAR f16 = CALCULATE( [16A Util %], DIM_DATE[Date] = dc )
    VAR f06 = CALCULATE( [06 Util %], DIM_DATE[Date] = dc )
    VAR mem16 =
        CALCULATE( COUNTROWS( FACT_LIMIT_16A ), DIM_DATE[Date] = dc,
            FACT_LIMIT_16A[StructureType] = ""Envelope"", FACT_LIMIT_16A[EnvelopeHolderId] <> ricosSel ) > 0
    VAR mem06 =
        CALCULATE( COUNTROWS( FACT_LIMIT_06 ), DIM_DATE[Date] = dc,
            FACT_LIMIT_06[StructureType] = ""Envelope"", FACT_LIMIT_06[EnvelopeHolderId] <> ricosSel ) > 0
    VAR hasCortex = CALCULATE( COUNTROWS( FACT_CORTEX ), DIM_DATE[Date] = dc ) > 0
    VAR c16 = CALCULATE( DIVIDE( SUM( FACT_CORTEX[16A_Util] ), SUM( FACT_CORTEX[16A_Auth] ) ), DIM_DATE[Date] = dc )
    VAR c06 = CALCULATE( DIVIDE( SUM( FACT_CORTEX[06_Util] ), SUM( FACT_CORTEX[06_Auth] ) ), DIM_DATE[Date] = dc )
    VAR own16 = NOT ISBLANK( f16 ) && NOT mem16
    VAR own06 = NOT ISBLANK( f06 ) && NOT mem06
    VAR bad16 = own16 && ( NOT hasCortex || ABS( f16 - c16 ) > 0.005 )
    VAR bad06 = own06 && ( NOT hasCortex || ABS( f06 - c06 ) > 0.02 )
    RETURN IF( NOT ISBLANK( dc ) && ( bad16 || bad06 ), ""●"" )
)" });
patch.Add(new[] { @"P1 Trend Note", @"v2\7 P1 visuals", @"", @"-- D1 subtitle: on 24 Jul 2026 the risk system's envelope utilisation figures changed overnight (e.g. one envelope
-- 166m -> 223m, under review). Shown only for 16A, when the period spans that date and the selection held envelopes.
VAR lo = MIN( DIM_DATE[Date] )
VAR hi = MAX( DIM_DATE[Date] )
VAR envSel =
    CALCULATE( COUNTROWS( FACT_LIMIT_16A ), DIM_DATE[Date] = DATE( 2026, 7, 23 ),
        FACT_LIMIT_16A[StructureType] = ""Envelope"" ) > 0
RETURN
    IF( [Sel Metric] = ""16A"" && lo <= DATE( 2026, 7, 24 ) && hi >= DATE( 2026, 7, 24 ) && envSel,
        ""Envelope figures: risk-system method change on 24 Jul 2026 (under review)"" )" });
var rep = new System.Text.StringBuilder();
int i = 0;
foreach (var p in patch) {
    i++;
    var exists = home.Measures.Contains(p[0]);
    var m = exists ? home.Measures[p[0]] : home.AddMeasure(p[0]);
    m.Expression = p[3];
    m.DisplayFolder = p[1];
    m.FormatString = p[2];
    rep.AppendLine(i + "/" + patch.Count + "  " + (exists ? "UPDATED  " : "CREATED  ") + p[0]);
}
rep.AppendLine("");
rep.AppendLine("Next: Ctrl+S, File > Save in Power BI Desktop.");
Info("P1_PATCH_05 BUILD 2026-10-20a\n\n" + rep.ToString());
