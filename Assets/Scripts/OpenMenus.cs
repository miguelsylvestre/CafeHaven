using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class OpenMenus : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject menu;
    public static bool isInCounterMenu = false;
    private static List<GameObject> allMenus = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (menu != null && !allMenus.Contains(menu))
        {
            allMenus.Add(menu);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInCounterMenu && menu != null)
        {
            menu.SetActive(true);
            PlayerMovement.IsInMenu = true;
            isInCounterMenu = true;
        }
    }

    public static void CloseMenus()
    {
        foreach (GameObject singleMenu in allMenus)
        {
            if (singleMenu != null)
                singleMenu.SetActive(false);
        }
        PlayerMovement.IsInMenu = false;
        isInCounterMenu = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
