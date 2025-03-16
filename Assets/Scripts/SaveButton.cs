using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public ConstructionManager constructionManager; // Referencia al ConstructionManager
    public string filePath = "Assets/structure.json";    // Ruta del archivo JSON

    public void Guardar()
    {
        constructionManager.SaveStructureToJson(filePath);
    }
    public void Cargar(){
        constructionManager.LoadStructureFromJson(filePath);
    }
}
