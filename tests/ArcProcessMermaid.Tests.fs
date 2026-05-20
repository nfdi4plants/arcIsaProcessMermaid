module Tests.ArcProcessMermaid

open Expecto
open ARCtrl
open ArcIsaProcessMermaid.Core

open Siren

[<Tests>]
let Main = testList "ArcProcessMermaid.Tests" [
    testCase "Ensure test framework" <| fun _ ->
        let actual = 1
        let expected = 1
        Expect.equal actual expected "1 should be equal to 1"
    testCase "Ensure load ARC" <| fun _ ->
        let arc = loadArc "example-arcs/arc-scaffold"
        Expect.isSome arc.Title ""
    testCase "ARC to md" <| fun _ ->
        let arc = loadArc "example-arcs/arc-scaffold"
        let actual = ArcSiren.createArcProcessMermaid direction.lr true arc
        let expected = """
flowchart LR
    classDef investigationStyle fill:#4FB3D9,rx:.4em,ry:.4em,color:#2d3e50,stroke:#2d3e50,font-weight:bold;
    classDef studyStyle fill:#dae7c1,rx:.4em,ry:.4em,color:#2d3e50,stroke:#2d3e50,font-weight:bold;
    classDef assayStyle fill:#ffe080,rx:.4em,ry:.4em,color:#2d3e50,stroke:#2d3e50,font-weight:bold;
    classDef processStyle fill:#E08F9C,rx:.4em,ry:.4em,color:#2d3e50,stroke:#2d3e50,font-weight:normal;
    id_0["ArcPrototype"]
    class id_0 investigationStyle;
    id_0-->STUDY_id_1
    subgraph STUDY_id_1[Study: Prototype for experimental data]
        id_2[CellCultivation]
        class id_2 processStyle;
        id_3[AccessoryDataRetrieval]
        class id_3 processStyle;
    end
    class STUDY_id_1 studyStyle;
    id_0-->STUDY_id_4
    subgraph STUDY_id_4[Study: experiment2]
        id_5[experiment2]
        class id_5 processStyle;
    end
    class STUDY_id_4 studyStyle;
    subgraph ASSAY_id_6[Assay: measurement1]
        id_7[Cell Lysis]
        class id_7 processStyle;
        id_8[Protein Extraction]
        class id_8 processStyle;
        id_9[Protein Measurement]
        class id_9 processStyle;
        id_10[Computational Proteome Analysis]
        class id_10 processStyle;
    end
    class ASSAY_id_6 assayStyle;
    subgraph ASSAY_id_11[Assay: measurement2]
    end
    class ASSAY_id_11 assayStyle;
    id_2-->|6|id_7
    id_7-->|6|id_8
    id_8-->|6|id_9
    id_9-->|6|id_10

"""
        Expect.trimEqual actual expected ""
]
