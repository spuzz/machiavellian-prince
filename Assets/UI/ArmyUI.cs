using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyUI : MonoBehaviour {

    List<Army> armies = new List<Army>();
    [SerializeField] GameObject armyPanel;
    [SerializeField] GameObject scrollViewContent;
    public void SetArmies(List<Army> armies)
    {
        if(armies.Count != this.armies.Count)
        {
            ClearArmies();

            foreach (Army army in armies)
            {
                this.armies.Add(army);
                GameObject armyPanelComp = Instantiate(armyPanel, scrollViewContent.transform);

                armyPanelComp.GetComponent<ArmyPanel>().SetArmy(army);
            }
        }

    }

    public void AddArmy(Army army)
    {
        armies.Add(army);
        GameObject armyPanelComp = Instantiate(armyPanel, scrollViewContent.transform);

        armyPanelComp.GetComponent<ArmyPanel>().SetArmy(army);
       
    }

    public void RemoveArmy(Army army)
    {
        if(armies.Contains(army))
        {
            armies.Remove(army);
        }
    }

    public void ClearArmies()
    {
        armies.Clear();
        foreach (Transform child in scrollViewContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
