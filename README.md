# arcIsaProcessMermaid

Tool to generate a minimal markdown containing a mermaid graph that displays an ARC's connections of ISA processes from ARC investigation through studies and assays

## Draw example mermaids

### based on ARC scaffold

```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid -p playground/example-arcs/arc-scaffold -o playground/ArcPrototype
```

### based on RO-Crate

```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid -p ./playground/example-arcs/arc-ro-crate-metadata.json -o playground/Facultative-CAM-in-Talinum
```

## Rationale

Sure, mermaid graphs are not the tool of choice to display the overall complexity of ARCs.
And they are far from anything dynamic or interactive.

However, mermaids in markdown

- are quick and can easily be adapted
- are static and should not break too easily (especially with type-safe F# library [Siren](https://www.nuget.org/packages/Siren))
- can readily be displayed in GitLab (i.e. [DataHUB](https://git.nfdi4plants.org)) or VSCode (with extensions)

## Idea

Based on discussions before and during the [ARC-Process-GraphViz](https://github.com/nfdi4plants/ARC-Symposium/tree/main/ARC-Process-GraphViz/) project at ARC symposium 2024.
