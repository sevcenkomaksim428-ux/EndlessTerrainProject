using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        texture.SetPixels(colourMap);
        texture.Apply();

        textureRender.sharedMaterial.SetTexture("_BaseMap", texture);
        textureRender.sharedMaterial.mainTexture = texture;

        textureRender.sharedMaterial.color = Color.white;
        textureRender.sharedMaterial.SetColor("_BaseColor", Color.white);

        textureRender.transform.localScale = new Vector3(width, 1, height);
    }
}