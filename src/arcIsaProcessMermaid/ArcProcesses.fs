namespace ArcIsaProcessMermaid

open ARCtrl
open ARCtrl.QueryModel

module ArcProcesses = 

    // Determine whether one process precedes another
    // based on min 1 intersecting Input/Output reference
    let isPreviousProcessOf (processA: ArcTable) (processB: ArcTable) : bool =    
    
        match processB.TryGetInputColumn() with
            | Some a -> 

                match processA.TryGetOutputColumn() with 

                | Some a -> 

                    Set.intersect (set processA.OutputNames) (set processB.InputNames)
                        |> Seq.length
                        |> fun x -> x > 0

                | None -> 

                    printfn "%s" $"INFO: No Output column found in {processA.Name}"
                    false

            | None -> 

                printfn "%s" $"INFO: No Input column found in {processB.Name}"
                false

    // Count the number of intersections
    let numSamplesFromPreviousProcess (processA: ArcTable) (processB: ArcTable) : int = 
                    
        match processB.TryGetInputColumn() with
            | Some a -> 

                match processA.TryGetOutputColumn() with 

                | Some a -> 

                    Set.intersect (set processA.OutputNames) (set processB.InputNames)
                    |> Seq.length

                | None -> 

                    printfn "%s" $"INFO: No Output column found in {processA.Name}"
                    0

            | None -> 

                printfn "%s" $"INFO: No Input column found in {processB.Name}"
                0

