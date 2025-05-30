using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveButton : MonoBehaviour
{
    public GameObject[] anchorPoint;
    public ConstructionManager constructionManager; // Referencia al ConstructionManager
    public string filePath = "structure.json";    // Ruta del archivo JSON
    public string Limpia ="limpio.json";
    public GameObject panelLimpiar;
    public void Guardar()
    {
        constructionManager.SaveStructureToJson(filePath);
    }
    public void Cargar(){
        constructionManager.LoadStructureFromJson(filePath);
    }
    public void PanelActivar(){
        panelLimpiar.SetActive(true); 
    }
    public void PanelDesactivar(){
        panelLimpiar.SetActive(false); 
    }
    public void Limpiar(){
        constructionManager.LoadStructureFromJson(Limpia);
        anchorPoint=GameObject.FindGameObjectsWithTag("AnchorPoint");
        
        for (int i = 0; i < anchorPoint.Length; i++){
            Destroy(anchorPoint[i]);
        }
        panelLimpiar.SetActive(false); 
    }
}
