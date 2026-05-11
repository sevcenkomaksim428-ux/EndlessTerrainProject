using UnityEngine;

public class AtmosphereController : MonoBehaviour
{
    [Header("Сонце")]
    public Light sunLight;
    public float daySpeed = 2.0f;

    [Header("Небо")]
    public float skySpeed = 0.5f;

    void Update()
    {
        if (sunLight != null)
        {
            sunLight.transform.Rotate(Vector3.right * Time.deltaTime * daySpeed);
        }

        float currentRotation = RenderSettings.skybox.GetFloat("_Rotation");
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation + skySpeed * Time.deltaTime);
    }
}