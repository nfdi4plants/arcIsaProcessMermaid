//////////////////////////////////////////////////
////////// Add style to nodes
/// TODO: needs to be more generalized and less hard-coded
/// - allow more css
/// - make css itmes optional

module MermaidStyling

type mermaidClassDef =
    { 
        className   : string
        style       : mermaidStyle
    }

and mermaidStyle =
    { 
        fill        : string
        color       : string
        fontWeight  : string
    }

let createMermaidclassDef (m : mermaidClassDef) : string =

    $"classDef {m.className} fill:{m.style.fill},color:{m.style.color},font-weight:{m.style.fontWeight};"


let assignMermaidClass (className : string) (classStates : string seq) =
    
    let concatStates = classStates |> String.concat(",")
    
    $"class {concatStates} {className};"

