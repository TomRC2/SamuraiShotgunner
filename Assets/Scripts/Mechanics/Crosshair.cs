using Unity.VisualScripting;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public RectTransform crosshair;

    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        crosshair.position = mousePosition;
    }
}