using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HumanController : MonoBehaviour {

    Player player;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI agentText;
    [SerializeField] TextMeshProUGUI leadersText;
    [SerializeField] TextMeshProUGUI empireText;
    [SerializeField] TextMeshProUGUI systemText;

    SystemUI systemUI;
    void Start () {
        player = GetComponent<Player>();
        systemUI = FindObjectOfType<SystemUI>();
    }
	
	// Update is called once per frame
	void Update () {
        goldText.SetText(player.GetGold().ToString());
        agentText.SetText(player.GetTotalAgents().ToString());
        leadersText.SetText(player.GetTotalLeadersControlled().ToString());
        empireText.SetText(player.GetEmpiresControlled().ToString());
        systemText.SetText(player.GetSystemsControlled().ToString());

    }
}
