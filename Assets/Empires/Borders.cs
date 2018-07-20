using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Borders : MonoBehaviour {

    Texture2D texture;
    Renderer renderer;
    Planet[] planets;
    void Start () {
        renderer = GetComponent<Renderer>();
        texture = renderer.material.GetTexture("_MainTex") as Texture2D;
        planets = (Planet[])GameObject.FindObjectsOfType(typeof(Planet));
        UpdatePixels();
    }
	
	void UpdatePixels () {
        Color[] color = new Color[texture.width * texture.height];
        for (int x=0;x < texture.width; x++)
        {
            for(int y=0;y< texture.height; y++)
            {
                color[(x * texture.height) + y] = Color.black;
                float localX  = (x / 512.0f - 0.5f) * 10.0f;
                float localY  = (y / 512.0f - 0.5f) * 10.0f;
                var worldPos = transform.TransformPoint(new Vector3(localX, 0.0f, localY));
                foreach(Planet planet in planets)
                {
                    if(Vector3.Distance(planet.transform.position,worldPos) < 5)
                    {
                        color[(x * texture.height) + y] = Color.blue;
                    }
                }

            }
        }
        texture.SetPixels(color);
        texture.Apply();
        renderer.material.mainTexture = texture;
        
    }
}
