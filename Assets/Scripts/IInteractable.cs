using UnityEngine;

public interface IInteractable
{
    void Interact();
    bool CanInteract();
    void Close();
    //void Glow();
    //void Regular();
    KeyCode GetInteractKey();
}