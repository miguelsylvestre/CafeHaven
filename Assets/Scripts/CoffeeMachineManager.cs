using UnityEngine;
using UnityEngine.UI;

public class CoffeeMachineManager : MonoBehaviour
{
    [SerializeField] private Button decafButton;
    [SerializeField] private Button regularButton;

    [SerializeField] private Button lowButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button highButton;

    [SerializeField] private Button pourButton;

    [SerializeField] private GameObject resetObject;
    [SerializeField] private GameObject claimObject;
    [SerializeField] private GameObject dragObject;
    [SerializeField] private GameObject cupObject;


    private bool? isDecaf = null;
    private int intensity = 0;
    private Drink pendingDrink = null;


    public void SelectDecaf(bool decaf)
    {
        isDecaf = decaf;

        SetButtonInteractable(decafButton, !decaf);
        SetButtonInteractable(regularButton, decaf);

        RefreshPourButton();
    }

    public void SelectIntensity(int level)
    {
        intensity = level;

        SetButtonInteractable(lowButton, level != 1);
        SetButtonInteractable(mediumButton, level != 2);
        SetButtonInteractable(highButton, level != 3);


        RefreshPourButton();
    }

    public void OnPour()
    {
        pendingDrink = new Drink
        {
            coffee = new Coffee
            {
                decaf = isDecaf.Value,
                intensity = intensity
            }
        };

        resetObject.SetActive(false);
        claimObject.SetActive(true);

        SetAllButtonsInteractable(false);
    }

    public void OnClaim()
    {
        if (pendingDrink == null) return;

        cupObject.SetActive(true);

        CupContents contents = cupObject.GetComponent<CupContents>();
        if (contents != null)
            contents.SetDrink(pendingDrink);

        claimObject.SetActive(false);
        resetObject.SetActive(false);
        ResetPanel();
    }


    public void RefreshPourButton()
    {
        CupDropSlot occupied = dragObject.GetComponent<CupDropSlot>();
        pourButton.interactable = (isDecaf.HasValue && intensity != 0 && occupied.occupied);
    }

    public void ResetPanel()
    {
        isDecaf = null;
        intensity = 0;
        pendingDrink = null;

        SetButtonInteractable(decafButton, true);
        SetButtonInteractable(regularButton, true);
        SetButtonInteractable(lowButton, true);
        SetButtonInteractable(mediumButton, true);
        SetButtonInteractable(highButton, true);

        RefreshPourButton();

        claimObject.SetActive(false);
        resetObject.SetActive(true);
    }

    void SetAllButtonsInteractable(bool state)
    {
        decafButton.interactable = state;
        regularButton.interactable = state;
        lowButton.interactable = state;
        mediumButton.interactable = state;
        highButton.interactable = state;
        pourButton.interactable = state;
    }

    static void SetButtonInteractable(Button btn, bool state) => btn.interactable = state;
}