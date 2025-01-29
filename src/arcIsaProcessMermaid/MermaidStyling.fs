namespace ArcIsaProcessMermaid

module MermaidStyling = 

    type MermaidClassDef =
        { 
            className   : string
            style       : MermaidStyle
        }

    and MermaidStyle =
        { 
            fill        : string
            color       : string
            fontWeight  : string
        }

    let createMermaidclassDef (m : MermaidClassDef) : string =
        $"classDef {m.className} fill:{m.style.fill},color:{m.style.color},font-weight:{m.style.fontWeight};"

    let assignMermaidClass (className : string) (classStates : string seq) =
        let concatStates = classStates |> String.concat(",")
        $"class {concatStates} {className};"