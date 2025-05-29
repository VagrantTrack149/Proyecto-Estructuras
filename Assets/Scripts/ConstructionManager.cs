using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using UnityEngine.SceneManagement;
public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance;
    public GameObject beamPrefab;
    public GameObject anchorPointPrefab;
    
    private AnchorPoint startAnchor;
    private GameObject tempBeam;
    private AnchorPoint tempAnchor;
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

        if (Input.GetMouseButtonUp(0))
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
        tempBeam = Instantiate(beamPrefab);
        tempBeam.GetComponent<Beam>().startAnchor = startAnchor;
        tempAnchor = Instantiate(anchorPointPrefab).GetComponent<AnchorPoint>();
        tempAnchor.isFixed = false;
    }

    private void UpdateTempBeam()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempAnchor.transform.position = mousePosition;
        tempBeam.GetComponent<Beam>().endAnchor = tempAnchor;
    }

    private void FinishCreatingBeam()
    {
        AnchorPoint endAnchor = FindClosestAnchor(tempAnchor.transform.position);
        
        if (endAnchor != null)
        {
            tempAnchor.transform.position = endAnchor.transform.position;
            tempBeam.GetComponent<Beam>().endAnchor = endAnchor;
            Destroy(tempAnchor.gameObject);
        }
        else
        {
            tempAnchor.isFixed = true;
        }

        tempBeam.GetComponent<Beam>().FreezeScale();
        startAnchor = null;
        tempBeam = null;
        tempAnchor = null;
    }

    private AnchorPoint FindClosestAnchor(Vector2 position)
    {
        float minDistance = 1f;
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
        StructureData structureData = new StructureData();
        structureData.beams = new List<BeamData>();

        Beam[] beams = FindObjectsOfType<Beam>();
        for (int i = 0; i < beams.Length; i++)
        {
            BeamData beamData = beams[i].GetBeamData();
            beamData.id = i + 1;
            structureData.beams.Add(beamData);
        }

        string json = JsonUtility.ToJson(structureData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Estructura guardada en: " + filePath);
    }

    public void LoadStructureFromJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StructureData structureData = JsonUtility.FromJson<StructureData>(json);

            // Limpiar estructura existente
            foreach (Beam beam in FindObjectsOfType<Beam>())
            {
                Destroy(beam.gameObject);
            }
            foreach (AnchorPoint anchor in FindObjectsOfType<AnchorPoint>())
            {
                if (!anchor.isFixed) Destroy(anchor.gameObject);
            }

            // Crear nueva estructura
            Dictionary<int, AnchorPoint> anchorDict = new Dictionary<int, AnchorPoint>();
            
            foreach (BeamData beamData in structureData.beams)
            {
                AnchorPoint startAnchor = CreateOrGetAnchor(beamData.startAnchorPosition, anchorDict);
                AnchorPoint endAnchor = CreateOrGetAnchor(beamData.endAnchorPosition, anchorDict);

                GameObject beamObject = Instantiate(beamPrefab);
                Beam beam = beamObject.GetComponent<Beam>();
                beam.startAnchor = startAnchor;
                beam.endAnchor = endAnchor;
                beam.weight = beamData.weight;
                beam.resistivity = beamData.resistivity;

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

    private AnchorPoint CreateOrGetAnchor(Vector2 position, Dictionary<int, AnchorPoint> anchorDict)
    {
        foreach (KeyValuePair<int, AnchorPoint> entry in anchorDict)
        {
            if (Vector2.Distance(entry.Value.transform.position, position) < 0.1f)
            {
                return entry.Value;
            }
        }

        GameObject anchorObject = Instantiate(anchorPointPrefab, position, Quaternion.identity);
        AnchorPoint newAnchor = anchorObject.GetComponent<AnchorPoint>();
        anchorDict.Add(anchorDict.Count + 1, newAnchor);
        return newAnchor;
    }

    public void Salir()
    {
        SceneManager.LoadScene(0);
    }
}
