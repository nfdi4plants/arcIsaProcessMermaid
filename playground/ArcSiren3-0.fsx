#r "nuget: ARCtrl,3.0.3"
#r "nuget: ARCtrl.QueryModel,2.1.0"
#r "nuget: Siren"

open ARCtrl
open ARCtrl.QueryModel
open Siren
open System.Collections.Generic

/// Type to add context (ParentID = StudyID or AssayID) to an ArcTable
type ProcessWithContext =
    { ParentId : string
      Table    : ArcTable }

/// Module containing functions for process relationships
module ArcProcesses = 

    /// Determines whether one process precedes another
    /// based on at least one intersecting Input/Output reference
    let isPreviousProcessOf (processA: ArcTable) (processB: ArcTable) : bool =    
        match processB.TryGetInputColumn() with
            | Some _ -> 
                match processA.TryGetOutputColumn() with 
                | Some _ -> 
                    Set.intersect (set processA.OutputNames) (set processB.InputNames)
                        |> Seq.length
                        |> fun x -> x > 0
                | None -> 
                    printfn "%s" $"INFO: No Output column found in {processA.Name}"
                    false
            | None -> 
                printfn "%s" $"INFO: No Input column found in {processB.Name}"
                false

    /// Counts the number of intersections between process outputs and inputs
    let numSamplesFromPreviousProcess (processA: ArcTable) (processB: ArcTable) : int = 
        match processB.TryGetInputColumn() with
            | Some _ -> 
                match processA.TryGetOutputColumn() with 
                | Some _ -> 
                    Set.intersect (set processA.OutputNames) (set processB.InputNames)
                    |> Seq.length
                | None -> 
                    printfn "%s" $"INFO: No Output column found in {processA.Name}"
                    0
            | None -> 
                printfn "%s" $"INFO: No Input column found in {processB.Name}"
                0

/// Module for generating Mermaid diagrams from ARC data
module ArcSiren =

    /// Represents a Mermaid class definition for styling
    type MermaidClassDef =
        { 
            className   : string
            style       : (string * string) []
        }

    /// Style for investigation nodes
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
        
    /// Style for study nodes
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

    /// Style for assay nodes
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
        
    /// Style for process nodes
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

    /// Gets or creates a unique Mermaid node ID for a given key
    let getId (dict: Dictionary<string, string>) (counter: byref<int>) (key: string) =
        if dict.ContainsKey(key) then
            dict.[key]
        else
            let id = $"id_{counter}"
            counter <- counter + 1
            dict.Add(key, id)
            id

    /// Builds the body of the Mermaid diagram for an ARC
    let createIsaMermaidBody (addSampleNumbers : bool) (arc : ARC)  =
        // Dictionaries are mutable, any changes done to this will be propagated.
        // inv, assay, study id are key and value are mermaid ids
        let key_dict = System.Collections.Generic.Dictionary<string, string>()
        let mutable counter = 0
        let getId (key: string) = getId key_dict &counter key
        
        let studies = arc.Studies
        let assays = arc.Assays
        
        // Collect all processes with context (study or assay)
        
        // This looks more complicated than needed, since the 
        // QueryModel would allow to collect all ArcTables via `let processes = arc.ArcTables`
        // However, this would not carry the context (StudyID or AssayID along)
        // In this way, processes are allowed to have duplicate table names across studies and assays
        let processes =
            seq {
                for s in arc.Studies do
                    for t in s.Tables do
                        yield { ParentId = s.Identifier; Table = t }
                for a in arc.Assays do
                    for t in a.Tables do
                        yield { ParentId = a.Identifier; Table = t }
            }
            |> Seq.toList

        let investigationId = getId (arc.Identifier)
        
        [
            // Add investigation start-node
            flowchart.node(
                investigationId, 
                arc.Title |> Option.defaultValue "<no-title>" |> formatting.unicode
            )

            // Add style to investigation
            flowchart.``class``([investigationId], investigationStyle.className)
            
            // Add study subgraphs
            for study in studies do
                let studyId = getId(study.Identifier)
                let subgraphId = "STUDY_" + studyId
                let sLabel = "Study: " + (study.Title |> Option.defaultValue study.Identifier)
                
                // Link studies to investigation
                flowchart.linkArrow(investigationId, subgraphId)
                
                // Add study subgraph nodes
                flowchart.subgraphNamed(subgraphId, sLabel, [
                    for table in study do
                        let tableId = getId $"{study.Identifier}:{table.Name}"
                        flowchart.node(tableId, table.Name)
                        flowchart.``class``([tableId], processStyle.className)
                ])
                
                // Add style to study subgraph
                flowchart.``class``([subgraphId], studyStyle.className)

            // Add assay subgraphs
            for assay in assays do
                let assayId = getId(assay.Identifier)
                let subgraphId = "ASSAY_" + assayId
                let aLabel = "Assay: " + assay.Identifier

                // Add assay subgraph nodes
                flowchart.subgraphNamed(subgraphId, aLabel, [
                    for table in assay do
                        let tableId = getId $"{assay.Identifier}:{table.Name}"
                        flowchart.node(tableId, table.Name)

                        // Add style to process
                        flowchart.``class``([tableId], processStyle.className)
                ])

                // Add style to assay subgraph
                flowchart.``class``([subgraphId], assayStyle.className)                

            // Add process-to-process edges, with sample numbers as edge name
            for p1 in processes do
                for p2 in processes do
                    if ArcProcesses.isPreviousProcessOf p1.Table p2.Table then
                        let nSamples = ArcProcesses.numSamplesFromPreviousProcess p1.Table p2.Table
                        let t1Id = getId($"{p1.ParentId}:{p1.Table.Name}")
                        let t2Id = getId($"{p2.ParentId}:{p2.Table.Name}")
                        
                        match addSampleNumbers with
                        | false -> flowchart.linkArrow(t1Id, t2Id)
                        | true -> flowchart.linkArrow(t1Id, t2Id, nSamples.ToString())
                                
        ]

    /// Creates a Mermaid diagram for an ARC with specified flow direction
    let createArcProcessMermaid (flowDirection : Direction) (addSampleNumbers : bool) (arc : ARC) = 
        siren.flowchart(flowDirection, [
            flowchart.classDef(investigationStyle.className, investigationStyle.style)
            flowchart.classDef(studyStyle.className, studyStyle.style)
            flowchart.classDef(assayStyle.className, assayStyle.style)
            flowchart.classDef(processStyle.className, processStyle.style)
            yield! createIsaMermaidBody addSampleNumbers arc
        ])
        |> siren.write

    /// Writes the Mermaid diagram to a file, as .md or .mmd
    let arcIsaProcesses2mermaid (flowDirection : Direction) (addSampleNumbers : bool) (arc : ARC) (outputFileName : string) (mmd: bool) =
        match mmd with
        | false ->
            let o = System.IO.Path.ChangeExtension(outputFileName, ".md")
            ["```mermaid"; createArcProcessMermaid flowDirection addSampleNumbers arc; "```"]
            |> fun c -> System.IO.File.WriteAllLines(o, c)
        | true ->
            let o = System.IO.Path.ChangeExtension(outputFileName, ".mmd")
            [createArcProcessMermaid flowDirection addSampleNumbers arc]
            |> fun c -> System.IO.File.WriteAllLines(o, c)


let home = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)

let arcPath = home + "/datahub-dataplant/Facultative-CAM-in-Talinum/"
 
let arc = ARC.load(arcPath)

ArcSiren.arcIsaProcesses2mermaid Siren.Direction.TD true arc "test" false
