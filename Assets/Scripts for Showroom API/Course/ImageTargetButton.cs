using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Course
{
    public class ImageTargetButton : MonoBehaviour
    {
        public TextMeshProUGUI id;
        public TextMeshProUGUI AutorName;
        public TextMeshProUGUI Description;
        public MeshRenderer PictureLink;

        // Attach this method to the button click event in the Unity Editor
        public void OnButtonClick()
        {
            // Find the image in the canvas by name and turn it off
            TurnOffImageTargetByName("Canvas", "purchesesnr");
        }

        private void TurnOffImageTargetByName(string canvasName, string imageName)
        {
            // Find the canvas by name
            Canvas canvas = GameObject.Find(canvasName)?.GetComponent<Canvas>();

            if (canvas != null)
            {
                // Find the Image component within the canvas
                Image image = canvas.transform.Find(imageName)?.GetComponent<Image>();

                // If the Image component is found, turn it off
                if (image != null)
                {
                    image.enabled = false;

                    // Find the Text component within the same GameObject as the image
                    TextMeshProUGUI textComponent = image.GetComponentInChildren<TextMeshProUGUI>();

                    // If the Text component is found, turn it off
                    if (textComponent != null)
                    {
                        textComponent.enabled = false;
                    }
                    else
                    {
                        Debug.LogError("Text component not found in the same GameObject as the image!");
                    }
                }
                else
                {
                    Debug.LogError("Image with name " + imageName + " not found in the canvas " + canvasName + "!");
                }
            }
            else
            {
                Debug.LogError("Canvas with name " + canvasName + " not found in the hierarchy!");
            }
        }
    }
}
