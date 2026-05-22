using UnityEngine;
using UnityEngine.SceneManagement;

public class CounterSwitch : MonoBehaviour
{
    public string sceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}