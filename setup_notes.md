
# arcIsaProcessMermaid

## Project setup

### add solution

```bash
dotnet new sln --name arcIsaProcessMermaid --force

dotnet new classlib -lang "F#" -o src/ArcIsaProcessMermaid.Core
dotnet new console -lang "F#" -o src/ArcIsaProcessMermaid.Tool

dotnet sln arcIsaProcessMermaid.sln add src/ArcIsaProcessMermaid.Core/ArcIsaProcessMermaid.Core.fsproj
dotnet sln arcIsaProcessMermaid.sln add src/ArcIsaProcessMermaid.Tool/ArcIsaProcessMermaid.Tool.fsproj
dotnet sln arcIsaProcessMermaid.sln add tests/Tests.fsproj


```

### add dependencies

```bash
cd src/arcIsaProcessMermaid/
dotnet add package Argu
dotnet add package ARCtrl
dotnet add package ARCtrl.QueryModel
dotnet add package Siren
cd ../../
```

## Build and pack

Here done explicitly. Since `<BuildOnPack>true</BuildOnPack>` is added to the .fsproj's, the `dotnet pack` below suffices.

```bash
dotnet build src/ArcIsaProcessMermaid.Core/ArcIsaProcessMermaid.Core.fsproj -c Release
dotnet pack src/ArcIsaProcessMermaid.Core/ArcIsaProcessMermaid.Core.fsproj -c Release

dotnet build src/ArcIsaProcessMermaid.Tool/ArcIsaProcessMermaid.Tool.fsproj -c Release
dotnet pack src/ArcIsaProcessMermaid.Tool/ArcIsaProcessMermaid.Tool.fsproj -c Release
```

## Test

```bash
src/arcIsaProcessMermaid.Tool/bin/Debug/net8.0/ArcIsaProcessMermaid.Tool
```

### based on ARC scaffold

```bash
src/arcIsaProcessMermaid.Tool/bin/Debug/net8.0/ArcIsaProcessMermaid.Tool -p tests/example-arcs/arc-scaffold/ -mmd -o pathToBeCreated/ArcPrototype -fd leftRight
```

### based on RO-Crate

```bash
src/arcIsaProcessMermaid.Tool/bin/Debug/net8.0/ArcIsaProcessMermaid.Tool -p tests/example-arcs/arc-ro-crate-metadata.json -o Facultative-CAM-in-Talinum
```

## Pack via dotnet and publish on nuget

### Adapt .fsproj file

adapt `src/arcIsaProcessMermaid/arcIsaProcessMermaid.fsproj` to include:

```xml
  <PropertyGroup>
    <VersionPrefix>1.0.1</VersionPrefix>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <PackAsTool>true</PackAsTool>
    <ToolCommandName>arcIsaProcessMermaid</ToolCommandName>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <PackageLicenseFile>LICENSE</PackageLicenseFile>
  </PropertyGroup>

    ...

  <ItemGroup>
    ...
    <None Include="../../README.md" Pack="true" PackagePath=""/>
    <None Include="../../LICENSE" Pack="true" PackagePath=""/>
  </ItemGroup>

```

### Pack tool

```bash
dotnet pack
```

### Test

```bash
cd src/arcIsaProcessMermaid.Tool
dotnet run --framework net8.0 -- --help
dotnet run --framework net9.0 -- --help
```

### Publish to nuget

```bash
# nuget setApiKey <>
nuget push src/arcIsaProcessMermaid.Tool/bin/Release/arcIsaProcessMermaid.1.0.10-alpha.2.nupkg -Source https://api.nuget.org/v3/index.json
nuget push src/ArcIsaProcessMermaid.Core/bin/Release/arcIsaProcessMermaid.Core.1.0.10-alpha.2.nupkg -Source https://api.nuget.org/v3/index.json
```

### Install tool locally

```bash
cd src
dotnet new tool-manifest
dotnet tool install --tool-path /usr/local/bin --add-source ./arcIsaProcessMermaid/bin/Release/ arcIsaProcessMermaid
```
