using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAnchorPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit= Physics2D.Raycast(mousePosition,Vector2.zero);
            if (hit.collider != null){
                if (hit.collider.CompareTag("AnchorPoint")){
                    Destroy(hit.collider.gameObject);
                } 
            }
        }
    }
}
