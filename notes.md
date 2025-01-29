
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

## publish executables

```bash
dotnet publish --runtime osx-x64 -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=true
dotnet publish --runtime win-x64 -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=true
```