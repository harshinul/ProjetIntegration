using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable element;

    // Reset is called when the script is added or reset in the Inspector
    private void Reset()
    {
        // Use FindFirstObjectByType instead of FindObjectOfType
        eventSystem = FindFirstObjectByType<EventSystem>();
    }

    // Update is called once per frame
    public void JumpTo()
    {
        if (eventSystem != null && element != null)
        {
            eventSystem.SetSelectedGameObject(element.gameObject);
        }
    }
}