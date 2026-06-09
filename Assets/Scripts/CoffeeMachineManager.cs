using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CoffeeMachineManager : MonoBehaviour
{
    public bool isPouring;
    public bool ready;

    [SerializeField] private Button decafButton;
    [SerializeField] private Button regularButton;

    [SerializeField] private Button lowButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button highButton;

    [SerializeField] private Button pourButton;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject resetObject;
    [SerializeField] private GameObject claimObject;
    [SerializeField] private GameObject dragObject;
    [SerializeField] private GameObject cupObject;


    private bool? isDecaf = null;
    private int intensity = 0;
    private Drink pendingDrink = null;

    void Start()
    {
        // timerText.SetActive(false);
    }


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

        StartCoroutine(Pouring());

        SetAllButtonsInteractable(false);
    }
    
    public void resetField()
    {
        if (!isPouring && !ready) {
            resetObject.SetActive(true);
        } else resetObject.SetActive(false);
    }

    private IEnumerator Pouring()
    {
        float remaining = 12f;
        resetObject.SetActive(false);
        // timerText.setActive(true);
        isPouring = true;
        ready = false;

        while (remaining > 0f)
        {
            remaining -= Time.deltaTime;
            updateTimeUI(remaining);
            yield return null;
        }
        // timerText.setActive(false);
        isPouring = false;
        ready = true;
        claimObject.SetActive(true);
    }

    private void updateTimeUI(float remaining)
    {
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
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
        ready = false;
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
        resetObject.SetActive(false);
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