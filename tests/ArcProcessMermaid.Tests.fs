module Tests.ArcProcessMermaid

open Expecto
open ARCtrl

open Siren

[<Tests>]
let Main = testList "ArcProcessMermaid.Tests" [
    testCase "Ensure test framework" <| fun _ ->
        let actual = 1
        let expected = 1
        Expect.equal actual expected "1 should be equal to 1"
    testCase "Ensure load ARC" <| fun _ ->
        let arc = loadArc "example-arcs/arc-scaffold"
        Expect.isSome arc.ISA ""
    testCase "ARC to md" <| fun _ ->
        let arc = loadArc "example-arcs/arc-scaffold"
        let actual = ArcIsaProcessMermaid.ArcSiren.createArcProcessMermaid direction.td arc
        let expected = """```mermaid
flowchart TD
    classDef investigationStyle fill:#6c7885,color:#2d3e50,fontWeight:bold;
    classDef studyStyle fill:#62d4c1,color:#2d3e50,fontWeight:bold;
    classDef assayStyle fill:#ffd34d,color:#2d3e50,fontWeight:bold;
    classDef processStyle fill:#D46275,color:#2d3e50;
    id_0["ArcPrototype"]
    class id_0 investigationStyle;
    id_0-->STUDY_id_1
    class id_1 studyStyle;
    subgraph STUDY_id_1[Study: Prototype for experimental data]
        class id_2 processStyle;
        id_2[CellCultivation]
        class id_3 processStyle;
        id_3[AccessoryDataRetrieval]
    end
    id_0-->STUDY_id_4
    class id_4 studyStyle;
    subgraph STUDY_id_4[Study: experiment2]
        class id_4 processStyle;
        id_4[experiment2]
    end
    class id_5 assayStyle;
    subgraph ASSAY_id_5[Assay: measurement1]
        class id_6 processStyle;
        id_6[Cell Lysis]
        class id_7 processStyle;
        id_7[Protein Extraction]
        class id_8 processStyle;
        id_8[Protein Measurement]
        class id_9 processStyle;
        id_9[Computational Proteome Analysis]
    end
    class id_10 assayStyle;
    subgraph ASSAY_id_10[Assay: measurement2]
    end
    id_2-->|6|id_6
    id_6-->|6|id_7
    id_7-->|6|id_8
    id_8-->|6|id_9

```"""
        Expect.trimEqual actual expected ""
]
