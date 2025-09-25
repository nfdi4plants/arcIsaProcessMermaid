# ArcIsaProcessMermaid.Core

Library to plot a mermaid graph based on ARC ISA processes.

## Use in script

```fsharp
#r "nuget: arcIsaProcessMermaid.Core, 1.0.10-alpha.2"

open ArcIsaProcessMermaid.Core
open ARCtrl

let arcPath = <path/to/ARC>

let arc = ARC.load(arcPath)

ArcSiren.createArcProcessMermaid Siren.Direction.LR true arc
```