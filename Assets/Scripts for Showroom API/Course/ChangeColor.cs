using UnityEngine;

namespace Assets.Course
{
    public class ChangeColor : MonoBehaviour
    {
        public string colorHex = "#FFFFFF"; // Default color is white

        void Start()
        {
            // Call the method to change the color
            ChangeObjectColor();
        }

        void ChangeObjectColor()
        {
            // Assuming the object has a Renderer component with a material
            Renderer renderer = GetComponent<Renderer>();

            if (renderer != null)
            {
                Color color;
                if (ColorUtility.TryParseHtmlString(colorHex, out color))
                {
                    renderer.material.color = color;
                }
                else
                {
                    Debug.LogWarning("Invalid color format. Please use #RRGGBB format (e.g., #FF0000 for red).");
                }
            }
            else
            {
                Debug.LogWarning("Renderer is null. Make sure it's assigned to the object.");
            }
        }
    }
}
