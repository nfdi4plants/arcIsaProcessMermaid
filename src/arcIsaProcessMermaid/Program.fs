module ArcIsaProcessMermaid.Program

open ARCtrl
open ARCtrl.QueryModel
open Siren
open Argu
open System

open ArcUtils.Arcload
open ArcUtils.Arcpaths
open ArcProcesses
open MermaidStyling
open ArcSiren

//////////////////////////////////////////////////
////////// Handle CLI arguments

[<HelpFlags([|"--help"; "-h"|])>]

type CliArguments =
    | [<AltCommandLine("-p")>][<Unique>]    Arcpath of path:string
    | [<AltCommandLine("-o")>][<Unique>]    Outpath of path:string
    | [<AltCommandLine("-mmd")>][<Unique>]  OutputMMD
    | [<AltCommandLine("-fd")>][<Unique>]   FlowDirection of string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Arcpath _ -> "specify path to an ARC (ARC directory or `arc-ro-crate-metadata.json`)"
            | Outpath _ -> "specify a file path and name to write results to (Default: `<path/to/ARC/arc-mermaid>.md or .mmd`)"
            | FlowDirection _ -> "specify the direction of the flowchart: `topDown` (Default) or `leftRight`"
            | OutputMMD -> "whether to output a .mmd file instead of markdown"

[<EntryPoint>]
let main(args) =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<CliArguments>(programName = "arcIsaProcessMermaid", errorHandler = errorHandler)

    let usage = parser.PrintUsage()

    let results = parser.Parse (args)

    let mmd = results.Contains OutputMMD
  
    let flowD =
        match results.TryGetResult(CliArguments.FlowDirection) with
        | Some o -> 
            match o with
            | "topDown"   -> Direction.TD
            | "leftRight" -> Direction.LR
            | _ -> Direction.TD
        | None ->
            Direction.TD
                

    match results.TryGetResult(CliArguments.Arcpath) with
    | Some i ->

        let arc = tryLoadARCFromAny(i)

        match results.TryGetResult(CliArguments.Outpath) with
        | Some o ->          
            
            let op = System.IO.FileInfo(o).Directory.FullName
            
            System.IO.Directory.CreateDirectory(op) |> ignore

            ArcSiren.arcIsaProcesses2mermaid flowD arc.Value o mmd
            1
        | None -> 

            let op = System.IO.FileInfo(i).Directory.FullName            
            let o = System.IO.Path.Join(op, "arc-mermaid")

            printfn "%s" $"INFO: Outpath missing; Defaulting to {o}"

            ArcSiren.arcIsaProcesses2mermaid flowD arc.Value o mmd
            1
    | None ->
        printfn "%s" "---------------"
        printfn "%s" "ERROR: No Arcpath provided"
        printfn "%s" "---------------"
        printfn "%s" usage
        0