#r "nuget: ARCtrl"
#r "nuget: ARCtrl.NET"
#r "nuget: ARCtrl.QueryModel"


#load "ArcUtils.fs"
#load "ArcProcesses.fs"
#load "Styling.fs"

open ArcUtils
open ArcProcesses

let arc = Arcload.tryLoadARCFromAny "playground/arc-ro-crate-metadata.json"

Arcpaths.generateAssayLinkFromRoot "sqldjasd" "."


