using UnityEngine;

public class ScrollTexture : MonoBehaviour
{

    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.1f;
    private Renderer rend;
    private Vector2 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<Renderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;
        rend.material.SetTextureOffset("_BaseMap", offset);
    }
}
