cwlVersion: v1.2
class: CommandLineTool
hints:
  DockerRequirement:
    dockerPull: mcr.microsoft.com/dotnet/sdk:6.0
requirements:
  - class: InitialWorkDirRequirement
    listing:
      - entryname: script.fsx
        entry:
          $include: script.fsx
  - class: EnvVarRequirement
    envDef:
      - envName: DOTNET_NOLOGO
        envValue: "true"
  - class: NetworkAccess
    networkAccess: true
baseCommand: [dotnet, fsi, script.fsx]
inputs:
  firstArg:
    type: File
    inputBinding:
      position: 1
  secondArg:
    type: string
    inputBinding:
      position: 2

outputs:
  output:
    type: File
    outputBinding:
      glob: "*.csv"

arc:has technology type:
  - class: arc:technology type
    arc:annotation value: "Docker Container"

arc:technology platform: ".NET"

arc:performer:
  - class: arc:Person
    arc:first name: "Timo"
    arc:last name: "Mühlhaus"
    arc:email: "muehlhaus@bio.uni-kl.de "
    arc:affiliation: "RPTU Kaiserslautern/Landau"
    arc:has role: 
      - class: arc:role
        arc:term accession: "https://credit.niso.org/contributor-roles/formal-analysis/"
        arc:annotation value: "Formal analysis"

arc:has process sequence:
  - class: arc:process sequence
    arc:name: "script.fsx"
    arc:has parameter value: 
      - class: arc:process parameter value
        arc:has parameter:
          - class: arc:protocol parameter
            arc:has parameter name: 
            - class: arc:parameter name
              arc:term accession: "http://purl.obolibrary.org/obo/NCIT_C43582"
              arc:term source REF: "NCIT"
              arc:annotation value: "Data Transformation"
        arc:value: 
          - class: arc:ontology annotation
            arc:term accession: "http://purl.obolibrary.org/obo/NCIT_C64911"
            arc:term source REF: "NCIT"
            arc:annotation value: "Addition"


$namespaces:
  arc: https://github.com/nfdi4plants/ARC_ontology

$schemas:
  - https://raw.githubusercontent.com/nfdi4plants/ARC_ontology/main/ARC_v2.0.owl