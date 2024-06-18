using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Transform of the camera to shake
    public Transform camTransform;

    // How long the object should shake for
    public float shakeDuration = 0f;

    // Amplitude of the shake. A smaller value shakes the camera more subtly
    public float shakeAmount = 0.2f; // Reduced for a more subtle effect
    public float decreaseFactor = 1.5f; // Increase the decrease factor for quicker calm down

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent<Transform>();
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}
