using UnityEngine;

public class WaterFollow : MonoBehaviour
{
    [Header("Налаштування")]
    public Transform viewer;
    public float waterHeight = 23f;

    [Tooltip("Розмір кроку телепорту. Має бути достатньо великим.")]
    public float gridSize = 10f;

    void LateUpdate()
    {
        if (viewer != null)
        {
            // Магія тут: ми округлюємо позицію гравця до найближчого кроку (наприклад, кратно 10)
            float targetX = Mathf.Round(viewer.position.x / gridSize) * gridSize;
            float targetZ = Mathf.Round(viewer.position.z / gridSize) * gridSize;

            // Вода рухається тільки "рівними" кроками, тому шейдер не смикається
            transform.position = new Vector3(targetX, waterHeight, targetZ);
        }
    }
}