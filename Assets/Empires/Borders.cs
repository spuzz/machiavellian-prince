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
                color[(y * texture.height) + x]  = new Color(0, 0, 0, 0);
                float localX  = (x / 512.0f - 0.5f) * 10.0f;
                float localY  = (y / 512.0f - 0.5f) * 10.0f;
                var worldPos = transform.TransformPoint(new Vector3(localX, 0.0f, localY));
                float influence = 0;
                foreach(Planet planet in planets)
                {
                    float distance = Vector3.Distance(planet.transform.position, worldPos);
                    if(distance < 10.0f)
                    {
                        influence += 1.0f / distance;
                    }
                }
                if (influence != 0)
                {
                    if(influence <= 0.11)
                    {
                        color[(y * texture.height) + x] = new Color(0, 0, 0.1f, 1.0f);
                    }
                    else
                    {
                        color[(y * texture.height) + x] = new Color(0, 0, 0.1f, 0.5f);
                    }
                    
                }
            }
        }
        texture.SetPixels(color);
        texture.Apply();
        renderer.material.mainTexture = texture;
        
    }
}
