using UnityEngine;
using UnityEngine.EventSystems;

public class FridgeController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject fridgeClosed;
    [SerializeField] private GameObject fridgeOpened;

    private static bool isOpened = false;

    void Start()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!OpenMenus.isInCounterMenu)
        {
            isOpened = !isOpened;
            if (fridgeClosed != null) fridgeClosed.SetActive(!isOpened);
            if (fridgeOpened != null) fridgeOpened.SetActive(isOpened);
           
        }
    }

}