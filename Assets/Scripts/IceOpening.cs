using UnityEngine;
using UnityEngine.EventSystems;

public class IceOpening : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject iceOpen;
    [SerializeField] private GameObject iceOpenShadow;
    [SerializeField] private GameObject iceClosedShadow;
    [SerializeField] private GameObject ice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (iceOpen != null && iceOpenShadow != null && iceClosedShadow != null)
        {
            iceOpen.SetActive(true);
            iceOpenShadow.SetActive(true);
            iceClosedShadow.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
