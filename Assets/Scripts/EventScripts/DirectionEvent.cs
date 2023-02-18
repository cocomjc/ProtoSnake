using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/DirectionEvent")]
public class DirectionEvent : ScriptableObject
{
    public UnityAction<Directions> OnEventRaised;

    public void RaiseEvent(Directions value)
    {
        OnEventRaised?.Invoke(value);
    }
}

public enum Directions
{
    Init,
    Horizontal,
    Vertical,
}
