```mermaid
flowchart TD
    ArcPrototype[ArcPrototype]
    ArcPrototype-->STUDY_MaterialPreparation
    subgraph STUDY_MaterialPreparation[Study:MaterialPreparation]
        CellCultivation[CellCultivation]
        AccessoryDataRetrieval[AccessoryDataRetrieval]
    end
    ArcPrototype-->STUDY_experiment2
    subgraph STUDY_experiment2[Study:experiment2]
        experiment2[experiment2]
    end
    subgraph ASSAY_measurement1[Assay:measurement1]
        Cell_Lysis[Cell Lysis]
        Protein_Extraction[Protein Extraction]
        Protein_Measurement[Protein Measurement]
        Computational_Proteome_Analysis[Computational Proteome Analysis]
    end
    subgraph ASSAY_measurement2[Assay:measurement2]
    end
    CellCultivation-->|6|Cell_Lysis
    Cell_Lysis-->|6|Protein_Extraction
    Protein_Extraction-->|6|Protein_Measurement
    Protein_Measurement-->|6|Computational_Proteome_Analysis

classDef inv fill:#6c7885,color:#2d3e50,font-weight:bold;
classDef study fill:#62d4c1,color:#2d3e50,font-weight:bold;
classDef assay fill:#ffd34d,color:#2d3e50,font-weight:bold;
classDef process fill:#D46275,color:#2d3e50,font-weight:;
class ArcPrototype inv;
class STUDY_MaterialPreparation,STUDY_experiment2 study;
class ASSAY_measurement1,ASSAY_measurement2 assay;
class CellCultivation,AccessoryDataRetrieval,experiment2,Cell_Lysis,Protein_Extraction,Protein_Measurement,Computational_Proteome_Analysis process;
```
