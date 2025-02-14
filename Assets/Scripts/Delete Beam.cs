using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBeam : MonoBehaviour
{
    void Update() {
            if (Input.GetMouseButtonDown(1)){ //Click derecho
                Debug.Log("Click derecho"); 
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition,Vector2.zero);
                Debug.Log(hit);
                if (hit.collider == null){
                    Debug.Log("Borrar a un paso");
                    if (hit.collider.CompareTag("Beam")){
                        Destroy(hit.collider.gameObject);  
                    }
                }
            }
    }
}
