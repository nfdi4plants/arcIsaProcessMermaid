module ArcProcesses

open ARCtrl
open ARCtrl.QueryModel

// Determine whether one process precedes another
// based on min 1 intersecting Input/Output reference

let isPreviousProcessOf (processA: ArcTable) (processB: ArcTable) : bool = 
    Set.intersect (set processA.OutputNames) (set processB.InputNames)
    |> Seq.length
    |> fun x -> x > 0

// Count the number of intersections

let numSamplesFromPreviousProcess (processA: ArcTable) (processB: ArcTable) : int = 
    Set.intersect (set processA.OutputNames) (set processB.InputNames)
    |> Seq.length