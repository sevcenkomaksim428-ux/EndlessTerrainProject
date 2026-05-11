using UnityEngine;
using System.Linq;

[CreateAssetMenu()]
public class TextureData : UpdatableData
{

    const int textureSize = 2048;
    const TextureFormat textureFormat = TextureFormat.RGBA32;

    public Layer[] layers;

    float savedMinHeight;
    float savedMaxHeight;
    public void ApplyToMaterial(Material material)
    {
        UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);

        if (layers == null || layers.Length == 0) return;

        material.SetFloat("layerCount", layers.Length);
        material.SetColorArray("baseColours", layers.Select(layer => layer.tint).ToArray());
        material.SetFloatArray("baseStartHeights", layers.Select(layer => layer.startHeight).ToArray());
        material.SetFloatArray("baseBlends", layers.Select(layer => layer.blendStrength).ToArray());
        material.SetFloatArray("baseColourStrength", layers.Select(layer => layer.tintStrength).ToArray());
        material.SetFloatArray("baseTextureScales", layers.Select(layer => layer.textureScale).ToArray());
        Texture2DArray texturesArray = GenarateTextereArray(layers.Select(layer => layer.texture).ToArray());
        material.SetTexture("baseTextures", texturesArray);
    }

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
    {
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
    }

    Texture2DArray GenarateTextereArray(Texture[] textures)
    {
        Texture2DArray textureArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);

        for (int i = 0; i < textures.Length; i++)
        {
            textureArray.SetPixels(((Texture2D)textures[i]).GetPixels(), i);
        }
        textureArray.Apply();
        return textureArray;
    }

    [System.Serializable]
    public class Layer
    {
        public Texture texture;
        public Color tint;
        [Range(0,1)]
        public float tintStrength;
        [Range(0,1)]
        public float startHeight;
        [Range(0,1)]
        public float blendStrength;
        public float textureScale;
    }
}
