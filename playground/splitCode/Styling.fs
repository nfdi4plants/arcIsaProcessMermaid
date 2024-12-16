//////////////////////////////////////////////////
////////// Add style to nodes
/// TODO: needs to be generalized and not hard-coded

module Styling

type mermaidClassDef =
    {
        className   : string
        fill        : string
        color       : string
        fontWeight  : string
    }


let createMermaidclassDef (m : mermaidClassDef) : string =
    
    $"classDef {m.className} fill:{m.fill},color:{m.color},font-weight:{m.fontWeight};"


let inv = 
    {
        className   = "inv"
        fill        = "#6c7885"
        color       = "#ECEBEB"
        fontWeight  = "bold"
    }

let classDefInv = createMermaidclassDef inv
let classDefStudy = "classDef study fill:#62d4c1,color:#2d3e50,font-weight:bold;"
let classDefAssay = "classDef assay fill:#ffd34d,color:#2d3e50,font-weight:bold;"
let classDefProcess = "classDef process fill:#D46275,color:#2d3e50;"

let assignMermaidClass (className : string) (classStates : string seq) =
    
    let concatStates = classStates |> String.concat(",")
    
    $"class {concatStates} {className};"