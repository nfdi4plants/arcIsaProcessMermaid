
# arcIsaProcessMermaid

## add solution

```bash
dotnet new sln --name arcIsaProcessMermaid
dotnet new console -lang "F#" -o src/arcIsaProcessMermaid
dotnet sln add src/arcIsaProcessMermaid/arcIsaProcessMermaid.fsproj
```

## add dependencies

```bash
cd src/arcIsaProcessMermaid/
dotnet add package Argu
dotnet add package ARCtrl.NET
dotnet add package ARCtrl.QueryModel
dotnet add package Siren
cd ../../
```

## build project

```bash
dotnet build src/arcIsaProcessMermaid/arcIsaProcessMermaid.fsproj
```

## start app

```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid --help
```

## local test

### using path to ARC scaffold
```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid -a ~/datahub-dataplant/Facultative-CAM-in-Talinum/
```

### using path to ARC RO-Crate

```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid -a  "./tests/arc-ro-crate-metadata.json"
```

## publish macos

```bash
dotnet publish --runtime osx-x64 -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=true
```

## add to local bin

```bash
chmod a+x src/arcIsaProcessMermaid/bin/Release/net8.0/osx-x64/publish/arcIsaProcessMermaid
cp src/arcIsaProcessMermaid/bin/Release/net8.0/osx-x64/publish/arcIsaProcessMermaid /usr/local/bin
```
