#r "nuget: ARCtrl"
#r "nuget: ARCtrl.NET"
#r "nuget: ARCtrl.QueryModel"
#r "nuget: Siren"
#r "nuget: Argu"

#load "ArcUtils.fs"
#load "ArcProcesses.fs"
#load "MermaidStyling.fs"
#load "ArcSiren.fs"

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
    | [<AltCommandLine("-a")>][<Unique>] Arcpath of path:string
    | [<AltCommandLine("-o")>][<Unique>] Outpath of path:string
    | [<AltCommandLine("-mmd")>][<Unique>] OutputMMD
    // | [<AltCommandLine("-fd")>][<Unique>] FlowDirection of string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Arcpath _ -> "specify path to an ARC"
            | Outpath _ -> "specify a file path and name to write results to (Default: `./arc-mermaid`)"
            | OutputMMD -> "whether to output a .mmd file instead of markdown"
            // | FlowDirection _ -> "(Default: `topDown`)"


// [<EntryPoint>]
let main(args) =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<CliArguments>(programName = "arcIsaProcessesSiren", errorHandler = errorHandler)

    let usage = parser.PrintUsage()

    printfn "%s" usage

    let results = parser.Parse (args)

    let mmd = results.Contains OutputMMD

    // TODO: allow selection of flow direction via argument
    let flowD  =  direction.topDown
        // match results.TryGetResult(CliArguments.FlowDirection) with
        //     | Some o -> 
        //         o
        //     | None ->
                

    match results.TryGetResult(CliArguments.Arcpath) with
    | Some i ->

        let arc = tryLoadARCFromAny(i)

        match results.TryGetResult(CliArguments.Outpath) with
        | Some o -> 
            arcIsaProcesses2mermaid flowD arc.Value o mmd
            1
        | None -> 
            printfn "Outpath missing; Defaulting to `./arc-mermaid.md`"
            let o = "arc-mermaid.md"
            arcIsaProcesses2mermaid flowD arc.Value o mmd
            1
    | None ->
        printfn "Arcpath missing"
        0


////////////////////////
/// Test


let args = [| 
            "--arcpath"; "playground/arc-ro-crate-metadata.json" ; 
            "--outpath"; "playground/test.md";
            // "-mmd"; "false";
            |]

main args