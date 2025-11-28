using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable element;

    private void Reset()
    {
        eventSystem = FindFirstObjectByType<EventSystem>();
    }

    public void JumpTo()
    {
        if (eventSystem != null && element != null)
        {
            eventSystem.SetSelectedGameObject(element.gameObject);
        }
    }
}