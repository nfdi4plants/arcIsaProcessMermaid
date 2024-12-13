#r "nuget: ARCtrl.NET"

#load "Arcutils.fs"

open Arcutils

Arcload.tryLoadARCFromAny "playground/arc-ro-crate-metadata.json"

Arcpaths.generateAssayLinkFromRoot "sqldjasd" "."
