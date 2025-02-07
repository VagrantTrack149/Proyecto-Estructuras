using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    private bool isDragging = false;

    void OnMouseDown()
    {
        isDragging = false;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }
}