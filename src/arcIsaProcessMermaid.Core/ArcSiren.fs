namespace ArcIsaProcessMermaid.Core

open ARCtrl
open ARCtrl.QueryModel
open Siren
open System.Collections.Generic

module ArcProcesses = 

    // Determine whether one process precedes another
    // based on min 1 intersecting Input/Output reference
    let isPreviousProcessOf (processA: ArcTable) (processB: ArcTable) : bool =    
    
        match processB.TryGetInputColumn() with
            | Some a -> 

                match processA.TryGetOutputColumn() with 

                | Some a -> 

                    Set.intersect (set processA.OutputNames) (set processB.InputNames)
                        |> Seq.length
                        |> fun x -> x > 0

                | None -> 

                    printfn "%s" $"INFO: No Output column found in {processA.Name}"
                    false

            | None -> 

                printfn "%s" $"INFO: No Input column found in {processB.Name}"
                false

    // Count the number of intersections
    let numSamplesFromPreviousProcess (processA: ArcTable) (processB: ArcTable) : int = 
                    
        match processB.TryGetInputColumn() with
            | Some a -> 

                match processA.TryGetOutputColumn() with 

                | Some a -> 

                    Set.intersect (set processA.OutputNames) (set processB.InputNames)
                    |> Seq.length

                | None -> 

                    printfn "%s" $"INFO: No Output column found in {processA.Name}"
                    0

            | None -> 

                printfn "%s" $"INFO: No Input column found in {processB.Name}"
                0


module ArcSiren =

    type MermaidClassDef =
        { 
            className   : string
            style       : (string * string) []
        }

    let investigationStyle = 
        {
            className   = "investigationStyle"
            style       = [|
                "fill", "#4FB3D9"
                "rx", ".4em"
                "ry", ".4em"
                "color", "#2d3e50"
                "stroke", "#2d3e50"
                "font-weight", "bold"
            |]
        }
        
    let studyStyle = 
        {
            className   = "studyStyle"
            style       = [|
                "fill", "#dae7c1"
                "rx", ".4em"
                "ry", ".4em"
                "color", "#2d3e50"
                "stroke", "#2d3e50"
                "font-weight", "bold"
            |]
        }    
    let assayStyle = 
        {
            className   = "assayStyle"
            style       = [|
                "fill", "#ffe080"
                "rx", ".4em"
                "ry", ".4em"
                "color", "#2d3e50"
                "stroke", "#2d3e50"
                "font-weight", "bold"
            |]
        }
        
    let processStyle = 
        {
            className   = "processStyle"
            style       = [|
                "fill", "#E08F9C"
                "rx", ".4em"
                "ry", ".4em"
                "color", "#2d3e50"
                "stroke", "#2d3e50"
                "font-weight", "normal"
            |]
        }

    let getId (dict: Dictionary<string, string>) (counter: byref<int>) (key: string) =
        if dict.ContainsKey(key) then
            dict.[key]
        else
            let id = $"id_{counter}"
            counter <- counter + 1
            dict.Add(key, id)
            id

    let createIsaMermaidBody (arc : ARC) =
        /// Dictionaries are mutable, any changes done to this will be propagated.
        /// inv, assay, study id are key and value are mermaid ids
        let key_dict = System.Collections.Generic.Dictionary<string, string>()
        let mutable counter = 0
        let getId (key: string) = getId key_dict &counter key
        
        // let investigation = arc.ISA.Value
        
        let studies = arc.Studies
        let assays = arc.Assays
        let processes = arc.ArcTables
        
        let investigationId = getId (arc.Identifier)
        [
            // add investigation start-node
            flowchart.node(
                investigationId, 
                arc.Title |> Option.defaultValue "<no-title>" |> formatting.unicode
            )
            flowchart.``class``([investigationId], investigationStyle.className)
            
            // adding "study:" and "assay: to the subgraph names to allow that study / 
            // assay identifier and one of their process names are identical
            // otherwise this breaks with mermaid
            
            for study in studies do
                let studyId = getId(study.Identifier)
                let subgraphId = "STUDY_" + studyId
                //// let sLabel = "Study:" + ArcUtils.Arcpaths.generateStudyLinkFromRoot s.Identifier "."
                let sLabel = "Study: " + (study.Title |> Option.defaultValue study.Identifier)
                // link studies to investigation
                flowchart.linkArrow(investigationId, subgraphId)
                flowchart.``class``([subgraphId], studyStyle.className)
                // add study subgraphs
                flowchart.subgraphNamed(subgraphId, sLabel, [
                    for table in study do
                        let tableId = getId(table.Name)
                        flowchart.node(tableId, table.Name)
                        flowchart.``class``([tableId], processStyle.className)
                ])

            // add assay subgraphs
            for assay in assays do
                let assayId = getId(assay.Identifier)
                let subgraphId = "ASSAY_" + assayId
                let aLabel = "Assay: " + assay.Identifier
                flowchart.subgraphNamed(subgraphId, aLabel, [
                    for table in assay do
                        let tableId = getId(table.Name)
                        flowchart.node(tableId, table.Name)            
                        flowchart.``class``([tableId], processStyle.className)
                ])
                flowchart.``class``([subgraphId], assayStyle.className)                

            // add process-to-process edges, with sample numbers as edge name
            for table1 in processes do
                for table2 in processes do
                    if ArcProcesses.isPreviousProcessOf table1 table2 then
                        let nSamples = ArcProcesses.numSamplesFromPreviousProcess table1 table2
                        let t1Id = getId(table1.Name)
                        let t2Id = getId(table2.Name)
                        flowchart.linkArrow(
                            t1Id, 
                            t2Id, 
                            nSamples.ToString()
                        )
        ]

    let createArcProcessMermaid (flowDirection : Direction) (arc : ARC) = 
        siren.flowchart(flowDirection, [
            flowchart.classDef(investigationStyle.className, investigationStyle.style)
            flowchart.classDef(studyStyle.className, studyStyle.style)
            flowchart.classDef(assayStyle.className, assayStyle.style)
            flowchart.classDef(processStyle.className, processStyle.style)
            yield! createIsaMermaidBody arc
        ])
        |> siren.write

    let arcIsaProcesses2mermaid (flowDirection : Direction) (arc : ARC) (outputFileName : string) (mmd: bool) =
        match mmd with
        | false ->
            let o = System.IO.Path.ChangeExtension(outputFileName, ".md")
            ["```mermaid"; createArcProcessMermaid flowDirection arc; "```"]
            |> fun c -> System.IO.File.WriteAllLines(o, c)
        | true ->
            let o = System.IO.Path.ChangeExtension(outputFileName, ".mmd")
            [createArcProcessMermaid flowDirection arc]
            |> fun c -> System.IO.File.WriteAllLines(o, c)
