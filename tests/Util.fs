[<AutoOpen>]
module Tests.Util

open ARCtrl

module Expect =
    open Expecto
    open System

    // /// <summary>
    // /// This function only verifies non-whitespace characters
    // /// </summary>
    // let stringEqual actual expected message =
    //     let pattern = @"\s+"
    //     let regex = System.Text.RegularExpressions.Regex(pattern, Text.RegularExpressions.RegexOptions.Singleline)
    //     let actual = regex.Replace(actual, "")
    //     let expected = regex.Replace(expected, "")
    //     let mutable isSame = true
    //     Seq.iter2 
    //         (fun s1 s2 -> 
    //             if isSame && s1 = s2 then 
    //                 ()
    //             elif isSame && s1 <> s2 then
    //                 isSame <- false
    //                 printfn "%s" (string s1)
    //             else
    //                 printfn "%s" (string s1)
    //         ) 
    //         actual 
    //         expected
    //     Expect.equal actual expected message

    /// Trims whitespace and normalizes lineendings to "\n"
    let trimEqual (actual: string) (expected: string) message =
        let a = actual.Trim().Replace("\r\n", "\n")
        let e = expected.Trim().Replace("\r\n", "\n")
        Expect.equal a e message

let loadArc (relativePath: string) =
    let root = System.IO.Path.GetFullPath __SOURCE_DIRECTORY__
    let p = System.IO.Path.Combine(root, relativePath)
    ARC.load p