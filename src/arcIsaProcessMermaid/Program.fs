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

    let parser = ArgumentParser.Create<CliArguments>(programName = "arcIsaProcessMermaid", errorHandler = errorHandler)

    let usage = parser.PrintUsage()

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
            
            //// let op = System.IO.Path.GetDirectoryName(o)
            
            let op = System.IO.FileInfo(o).Directory.FullName
            
            System.IO.Directory.CreateDirectory(op) |> ignore

            ArcSiren.arcIsaProcesses2mermaid flowD arc.Value o mmd
            1
        | None -> 

            let op = System.IO.FileInfo(i).Directory.FullName            
            let o = System.IO.Path.Join(op, "arc-mermaid")

            printfn "%s" $"Outpath missing; Defaulting to {o}"

            ArcSiren.arcIsaProcesses2mermaid flowD arc.Value o mmd
            1
    | None ->
        printfn "Arcpath missing"
        0