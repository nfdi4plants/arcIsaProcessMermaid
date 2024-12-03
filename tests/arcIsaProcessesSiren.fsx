// Script to write a minimal markdown containing a mermaid graph
// that displays an ARC's connections of ISA processes
// from ARC investigation through studies and assays

#r "nuget: ARCtrl.NET"
#r "nuget: ARCtrl.QueryModel"
#r "nuget: Siren"
#r "nuget: FSharp.Data"

open FSharp.Data
open ARCtrl.NET
open ARCtrl
open ARCtrl.QueryModel
open Siren


// Load ARC from an RO-Crate file
let loadARCfromROCrate (arcRocPath : string) = 
    JsonValue.Load(arcRocPath)
        |> string
        |> ARC.fromROCrateJsonString

// Generate html links
let generateHtmlLink (url : string) (text : string) =
    $"<a href='{url}'>{text}</a>"

// Build link to a study directory
let generateStudyLinkFromRoot (studyID : string) (arcRoot: string) =
    
    let filePath = System.IO.Path.Join(arcRoot, "studies", studyID)
    
    generateHtmlLink filePath studyID


let createMermaidLabel (id: string) (label: string) =
    $"{id}[{label}]"

// Determine whether one process precedes another
// based on min 1 intersecting Input/Output reference

let isPreviousProcessOf (processA: ArcTable) (processB: ArcTable) : bool = 
    Set.intersect (set processA.OutputNames) (set processB.InputNames)
    |> Seq.length
    |> fun x -> x > 0

// Count the number of intersections

let numSamplesFromPreviousProcess (processA: ArcTable) (processB: ArcTable) : int = 
    Set.intersect (set processA.OutputNames) (set processB.InputNames)
    |> Seq.length

let createIsaMermaid (arc : ARC) =

    let investigation = arc.ISA.Value
        
    let studies = investigation.Studies
    
    let assays = investigation.Assays

    let processes = investigation.ArcTables
    
    siren.flowchart(direction.leftToRight, [

        // add investigation start-node
        flowchart.node(investigation.Identifier) // probably obsolete, since the node is added by linking inv -> s below
        
        //// adding "study:" and "assay: to the subgraph names to allow that study / assay identifier and one of their process names are identical
        //// otherwise this breaks with mermaid
        
        for s in studies do

            // let sid = "Study:" + s.Identifier
                        
            let sLabel = "Study:" + generateStudyLinkFromRoot s.Identifier "."

            let subgraphLabel = createMermaidLabel s.Identifier sLabel

            // link studies to investigation
            flowchart.linkArrow(investigation.Identifier, s.Identifier)
            
            // add study subgraphs
            flowchart.subgraph(subgraphLabel, [

                for p in s do
                    flowchart.node(p.Name.Replace(" ", "-"), p.Name)
                    ])

        // add assay subgraphs

        for a in assays do

            let aLabel = "Assay:" + generateStudyLinkFromRoot a.Identifier "."

            let subgraphAssayLabel = createMermaidLabel a.Identifier aLabel

            flowchart.subgraph(subgraphAssayLabel, [

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


// Write ARC mermaid to markdown file

let arcIsaProcesses2mermaid (arc : ARC) (outputFileName : string) (markdown: bool) =

    match markdown with
    | true ->
        let o = System.IO.Path.ChangeExtension(outputFileName, ".md")
        ["```mermaid"; createIsaMermaid arc; "```"]
        |> fun c -> System.IO.File.WriteAllLines(o, c)
    | false ->
        let o = System.IO.Path.ChangeExtension(outputFileName, ".mmd")
        [createIsaMermaid arc]
        |> fun c -> System.IO.File.WriteAllLines(o, c)

// let args : string array = fsi.CommandLineArgs |> Array.tail
// let arcPath = args.[0]
// let outputFileName = args.[1]

// printfn "Printing output to %s" outputFileName


let outputFileName = "test"
let arcRocPath = "2024-11-26T14-38-14_datahubArcId1974_rocrate.json"
let arcRocFullPath = System.IO.Path.Join(__SOURCE_DIRECTORY__, arcRocPath)
let arc = loadARCfromROCrate(arcRocFullPath)


arcIsaProcesses2mermaid arc outputFileName true
