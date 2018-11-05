using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    HUD hud;

    private void Start()
    {
        hud = FindObjectOfType<HUD>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            hud.SetAgentTarget(transform.parent.parent.parent.gameObject);
    }
}
