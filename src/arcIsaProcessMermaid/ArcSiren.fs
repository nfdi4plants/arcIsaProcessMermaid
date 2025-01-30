namespace ArcIsaProcessMermaid

open ARCtrl
open ARCtrl.QueryModel
open Siren

open ArcUtils
open ArcProcesses
open MermaidStyling

module ArcSiren = 

    let replaceChars (chars) = String.map (fun c -> if Seq.exists((=)c) chars then '_' else c)    
    let mermaidBreakingChars = "°^!§$%&/\|()=?`´*+~#;:,.<>' "

    let inv = 
        {
            className   = "inv"
            style       = {
                fill   = "#6c7885"
                color   = "#2d3e50"
                fontWeight  = "bold"
                }
        }
        
    let std = 
        {
            className   = "study"
            style       = {
                fill   = "#62d4c1"
                color   = "#2d3e50"
                fontWeight  = "bold"
                }
        }    
    let asy = 
        {
            className   = "assay"
            style       = {
                fill   = "#ffd34d"
                color   = "#2d3e50"
                fontWeight  = "bold"
                }
        }
        
    let prc = 
        {
            className   = "process"
            style       = {
                fill   = "#D46275"
                color   = "#2d3e50"
                fontWeight  = ""
                }
        }

    let classDefCollection = 
        [inv; std; asy; prc]
        |> List.map (fun i -> MermaidStyling.createMermaidclassDef i)
        |> String.concat "\n"

    let createIsaMermaidBody (flowDirection : Direction) (arc : ARC) =
        let investigation = arc.ISA.Value
        let studies = investigation.Studies
        let assays = investigation.Assays
        let processes = investigation.ArcTables

        siren.flowchart(flowDirection, [
            // add investigation start-node
            flowchart.node(investigation.Identifier) // probably obsolete, since the node is added by linking inv -> s below
            
            /// adding "study:" and "assay: to the subgraph names to allow that study / 
            /// assay identifier and one of their process names are identical
            /// otherwise this breaks with mermaid
            
            for s in studies do
                let sid = "STUDY_" + s.Identifier |> replaceChars mermaidBreakingChars
                //// let sLabel = "Study:" + ArcUtils.Arcpaths.generateStudyLinkFromRoot s.Identifier "."
                let sLabel = "Study:" + s.Identifier
                /// link studies to investigation
                flowchart.linkArrow(investigation.Identifier, sid)
                /// add study subgraphs
                flowchart.subgraphNamed(sid, sLabel, [
                    for p in s do
                        flowchart.node(p.Name |> replaceChars mermaidBreakingChars, p.Name)
                ])

            // add assay subgraphs
            for a in assays do
                let aid = "ASSAY_" + a.Identifier |> replaceChars mermaidBreakingChars
                //// let aLabel = "Assay:" + ArcUtils.Arcpaths.generateAssayLinkFromRoot a.Identifier "."
                let aLabel = "Assay:" + a.Identifier
                flowchart.subgraphNamed(aid, aLabel, [
                    for p in a do
                        flowchart.node(p.Name |> replaceChars mermaidBreakingChars, p.Name)            
                ])

            // add process-to-process edges, with sample numbers as edge name
            for p1 in processes do
                for p2 in processes do
                    if ArcProcesses.isPreviousProcessOf p1 p2 then
                        let nSamples = ArcProcesses.numSamplesFromPreviousProcess p1 p2
                        flowchart.linkArrow(
                                p1.Name |> replaceChars mermaidBreakingChars, 
                                p2.Name |> replaceChars mermaidBreakingChars, 
                                nSamples.ToString()
                                )
        ])
        |> siren.write

    let createIsaMermaidStyle (arc : ARC) =
        let investigation = arc.ISA.Value
        let studies = investigation.Studies
        let assays = investigation.Assays
        let processes = investigation.ArcTables

        [
            [inv; std; asy; prc]
            |> List.map (fun i -> MermaidStyling.createMermaidclassDef i)
            |> String.concat "\n"
            
            [investigation.Identifier]
            |> MermaidStyling.assignMermaidClass "inv"

            let collectStudyIDs = 
                studies
                |> Seq.map (fun s -> "STUDY_" + s.Identifier |> replaceChars mermaidBreakingChars)

            match collectStudyIDs |> Seq.length < 1 with
            | true  -> 
                printfn "%s" "no study found"
                ""
            | false -> 
                collectStudyIDs
                |> MermaidStyling.assignMermaidClass "assay"
            
            let collectAssayIDs = 
                assays
                |> Seq.map (fun a -> "ASSAY_" + a.Identifier |> replaceChars mermaidBreakingChars)

            match collectAssayIDs |> Seq.length < 1 with
            | true  -> 
                printfn "%s" "no assay found"
                ""
            | false -> 
                collectAssayIDs
                |> MermaidStyling.assignMermaidClass "assay"
        
            let collectProcessIDs = 
                processes
                |> Seq.map (fun p -> p.Name |> replaceChars mermaidBreakingChars)

            match collectProcessIDs |> Seq.length < 1 with
            | true  -> 
                printfn "%s" "no process found"
                ""
            | false -> 
                collectProcessIDs
                |> MermaidStyling.assignMermaidClass "process"

        ]
        |> String.concat("\n")

    let createIsaMermaid (flowDirection : Direction) (arc : ARC) = 
        [
            createIsaMermaidBody flowDirection arc
            createIsaMermaidStyle arc
        ]
        |> String.concat("\n")

    let arcIsaProcesses2mermaid (flowDirection : Direction) (arc : ARC) (outputFileName : string) (mmd: bool) =
        match mmd with
        | false ->
            let o = System.IO.Path.ChangeExtension(outputFileName, ".md")
            ["```mermaid"; createIsaMermaid flowDirection arc; "```"]
            |> fun c -> System.IO.File.WriteAllLines(o, c)
        | true ->
            let o = System.IO.Path.ChangeExtension(outputFileName, ".mmd")
            [createIsaMermaid flowDirection arc]
            |> fun c -> System.IO.File.WriteAllLines(o, c)