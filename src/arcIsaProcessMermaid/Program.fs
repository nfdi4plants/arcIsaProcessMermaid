module ArcIsaProcessMermaid.Program

open ARCtrl.NET
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

type CliArguments =
    | [<AltCommandLine("-p")>][<Unique>] Arcpath of path:string
    | [<AltCommandLine("-o")>][<Unique>] Outpath of path:string
    | [<AltCommandLine("-mmd")>][<Unique>] OutputMMD
    // | [<AltCommandLine("-fd")>][<Unique>] FlowDirection of string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Arcpath _ -> "specify path to an ARC"
            | Outpath _ -> "specify a file path and name to write results to (Default: `<path/to/ARC/arc-mermaid>.md or .mmd`)"
            | OutputMMD -> "whether to output a .mmd file instead of markdown"
            // | FlowDirection _ -> "(Default: `topDown`)"

[<EntryPoint>]
let main(args) =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<CliArguments>(programName = "arcIsaProcessesSiren", errorHandler = errorHandler)

    let usage = parser.PrintUsage()

    printfn "%s" usage

    let results = parser.Parse (args)

    let mmd = results.Contains OutputMMD

    // TODO: allow selection of flow direction via argument
    let flowD  =  Direction.TD
        // match results.TryGetResult(CliArguments.FlowDirection) with
        //     | Some o -> 
        //         o
        //     | None ->
                

    match results.TryGetResult(CliArguments.Arcpath) with
    | Some i ->

        let arc = tryLoadARCFromAny(i)

        match results.TryGetResult(CliArguments.Outpath) with
        | Some o -> 
            ArcSiren.arcIsaProcesses2mermaid flowD arc.Value o mmd
            1
        | None -> 
            printfn "Outpath missing; Defaulting to `<path/to/ARC/arc-mermaid>.md or .mmd`"

            let op = System.IO.Path.GetDirectoryName(i)
            let o = System.IO.Path.Join(op, "arc-mermaid")

            ArcSiren.arcIsaProcesses2mermaid flowD arc.Value o mmd
            1
    | None ->
        printfn "Arcpath missing"
        0