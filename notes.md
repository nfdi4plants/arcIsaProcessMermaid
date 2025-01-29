
# arcIsaProcessMermaid

## Project setup

### add solution

```bash
dotnet new sln --name arcIsaProcessMermaid
dotnet new console -lang "F#" -o src/arcIsaProcessMermaid
dotnet sln add src/arcIsaProcessMermaid/arcIsaProcessMermaid.fsproj
```

### add dependencies

```bash
cd src/arcIsaProcessMermaid/
dotnet add package Argu
dotnet add package ARCtrl.NET
dotnet add package ARCtrl.QueryModel
dotnet add package Siren
cd ../../
```

## Build project

```bash
dotnet build src/arcIsaProcessMermaid/arcIsaProcessMermaid.fsproj
```

## Test

```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid --help
```

### based on ARC scaffold

```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid -p tests/example-arcs/arc-scaffold/ -o pathToBeCreated/ArcPrototype
```

### based on RO-Crate

```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid -p tests/example-arcs/arc-ro-crate-metadata.json -o Facultative-CAM-in-Talinum
```

## Compile executables

```bash
dotnet publish --runtime win-x64 -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=true
dotnet publish --runtime osx-x64 -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=true
```