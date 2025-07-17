
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
dotnet add package ARCtrl
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
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid
```

### based on ARC scaffold

```bash
src/arcIsaProcessMermaid/bin/Debug/net8.0/arcIsaProcessMermaid -p tests/example-arcs/arc-scaffold/ -mmd -o pathToBeCreated/ArcPrototype -fd leftRight
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
cd src/arcIsaProcessMermaid
dotnet run -- --help
```

### Publish to nuget

```bash
# nuget setApiKey <>
nuget push src/arcIsaProcessMermaid/bin/Release/arcIsaProcessMermaid.1.0.8.nupkg -Source https://api.nuget.org/v3/index.json
```

### Install tool locally

```bash
cd src
dotnet new tool-manifest
dotnet tool install --tool-path /usr/local/bin --add-source ./arcIsaProcessMermaid/bin/Release/ arcIsaProcessMermaid
```
