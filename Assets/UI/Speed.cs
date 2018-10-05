using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = ("UI/Speed"))]

public class Speed : ScriptableObject
{
    [SerializeField] Sprite icon;
    [SerializeField] string name;
    [SerializeField] float speed;

    public float GetSpeed()
    {
        return speed;
    }

    public string GetName()
    {
        return name;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

}
