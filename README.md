# arcIsaProcessMermaid

Tool to generate a minimal markdown containing a mermaid graph that displays an ARC's connections of ISA processes from ARC investigation through studies and assays

:bulb: This tool is not meant as a long-term solution, but a quickfix and to illustrate what could readily and automatically be read from an ARC using the [ARCtrl](https://github.com/nfdi4plants/ARCtrl) library.

## Installation

Executables for macOS and Windows are available under [releases](https://github.com/nfdi4plants/arcIsaProcessMermaid/releases).

1. Download and store the executable somewhere useful (e.g. program files)
2. Add it to `$PATH` variable

:bulb: On first use, one has to grant permissions (Windows Defender or macOS security warning)

## Usage

Open a command line or terminal and run `arcIsaProcessMermaid -p local/path/to/ARC`.

```bash
USAGE: arcIsaProcessMermaid [--help] [--arcpath <path>] [--outpath <path>] [--outputmmd]

OPTIONS:

    --arcpath, -p <path>  specify path to an ARC
    --outpath, -o <path>  specify a file path and name to write results to (Default: `<path/to/ARC/arc-mermaid>.md or .mmd`)
    --outputmmd, -mmd     whether to output a .mmd file instead of markdown
    --help                display this list of options.
```

## Rationale

Sure, mermaid graphs are not the tool of choice to display the overall complexity of ARCs.
And they are far from anything dynamic or interactive.

However, [mermaids](https://mermaid.js.org/) in markdown

- are quick and can easily be adapted
- are static and should not break too easily (especially with type-safe F# library [Siren](https://www.nuget.org/packages/Siren))
- can readily be displayed in GitLab (i.e. [DataHUB](https://git.nfdi4plants.org)) or VSCode (with extensions)

## Recommended mermaid VS Code extensions

To display the resulting mermaid graph, preview the file in VS Code with extensions installed:

- [Markdown Preview Mermaid Support](https://marketplace.visualstudio.com/items?itemName=bierner.markdown-mermaid)
- [Mermaid Editor](https://marketplace.visualstudio.com/items?itemName=tomoyukim.vscode-mermaid-editor)

Alternatively, copy/paste it to [Mermaid Live Editor](http://mermaid.live). 

## Idea

Based on discussions before and during the [ARC-Process-GraphViz](https://github.com/nfdi4plants/ARC-Symposium/tree/main/ARC-Process-GraphViz/) project at ARC symposium 2024.
