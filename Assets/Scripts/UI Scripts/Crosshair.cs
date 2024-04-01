using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage;
    public Color defaultColor = Color.white;
    public Color targetColor = Color.red;
    public LayerMask targetLayer;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, targetLayer))
        {
            // Change color when the crosshair is over a target
            crosshairImage.color = targetColor;
        }
        else
        {
            // Revert to the default color
            crosshairImage.color = defaultColor;
        }
    }
}
