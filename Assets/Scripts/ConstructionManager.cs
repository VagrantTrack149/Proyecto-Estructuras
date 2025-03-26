using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using UnityEngine.SceneManagement;
public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance; // Instancia para usarse en el punto de anclaje

    public GameObject beamPrefab; // Prefab de la viga
    public GameObject anchorPointPrefab; // Prefab del punto de anclaje

    private AnchorPoint startAnchor; // Punto de anclaje inicial
    private GameObject tempBeam; // Viga temporal
    private AnchorPoint tempAnchor; // Punto de anclaje temporal (Propio)

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (startAnchor != null)
        {
            UpdateTempBeam();
        }

        if (Input.GetMouseButtonUp(0)) // Al soltar el clic
        {
            if (startAnchor != null)
            {
                FinishCreatingBeam();
            }
        }
    }

    public void StartCreatingBeam(AnchorPoint anchor)
    {
        startAnchor = anchor;

        // Crear viga temporal
        tempBeam = Instantiate(beamPrefab);
        Beam beamScript = tempBeam.GetComponent<Beam>();
        beamScript.startAnchor = startAnchor;

        // Crear punto de anclaje temporal
        tempAnchor = Instantiate(anchorPointPrefab).GetComponent<AnchorPoint>();
        tempAnchor.isFixed = false; // El punto de anclaje es propio de la viga
    }

    private void UpdateTempBeam()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Mover el punto de anclaje temporal al ratón
        tempAnchor.transform.position = mousePosition;

        // Actualizar la viga temporal
        Beam beamScript = tempBeam.GetComponent<Beam>();
        beamScript.endAnchor = tempAnchor;
    }

    private void FinishCreatingBeam()
    {
        // Buscar el punto de anclaje más cercano
        AnchorPoint endAnchor = FindClosestAnchor(tempAnchor.transform.position);

        if (endAnchor != null)
        {
            // Mover el punto de anclaje temporal a la posición del punto de anclaje más cercano
            tempAnchor.transform.position = endAnchor.transform.position;

            // Conectar la viga al punto de anclaje
            Beam beamScript = tempBeam.GetComponent<Beam>();
            beamScript.endAnchor = endAnchor;

            // Destruir el punto de anclaje temporal
            Destroy(tempAnchor.gameObject);
        }
        else
        {
            // Fijar el punto de anclaje temporal en su posición actual
            tempAnchor.isFixed = true;
        }

        // Congela la escala de la viga
        if (tempBeam != null)
        {
            Beam beamScript = tempBeam.GetComponent<Beam>();
            beamScript.FreezeScale(); // Aquí se congela la escala
        }

        startAnchor = null;
        tempBeam = null;
        tempAnchor = null;
    }

    private AnchorPoint FindClosestAnchor(Vector2 position)
    {
        float minDistance = 1f; // Distancia máxima para conectar a un anchorPoint
        AnchorPoint closestAnchor = null;

        foreach (AnchorPoint anchor in FindObjectsOfType<AnchorPoint>())
        {
            if (anchor != startAnchor && anchor.isFixed)
            {
                float distance = Vector2.Distance(position, anchor.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestAnchor = anchor;
                }
            }
        }

        return closestAnchor;
    }

    public void SaveStructureToJson(string filePath)
    {
        // Crear una instancia de StructureData
        StructureData structureData = new StructureData();
        structureData.beams = new List<BeamData>();

        // Obtener todas las vigas en la escena
        Beam[] beams = FindObjectsOfType<Beam>();

        // Recopilar los datos de cada viga y asignar un ID
        for (int i = 0; i < beams.Length; i++)
        {
            BeamData beamData = beams[i].GetBeamData();
            beamData.id = i + 1; // Asignar un ID único (comenzando desde 1)
            structureData.beams.Add(beamData);
        }

        // Convertir a JSON
        string json = JsonUtility.ToJson(structureData, true); // El segundo parámetro (true) formatea el JSON para que sea legible

        // Guardar el JSON en un archivo
        File.WriteAllText(filePath, json);

        Debug.Log("Estructura guardada en: " + filePath);
    }

    public void LoadStructureFromJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            // Leer el archivo JSON
            string json = File.ReadAllText(filePath);

            // Deserializar el JSON a StructureData
            StructureData structureData = JsonUtility.FromJson<StructureData>(json);

            // Recrear las vigas en la escena
            foreach (BeamData beamData in structureData.beams)
            {
                // Crear puntos de anclaje
                AnchorPoint startAnchor = CreateAnchorPoint(beamData.startAnchorPosition);
                AnchorPoint endAnchor = CreateAnchorPoint(beamData.endAnchorPosition);

                // Crear la viga
                GameObject beamObject = Instantiate(beamPrefab);
                Beam beam = beamObject.GetComponent<Beam>();
                beam.startAnchor = startAnchor;
                beam.endAnchor = endAnchor;

                // Aplicar la escala y rotación
                beamObject.transform.localScale = new Vector3(beamData.scaleX, beamObject.transform.localScale.y, beamObject.transform.localScale.z);
                beamObject.transform.rotation = Quaternion.Euler(0, 0, beamData.angle);
            }

            Debug.Log("Estructura cargada desde: " + filePath);
        }
        else
        {
            Debug.LogError("El archivo no existe: " + filePath);
        }
    }

    private AnchorPoint CreateAnchorPoint(Vector2 position)
    {
        GameObject anchorObject = Instantiate(anchorPointPrefab, position, Quaternion.identity);
        return anchorObject.GetComponent<AnchorPoint>();
    }
    public void Salir(){
        SceneManager.LoadScene(0);
    }
}
