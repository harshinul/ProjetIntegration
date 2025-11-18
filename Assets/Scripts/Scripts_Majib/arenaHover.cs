using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArenaHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Fond à activer au hover ou à la sélection")]
    public GameObject back;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (back != null)
            back.SetActive(true);
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (back != null)
            back.SetActive(false);
        
    }

    //Appelé quand le bouton est "sélectionné" via manette / clavier
    public void OnSelect(BaseEventData eventData)
    {
        if (back != null)
            back.SetActive(true);
       
    }

    //Appelé quand on navigue vers un autre bouton
    public void OnDeselect(BaseEventData eventData)
    {
        if (back != null)
            back.SetActive(false);
        
    }
}