using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BeamData
{
    public int id;
    public Vector2 startAnchorPosition;
    public Vector2 endAnchorPosition;
    public float angle;
    public float scaleX;
    public float weight = 1.0f;
    public float resistivity = 1.5f;
}

public class Beam : MonoBehaviour
{
    public float weight = 1.0f;
    public float resistivity = 1.5f;
    public AnchorPoint startAnchor;
    public AnchorPoint endAnchor;
    public float currentForce;
    public bool isBroken = false;
    public bool isSupported = false;
    public List<Beam> connectedBeams = new List<Beam>();
    
    private bool isScaleFrozen = false;
    private SpriteRenderer spriteRenderer;
    private HashSet<Beam> calculatedBeams = new HashSet<Beam>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (startAnchor != null && endAnchor != null && !isBroken)
        {
            UpdatePositionAndRotation();
            UpdateColorBasedOnTension(currentForce);
        }
    }

    public void CheckSupport(List<Beam> allBeams)
    {
        foreach (Beam otherBeam in allBeams)
        {
            if (otherBeam == this || otherBeam.isBroken) continue;
            
            if (Mathf.Abs(startAnchor.transform.position.y - otherBeam.transform.position.y) < 0.1f &&
                ((startAnchor.transform.position.x >= otherBeam.startAnchor.transform.position.x &&
                  startAnchor.transform.position.x <= otherBeam.endAnchor.transform.position.x) ||
                 (endAnchor.transform.position.x >= otherBeam.startAnchor.transform.position.x &&
                  endAnchor.transform.position.x <= otherBeam.endAnchor.transform.position.x)))
            {
                isSupported = true;
                break;
            }
        }
    }

    public float CalculateRealStress(HashSet<Beam> visited = null)
    {
        if (visited == null) visited = new HashSet<Beam>();
        if (visited.Contains(this)) return 0f; // Evitar ciclos
        
        visited.Add(this);
        float totalStress = weight * 9.81f;
        
        foreach (Beam connectedBeam in connectedBeams)
        {
            if (!connectedBeam.isBroken && !connectedBeam.isSupported)
            {
                totalStress += connectedBeam.CalculateRealStress(visited);
            }
        }
        
        currentForce = totalStress;
        return totalStress;
    }

    public void CheckFailure()
    {
        currentForce = CalculateRealStress();
        if (currentForce > resistivity)
        {
            BreakBeam();
        }
    }

    private void BreakBeam()
    {
        isBroken = true;
        spriteRenderer.color = Color.black;
        Invoke("DestroyBeam", 0.5f);
    }

    private void DestroyBeam()
    {
        if (startAnchor != null )
            Destroy(startAnchor.gameObject);
        
        if (endAnchor != null )
            Destroy(endAnchor.gameObject);
        
        Destroy(gameObject);
    }

    private void UpdatePositionAndRotation()
    {
        Vector2 midpoint = (startAnchor.transform.position + endAnchor.transform.position) / 2;
        transform.position = midpoint;

        Vector2 direction = endAnchor.transform.position - startAnchor.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (!isScaleFrozen)
        {
            float distance = Vector2.Distance(startAnchor.transform.position, endAnchor.transform.position);
            transform.localScale = new Vector3(distance, transform.localScale.y, transform.localScale.z);
        }
    }

    private void UpdateColorBasedOnTension(float tension)
    {
        float tensionRatio = tension / resistivity;
        
        if (tensionRatio < 0.5f)
            spriteRenderer.color = Color.Lerp(Color.green, Color.yellow, tensionRatio * 2);
        else
            spriteRenderer.color = Color.Lerp(Color.yellow, Color.red, (tensionRatio - 0.5f) * 2);
    }

    public BeamData GetBeamData()
    {
        BeamData data = new BeamData();
        if (startAnchor != null && endAnchor != null)
        {
            data.startAnchorPosition = startAnchor.transform.position;
            data.endAnchorPosition = endAnchor.transform.position;
            data.angle = GetAngle();
            data.scaleX = transform.localScale.x;
            data.weight = weight;
            data.resistivity = resistivity;
        }
        return data;
    }

    public float GetAngle()
    {
        Vector2 direction = endAnchor.transform.position - startAnchor.transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public void FreezeScale()
    {
        isScaleFrozen = true;
    }
}