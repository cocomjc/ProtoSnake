using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/BoolEvent")]
public class BoolEvent : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool value)
    {
        OnEventRaised?.Invoke(value);
    }
}
