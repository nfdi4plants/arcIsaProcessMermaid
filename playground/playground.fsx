
#r "nuget: ARCtrl.NET"
#r "nuget: ARCtrl.QueryModel"
#r "nuget: Siren"
#r "nuget: FSharp.Data"

open FSharp.Data
open ARCtrl.NET
open ARCtrl
open ARCtrl.QueryModel
open Siren


[
siren.flowchart(direction.leftToRight, [
    flowchart.node("id", "label")
    flowchart.subgraphNamed("subgraphID", "label", [])
    ])
|> siren.write
]
|> fun c -> System.IO.File.WriteAllLines("test.mmd", c)

