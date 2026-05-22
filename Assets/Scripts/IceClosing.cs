using UnityEngine;
using UnityEngine.EventSystems;

public class IceClosing : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject iceClosed;
    [SerializeField] private GameObject iceOpenShadow;
    [SerializeField] private GameObject iceClosedShadow;
    [SerializeField] private GameObject ice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (iceClosed != null && iceOpenShadow != null && iceClosedShadow != null && ice != null)
        {
            iceClosed.SetActive(true);
            iceOpenShadow.SetActive(false);
            iceClosedShadow.SetActive(true);
            gameObject.SetActive(false);
            ice.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
