using UnityEngine;

public class MirrorPos : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private Vector2 offset;

    private void Update()
    {
        if (target == null) return;

        GetComponent<RectTransform>().anchoredPosition = target.anchoredPosition + offset;
    }
}