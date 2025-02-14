using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    public bool isFixed = true; // Si es true, no se puede mover

    void OnMouseDown()
    {
        Debug.Log("Click en anchor Point");
        if (!isFixed)
        {
            ConstructionManager.Instance.StartCreatingBeam(this);
        }else{
            ConstructionManager.Instance.StartCreatingBeam(this);
        }
    }
}