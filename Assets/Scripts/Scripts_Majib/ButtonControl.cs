using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
    
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable element;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Reset()
    {
        eventSystem = FindObjectOfType<EventSystem>();
       
    }

    // Update is called once per frame
    void JumpTo()
    {
        eventSystem.SetSelectedGameObject(element.gameObject);
    }
}
