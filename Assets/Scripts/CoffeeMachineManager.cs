using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CoffeeMachineManager : MonoBehaviour
{
    public bool cupInSlot;
    public bool isPouring;
    public bool ready;
    private float pourDuration;

    [SerializeField] private Button decafButton;
    [SerializeField] private Button regularButton;

    [SerializeField] private Button lowButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button highButton;
    [SerializeField] private Button resetButton;

    [SerializeField] private Button pourButton;
    [SerializeField] public GameObject timerText;

    [SerializeField] private GameObject resetObject;
    [SerializeField] private GameObject claimObject;
    [SerializeField] private GameObject dragObject;
    [SerializeField] private GameObject cupObject;

    [SerializeField] private CupDragging cupDragging;
    [SerializeField] private bool isRightPanel;

    private Coroutine pouringCoroutine;

    private bool? isDecaf = null;
    private int intensity = 0;
    private Drink pendingDrink = null;
    private Sizes size;

    void Start()
    {
        timerText.SetActive(false);
        resetButton.interactable = false;
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

        if (intensity == 1) pourDuration = 12f;
        if (intensity == 2) pourDuration = 16f;
        if (intensity == 3) pourDuration = 20f;
        if (size == Sizes.Tall) pourDuration *= 1.5f;
        timerText.SetActive(true);
        updateTimeUI(pourDuration);

        RefreshPourButton();
    }

    public void OnPour()
    {
        pendingDrink = new Drink
        {
            size = size,
            coffee = new Coffee
            {
                decaf = isDecaf.Value,
                intensity = intensity
            }
        };

        pouringCoroutine = StartCoroutine(Pouring());
        SetAllButtonsInteractable(false);
        resetButton.interactable = true;
    }

    public void OnReset()
    {
        if (isPouring)
        {
            if (pouringCoroutine != null)
                StopCoroutine(pouringCoroutine);

            isPouring = false;
            timerText.SetActive(false);
            SetAllButtonsInteractable(true);

            if (isRightPanel)
                cupDragging.ResetRight();
            else
                cupDragging.ResetLeft();
        }

        ResetPanel();
    }

    public void resetField()
    {
        if (!isPouring && !ready)
        {
            resetObject.SetActive(true);
        }
        else
        {
            resetObject.SetActive(false);
        }
    }

    private IEnumerator Pouring()
    {
        resetObject.SetActive(false);
        isPouring = true;
        ready = false;
        float remaining = pourDuration;

        while (remaining > 0f)
        {
            remaining -= Time.deltaTime;
            updateTimeUI(remaining);
            yield return null;
        }

        timerText.SetActive(false);
        isPouring = false;
        ready = true;
        claimObject.SetActive(true);
        resetButton.interactable = false;
    }

    private void updateTimeUI(float remaining)
    {
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        timerText.GetComponent<TextMeshProUGUI>().text = $"{minutes:00}:{seconds:00}";
    }

    public void OnClaim()
    {
        if (pendingDrink == null) return;

        CupContents contents = cupObject.GetComponent<CupContents>();
        if (contents == null) return;

        if (contents.filled)
        {
            Debug.Log("finish the other freaking cup bro.");
            return;
        }

        contents.SetDrink(pendingDrink);
        contents.updateCup();
        cupObject.SetActive(true);

        claimObject.SetActive(false);
        resetObject.SetActive(false);
        ready = false;
        ResetPanel();
    }

    public void RefreshPourButton()
    {
        CupDropSlot occupied = dragObject.GetComponent<CupDropSlot>();
        pourButton.interactable = isDecaf.HasValue && intensity != 0 && occupied.occupied;
        resetButton.interactable = isPouring || isDecaf.HasValue || intensity != 0;
    }

    public void UpdateSize(Sizes s)
    {
        size = s;
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

        timerText.SetActive(false);
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

    static void SetButtonInteractable(Button btn, bool state){btn.interactable = state;}
}