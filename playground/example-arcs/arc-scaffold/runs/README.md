# CWL examples

:bulb: Requires installation of a CWL runner and docker  
&rarr; checkout CWL guides in the [DataPLANT Knowledge Base](https://nfdi4plants.github.io/nfdi4plants.knowledgebase/guides/arc-cwl/)

All three workflow examples below (re)create the file [*/result.csv](ARCMountResult/result.csv).

## with a docker container and a fixed script

```bash
cd runs/FixedScriptResult
cwltool run.cwl run.yml
```

## With a fixed script in a mounted arc

```bash
cd runs/ARCMountResult
cwltool run.cwl run.yml
```

## Within an ARC with a devcontainer

```bash
cd runs/DevContainerResult
cwltool run.cwl run.yml
```
