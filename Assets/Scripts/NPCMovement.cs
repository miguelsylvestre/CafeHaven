using UnityEngine;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour
{
    public float speed = 15f;
    public Queue<Vector2> paths = new Queue<Vector2>();
    private Vector2 currentPath;
    private Vector3 lastPos;
    public SpriteRenderer npc;
    public bool hasPath = false;
    public Animator animator;
    public bool destroy = false;
    public bool canSit;
    private Vector2 table1 = new Vector2(53f, 31f);
    private Vector2 table2 = new Vector2(63f, 0f);
    public bool sitting;
    public float sittingLength;
    public float sittingTimer;
    public bool delivered;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        npc = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPath)
        {
            if (paths.Count > 0)
            {
                currentPath = paths.Dequeue();
                hasPath = true;
            }
            else
                if (destroy)
                    Destroy(gameObject);
            return;
        }
        lastPos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, currentPath, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPath) < 0.1f)
            hasPath = false;

        animator.SetFloat("MoveX", (transform.position - lastPos).x);
        animator.SetBool("Moving", hasPath);

        if (canSit){
            if (Vector2.Distance(transform.position, table1) < 0.1f || Vector2.Distance(transform.position, table2) < 0.1f){
                animator.SetBool("Sitting", true);
                sitting = true;
            }
            else animator.SetBool("Sitting", false);
        }
    }

    public void setPath(List<Vector2> points)
    {
        paths.Clear();
        foreach (var v in points)
            paths.Enqueue(v);

        hasPath = false;
    }

    public void addPoint(Vector2 point)
    {
        paths.Enqueue(point);

    }

    public void findTable(int num)
    {
        List<Vector2> path = new List<Vector2>();
        if (num == 0)
        {
            path.Add(new Vector2(23f, 26f));
            path.Add(table1);
        }
        else
        {
            path.Add(table2);
        }
        setPath(path);
    }
}