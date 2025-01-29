cwlVersion: v1.2
class: Workflow

requirements:
  MultipleInputFeatureRequirement: {}

inputs:
  firstArg: File
  secondArg: string

steps:
  FixedScript:
    run: ../../workflows/FixedScript/workflow.cwl
    in:
      firstArg: firstArg
      secondArg: secondArg
    out: [output]

outputs:
  FixedScriptResult:
    type: File
    outputSource: FixedScript/output