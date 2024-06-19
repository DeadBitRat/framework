using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform[] transforms; // Array to hold the transforms of the sprites in this layer
        public float scrollSpeed; // Speed of scrolling for this layer
        public float layerWidth; // Width of the sprite for this layer
    }

    public ParallaxLayer[] parallaxLayers; // Array to hold all parallax layers

    void Update()
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            // Scroll each child of the layer
            for (int i = 0; i < layer.transforms.Length; i++)
            {
                Transform t = layer.transforms[i];
                t.position += new Vector3(layer.scrollSpeed * Time.deltaTime, 0, 0);

                // Check if the child has moved completely off-screen to the right
                if (t.position.x >= layer.layerWidth)
                {
                    // Move the child to the left end
                    float offset = t.position.x - layer.layerWidth;
                    t.position = new Vector3(-layer.layerWidth + offset, t.position.y, t.position.z);
                }
            }
        }
    }
}