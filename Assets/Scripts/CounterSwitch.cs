using UnityEngine;
using UnityEngine.SceneManagement;

public class CounterSwitch : MonoBehaviour, IInteractable
{
    public string sceneName;
    private bool hasLoadedOnce = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger")){
            if (!hasLoadedOnce){
                PlayerMovement.IsInMenu = true;
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                hasLoadedOnce = true;
            }
            else{
                if (CounterViewManager.Instance != null){
                    CounterViewManager.Instance.ToggleCounterView(true);
                }
            }
        }
    }
    
    public void Interact() 
    {

    }

    public void Close()
    {
        
    }

    public KeyCode GetInteractKey()
    {
        return KeyCode.E;
    }

    public bool CanInteract()
    {
        return true;
    }
}