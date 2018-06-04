using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] string playerName;
    [SerializeField] int playerNumber;
    [SerializeField] Color playerColor;

    public int GetPlayerNumber() { return playerNumber;  }
    public string GetPlayerName() { return playerName; }
    public Color GetPlayerColor() { return playerColor; }
}
