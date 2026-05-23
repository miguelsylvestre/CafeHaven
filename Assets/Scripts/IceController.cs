using UnityEngine;
using UnityEngine.EventSystems;

public class IceController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject iceClosed;
    [SerializeField] private GameObject iceOpen;
    [SerializeField] private GameObject iceClosedShadow;
    [SerializeField] private GameObject iceOpenShadow;
    [SerializeField] private GameObject ice;

    private static bool isOpened;

    void Start()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!OpenMenus.isInCounterMenu)
        {
            isOpened = !isOpened;
            if (iceClosed != null) iceClosed.SetActive(!isOpened);
            if (iceOpen != null) iceOpen.SetActive(isOpened);
            if (iceClosedShadow != null) iceClosedShadow.SetActive(!isOpened);
            if (iceOpenShadow != null) iceOpenShadow.SetActive(isOpened);
            if (ice != null) ice.SetActive(isOpened);
        }
    }

}