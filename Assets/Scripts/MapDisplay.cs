using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture)
    {

        textureRender.sharedMaterial.SetTexture("_BaseMap", texture);
        textureRender.sharedMaterial.mainTexture = texture;

        textureRender.sharedMaterial.color = Color.white;
        textureRender.sharedMaterial.SetColor("_BaseColor", Color.white);

        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}