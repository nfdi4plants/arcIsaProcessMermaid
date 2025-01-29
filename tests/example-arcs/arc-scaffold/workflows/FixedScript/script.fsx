#r "nuget: FSharp.Data, 5.0.2"

open FSharp.Data
open System.IO

let args : string array = fsi.CommandLineArgs |> Array.tail
let first = args.[0]
let second = args.[1]

let data =
    CsvFile
        .Load(first)
        .Cache()
        
let r =
    [
        yield "Sum_1-2"
        for row in data.Rows do
            yield sprintf "%f"  ((row.["value_1"].AsFloat()) + (row.["value_2"].AsFloat()))
    ]

System.IO.File.WriteAllLines(second,r)