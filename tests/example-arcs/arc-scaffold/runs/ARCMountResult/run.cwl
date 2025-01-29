cwlVersion: v1.2
class: Workflow

requirements:
  MultipleInputFeatureRequirement: {}

inputs:
  arcDirectory: Directory

steps:
  ARCMount:
    run: ../../workflows/ARCMount/workflow.cwl
    in:
      arcDirectory: arcDirectory
    out: [output]

outputs:
  ARCMountResult:
    type: File
    outputSource: ARCMount/output