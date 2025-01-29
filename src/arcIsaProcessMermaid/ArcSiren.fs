namespace ArcIsaProcessMermaid

open ARCtrl
open ARCtrl.QueryModel
open Siren

open ArcUtils.Arcpaths
open ArcProcesses
open MermaidStyling

module ArcSiren = 

    let inv = 
        {
            className   = "inv"
            style       = {
                fill   = "#6c7885"
                color   = "#6c7885"
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
        |> List.map (fun i -> createMermaidclassDef i)
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

                let sid = "Study:" + s.Identifier
                            
                let sLabel = "Study:" + generateStudyLinkFromRoot s.Identifier "."

                // let subgraphLabel = createMermaidLabel s.Identifier sLabel

                // link studies to investigation
                flowchart.linkArrow(investigation.Identifier, sid)
                
                // add study subgraphs
                flowchart.subgraphNamed(sid, sLabel, [

                    for p in s do
                        flowchart.node(p.Name.Replace(" ", "-"), p.Name)
                        ])

            // add assay subgraphs

            for a in assays do

                let aid = "Assay:" + a.Identifier

                let aLabel = "Assay:" + generateAssayLinkFromRoot a.Identifier "."

                flowchart.subgraphNamed(aid, aLabel, [

                    for p in a do
                        flowchart.node(p.Name.Replace(" ", "-"), p.Name)            
                        ])

            // add process-to-process edges, with sample numbers as edge name

            for p1 in processes do
                    for p2 in processes do

                        if isPreviousProcessOf p1 p2 then
                            let nSamples = numSamplesFromPreviousProcess p1 p2
                            flowchart.linkArrow(p1.Name.Replace(" ", "-"), p2.Name.Replace(" ", "-"), nSamples.ToString())

        ])
        |> siren.write


    let createIsaMermaidStyle (arc : ARC) =

        let investigation = arc.ISA.Value
            
        let studies = investigation.Studies
        
        let assays = investigation.Assays

        let processes = investigation.ArcTables

        [
        
            [inv; std; asy; prc]
            |> List.map (fun i -> createMermaidclassDef i)
            |> String.concat "\n"
            
            [investigation.Identifier]
            |> assignMermaidClass "inv"

            let collectStudyIDs = 
                studies
                |> Seq.map (fun s ->
                    "Study:" + s.Identifier
                )

            collectStudyIDs
            |> assignMermaidClass "study"
            
            let collectAssayIDs = 
                assays
                |> Seq.map (fun a ->
                    "Assay:" + a.Identifier
                )

            collectAssayIDs
            |> assignMermaidClass "assay"
            
                    
            let collectProcessIDs = 
                processes
                |> Seq.map (fun p ->
                    p.Name.Replace(" ", "-")
                )

            collectProcessIDs
            |> assignMermaidClass "process"
            
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