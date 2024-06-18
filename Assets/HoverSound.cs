using UnityEngine;
using UnityEngine.EventSystems; // Required for Event triggers

public class HoverSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public AudioSource audioSource; // Assign this in the inspector
    public AudioClip hoverSound;    // Assign the hover sound clip in the inspector
    public AudioClip clickSound;    // Assign the click sound clip in the inspector
    public Vector3 normalSize = new Vector3(1, 1, 1);        // Normal size
    public Vector3 hoveredSize = new Vector3(1.1f, 1.1f, 1.1f); // Slightly larger size

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play hover sound when the mouse enters the button area
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
        transform.localScale = hoveredSize; // Make button larger
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play click sound when the button is clicked
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = normalSize; // Reset button size when the mouse exits
    }
}
