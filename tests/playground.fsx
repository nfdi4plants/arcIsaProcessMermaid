
#r "nuget: ARCtrl.NET"
#r "nuget: ARCtrl.QueryModel"
#r "nuget: Siren"
#r "nuget: FSharp.Data"

open FSharp.Data
open ARCtrl.NET
open ARCtrl
open ARCtrl.QueryModel
open Siren


// let id = "id"
// let link = "<a href='google.com'>google</a>"

// [
// siren.flowchart(direction.leftToRight, [
//     flowchart.node(id, link)
//     ])
// |> siren.write
// ]
// |> fun c -> System.IO.File.WriteAllLines("test.mmd", c)



// Load ARC from an RO-Crate file
let loadARCfromROCrate (arcRocPath : string) = 
    JsonValue.Load(arcRocPath)
        |> string
        |> ARC.fromROCrateJsonString


let arcRocPath = "2024-11-26T14-38-14_datahubArcId1974_rocrate.json"
let arcRocFullPath = System.IO.Path.Join(__SOURCE_DIRECTORY__, arcRocPath)
let arc = loadARCfromROCrate(arcRocFullPath)


let investigation = arc.ISA.Value
        
let studies = investigation.Studies

let generateHtmlLink (url : string) (text : string) =
    $"<a href='{url}'>{text}</a>"

let createStudyLinkFromRoot (studyID : string) (arcRoot: string) =
    
    let filePath = System.IO.Path.Join(arcRoot, "studies", studyID)
    
    generateHtmlLink filePath studyID



createStudyLinkFromRoot studies.[0].Identifier "."

[
siren.flowchart(direction.leftToRight, [
    flowchart.node(id, link)
    ])
|> siren.write
]
|> fun c -> System.IO.File.WriteAllLines("test.mmd", c)

