using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAnchorPoint : MonoBehaviour
{
    public GameObject anchorPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GenerarAnchor();
            Debug.Log("Aparecer anchorPoint");
        }   
    }
    public void GenerarAnchor(){
        Vector3 mousePosition= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z=0;
        Debug.Log(mousePosition);
        Instantiate (anchorPoint, mousePosition,(Quaternion.identity));
    }
}

