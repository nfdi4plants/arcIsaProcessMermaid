// Script to write a minimal markdown containing a mermaid graph
// that displays an ARC's connections of ISA processes
// from ARC investigation through studies and assays

open ARCtrl.NET
open ARCtrl
open ARCtrl.QueryModel
open Siren
open Argu
open System

// Load ARC from an RO-Crate file
let tryLoadARCfromROCrate (arcRocPath : string) = 
    try
        arcRocPath
        |> System.IO.File.ReadAllText 
        |> ARC.fromROCrateJsonString
        |> Some
    with 
    | err -> None

let tryLoadARCFromScaffold (arcPath : string) = 
    try
        ARC.load(arcPath)
        |> Some
    with 
    | err -> None

let tryLoadARCFromAny (arcPath) =
    printfn "%s" $"## Loading ARC from {arcPath}"

    match tryLoadARCfromROCrate arcPath with
    | Some arc -> Some arc 
    | None ->
        printfn "%s" "### Could not load ARC from ROCrate \n --> trying to load ARC scaffold"; 
        match tryLoadARCFromScaffold arcPath with
        | Some arc -> Some arc 
        | None -> 
            printfn "%s" "### Could not load ARC from scaffold";
            None

// Generate html links
let generateHtmlLink (url : string) (text : string) =
    $"<a href='{url}'>{text}</a>"

// Build link to a study directory
let generateStudyLinkFromRoot (studyID : string) (arcRoot: string) =
    
    let relStudyPath = ArcPathHelper.getStudyFolderPath studyID

    let absStudyPath = System.IO.Path.Join(arcRoot, relStudyPath)
        
    generateHtmlLink absStudyPath studyID

let generateAssayLinkFromRoot (assayID : string) (arcRoot: string) =
    
    let relAssayPath = ArcPathHelper.getAssayFolderPath assayID

    let absAssayPath = System.IO.Path.Join(arcRoot, relAssayPath)
        
    generateHtmlLink absAssayPath assayID

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

let arcIsaProcesses2mermaid (arc : ARC) (outputFileName : string) (mmd: bool) =

    match mmd with
    | false ->
        let o = System.IO.Path.ChangeExtension(outputFileName, ".md")
        ["```mermaid"; createIsaMermaid arc; "```"]
        |> fun c -> System.IO.File.WriteAllLines(o, c)
    | true ->
        let o = System.IO.Path.ChangeExtension(outputFileName, ".mmd")
        [createIsaMermaid arc]
        |> fun c -> System.IO.File.WriteAllLines(o, c)
  

type CliArguments =
    | [<AltCommandLine("-a")>][<Unique>] Arcpath of path:string
    | [<AltCommandLine("-o")>][<Unique>] Outpath of path:string
    | [<AltCommandLine("-mmd")>][<Unique>] OutputMMD

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Arcpath _ -> "specify path to an ARC"
            | Outpath _ -> "specify a file path and name to write results to (Default: `./arc-mermaid`)"
            | OutputMMD -> "whether to output a .mmd file instead of markdown"

[<EntryPoint>]
let main(args) =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<CliArguments>(programName = "arcIsaProcessesSiren", errorHandler = errorHandler)

    let usage = parser.PrintUsage()

    printfn "%s" usage

    let results = parser.Parse (args)

    let mmd = results.Contains OutputMMD

    match results.TryGetResult(CliArguments.Arcpath) with
    | Some i ->

        let arc = tryLoadARCFromAny(i)

        match results.TryGetResult(CliArguments.Outpath) with
        | Some o -> 
            arcIsaProcesses2mermaid arc.Value o mmd
            1
        | None -> 
            printfn "Outpath missing; Defaulting to `./arc-mermaid.md`"
            let o = "arc-mermaid.md"
            arcIsaProcesses2mermaid arc.Value o mmd
            1
    | None ->
        printfn "Arcpath missing"
        0

