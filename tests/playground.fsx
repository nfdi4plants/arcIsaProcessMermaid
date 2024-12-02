
#r "nuget: ARCtrl.NET"
#r "nuget: ARCtrl.QueryModel"
#r "nuget: Siren"

open ARCtrl.NET
open ARCtrl
open ARCtrl.QueryModel
open Siren


let id = "id"
let link = "<a href='google.com'>google</a>"

[
siren.flowchart(direction.leftToRight, [
    flowchart.node(id, link)
    ])
|> siren.write
]
|> fun c -> System.IO.File.WriteAllLines("test.mmd", c)