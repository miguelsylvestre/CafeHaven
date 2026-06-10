using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CounterSwitch : MonoBehaviour, IInteractable
{
    public string sceneName;
    private bool playerIn;
    private bool hasLoadedOnce = false;
    [SerializeField] private GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger")) {
            playerIn = true;
            player.GetComponent<Animator>().SetBool("inLeftC", true);
            Debug.Log("player in");
        }  
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Trigger")) {
            playerIn = false;
            player.GetComponent<Animator>().SetBool("inLeftC", false);
            Debug.Log("player out");
        }
    }
    
    public void Interact() 
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIn) {
            if (!hasLoadedOnce){
                PlayerMovement.IsInMenu = true;
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                hasLoadedOnce = true;
            }
            else {
                if (CounterViewManager.Instance != null){
                    CounterViewManager.Instance.ToggleCounterView(true);
                }
            }
        Debug.Log("clicked");
        }
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