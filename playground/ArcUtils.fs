namespace ArcUtils

open ARCtrl.NET
open ARCtrl

// Load ARC from an RO-Crate file

module Arcload =

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

module Arcpaths =

    // Generate html links
    let generateHtmlLink (url : string) (text : string) =
        $"<a href='{url}'>{text}</a>"

    // Build link to a study directory
    
    let generateStudyLinkFromRoot (studyID : string) (arcRoot: string) =
        
        let relStudyPath = ArcPathHelper.getStudyFolderPath studyID

        let absStudyPath = System.IO.Path.Join(arcRoot, relStudyPath)
            
        generateHtmlLink absStudyPath studyID

    // Build link to a assay directory

    let generateAssayLinkFromRoot (assayID : string) (arcRoot: string) =
        
        let relAssayPath = ArcPathHelper.getAssayFolderPath assayID

        let absAssayPath = System.IO.Path.Join(arcRoot, relAssayPath)
            
        generateHtmlLink absAssayPath assayID
