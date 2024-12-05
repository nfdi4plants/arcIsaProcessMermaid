#r "nuget: FSharp.Data, 5.0.2"

open FSharp.Data
open System.IO

printfn "%A" (System.IO.Directory.GetCurrentDirectory())

let data =
    CsvFile
        .Load(Path.Combine(System.IO.Directory.GetCurrentDirectory(),"./arc/assays/measurement1/dataset/proteomics_result.csv"))
        .Cache()
        
let r =
    [
        yield "Sum_1-2"
        for row in data.Rows do
            yield sprintf "%f"  ((row.["value_1"].AsFloat()) + (row.["value_2"].AsFloat()))
    ]

System.IO.File.WriteAllLines("./arc/runs/ARCMountResult/result.csv",r)