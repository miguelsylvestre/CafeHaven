using UnityEngine;
using UnityEngine.InputSystem;

public class CounterViewManager : MonoBehaviour
{
    public static CounterViewManager Instance;

    [SerializeField] private GameObject counterGraphicsParent;
    [SerializeField] private GameObject menusParent;

    private bool isVisible = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (isVisible)
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                if (!OpenMenus.isInCounterMenu)
                    ToggleCounterView(false);
                else
                    OpenMenus.CloseMenus();
            }
        }
    }

    public void ToggleCounterView(bool show)
    {
        isVisible = show;

        if (counterGraphicsParent != null)
        {

            Renderer[] renderers = counterGraphicsParent.GetComponentsInChildren<Renderer>(true);
            Collider2D[] colliders = counterGraphicsParent.GetComponentsInChildren<Collider2D>(true);

            foreach (Renderer r in renderers) r.enabled = show;
            foreach (Collider2D c in colliders) c.enabled = show;
        }
        if (menusParent != null)
        {

            Renderer[] renderers = menusParent.GetComponentsInChildren<Renderer>(true);
            Collider2D[] colliders = menusParent.GetComponentsInChildren<Collider2D>(true);

            foreach (Renderer r in renderers) r.enabled = show;
            foreach (Collider2D c in colliders) c.enabled = show;
        }

        PlayerMovement.IsInMenu = show;
    }
}