using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoveMovement : MonoBehaviour
{
    public float val;
    public List<GameObject> hits;
    private int speed = 5;
    private float time = 0;
    private int changeDirction = 3;
    private Vector3 dir;
    // Start is called before the first frame update
    void Awake()
    {
        hits = new List<GameObject>();
    }

    private void Start()
    {
        dir = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (time == 0) 
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);
            dir = new Vector3(x, y, 0);
        }

        transform.Translate(dir * speed * Time.deltaTime, Space.World);
        time += Time.deltaTime;

        if (time > changeDirction)
        {
            time = 0;
        }
    }

    public void solveCompete()
    {
        List<DoveMovement> tempDove = new List<DoveMovement>();
        if (hits.Count > 1)
        {
            for (int i = 0; i < hits.Count; i++)
            {
                if (hits[i])
                   continue;

                if (hits[i].tag.Equals("Dove"))
                {
                    tempDove.Add(hits[i].GetComponent<DoveMovement>());
                }
            }
            tempDove.Add(this);
            int randomNum = Random.Range(0, tempDove.Count);
            tempDove[randomNum].val += 50;
            foreach (DoveMovement dm in tempDove)
            {
                dm.val -= GameManagement.Instance.energy_lose_bluffing;
            }
        }

        hits.Clear();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Food"))
        {
            Debug.Log("dove" + other.gameObject.tag);
            hits.Add(other.gameObject);
            solveCompete();
        }

    }
}
