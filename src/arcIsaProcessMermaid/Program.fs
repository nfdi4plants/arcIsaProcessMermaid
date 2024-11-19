open ARCtrl.NET
open ARCtrl
open ARCtrl.QueryModel
open Siren
open Argu

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

// TODO: fix .Replace(" ", "-") to easily handle blanks in process names

// Draw connections as mermaid string

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
                       
            // add links to studies
            flowchart.linkArrow(investigation.Identifier, sid)
            
            // add study subgraphs
            flowchart.subgraph(sid, [

                for p in s do
                    flowchart.node(p.Name.Replace(" ", "-"))            
                    ])

        // add assay subgraphs

        for a in assays do
            flowchart.subgraph("Assay:" + a.Identifier, [

                for p in a do
                    flowchart.node(p.Name.Replace(" ", "-"))            
                    ])

        // add process-to-process edges, with sample numbers as edge name

        for p1 in processes do
                for p2 in processes do

                    if isPreviousProcessOf p1 p2 then
                        let nSamples = numSamplesFromPreviousProcess p1 p2
                        flowchart.linkArrow(p1.Name.Replace(" ", "-"), p2.Name.Replace(" ", "-"), nSamples.ToString())

    ])
    |> siren.write

// Load ARC and write mermaid to markdown file

let arcIsaProcesses2mermaid (arcPath : string) (outputFileName : string)  = 
    let arc = ARC.load(arcPath)
    [
    "```mermaid"
    createIsaMermaid arc
    "```"
    ]
    |> fun c -> System.IO.File.WriteAllLines(outputFileName, c)




type CliArguments =
    | [<AltCommandLine("-a")>][<Unique>] Arcpath of path:string
    | [<AltCommandLine("-o")>][<Unique>] Outfile of path:string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Arcpath _ -> "Specify path to an ARC"
            | Outfile _ -> "Specify a (text) file to write results to (Default: `arc-mermaid.md`)"

[<EntryPoint>]
let main(args) =

    let parser = ArgumentParser.Create<CliArguments>()

    let results = parser.Parse (args)

    match results.TryGetResult(CliArguments.Arcpath) with
    | Some i -> 
        match results.TryGetResult(CliArguments.Outfile) with
        | Some o -> 
            arcIsaProcesses2mermaid i o
            1
        | None -> 
            printfn "Outfile missing; Defaulting to `arc-mermaid.md`"
            let o = "arc-mermaid.md"
            arcIsaProcesses2mermaid i o
            1
    | None ->
        printfn "Arcpath missing"
        0