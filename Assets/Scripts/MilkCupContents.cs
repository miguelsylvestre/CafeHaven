using UnityEngine;

public class MilkCupContents : MonoBehaviour
{
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite filled;
    private SpriteRenderer cup;


    public Milk milk;

    private void Start()
    {
        cup = GetComponent<SpriteRenderer>();
    }

    public void UpdateVisual()
    {
        if (cup == null) return;
        
        cup.sprite = (milk != null && milk.amount > 0) 
            ? filled 
            : empty;
    }
}