using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PerspectiveSort : MonoBehaviour
{
    public Transform sortPoint;
    Transform trans;
    SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        float y;
        float x = transform.position.x;

        if (sortPoint != null)
            y = sortPoint.position.y;
        else
            y = rend.bounds.min.y;

        rend.sortingOrder = Mathf.RoundToInt(-y);
    }
}
