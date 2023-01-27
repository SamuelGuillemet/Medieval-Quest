using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour
{
    [SerializeField]
    private Renderer _textureRender;

    public Renderer TextureRender { get => _textureRender; }

    public void DrawTexture(Texture2D texture)
    {
        _textureRender.sharedMaterial.mainTexture = texture;
        // The default size of the plane is 10x10, so we need to scale it to fit the texture, every unit of the texture 4 units in unity
        Vector3 scale = new Vector3(texture.width / 10f * 4f, 1, texture.height / 10f * 4f);
        _textureRender.transform.localScale = scale;
        _textureRender.transform.localPosition = new Vector3(texture.width * 2f, 0, texture.height * 2f);
        _textureRender.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}
