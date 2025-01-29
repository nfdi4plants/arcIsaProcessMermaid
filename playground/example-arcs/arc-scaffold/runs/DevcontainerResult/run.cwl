cwlVersion: v1.2
class: Workflow

requirements:
  MultipleInputFeatureRequirement: {}

inputs:
  arcDirectory: Directory

steps:
  Devcontainer:
    run: ../../workflows/Devcontainer/workflow.cwl
    in:
      arcDirectory: arcDirectory
    out: [output]

outputs:
  DevcontainerResult:
    type: File
    outputSource: Devcontainer/output