using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkMovement : MonoBehaviour
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
        List<HawkMovement> tempHawk = new List<HawkMovement>();
        if (hits.Count > 1) 
        {
            for (int i = 0; i < hits.Count; i++)
            {
                //Debug.Log(hits[i]);
                if (hits[i])
                    continue;

                if (hits[i].tag.Equals("Hawk"))
                {
                    tempHawk.Add(hits[i].GetComponent<HawkMovement>());
                }
                else if (hits[i].tag.Equals("Dove"))
                {
                    val += 50;
                }
            }
            tempHawk.Add(this);
            int randomNum = Random.Range(0, tempHawk.Count);
            tempHawk[randomNum].val += 50;
            tempHawk.RemoveAt(randomNum);
            foreach (HawkMovement hm in tempHawk)
            {
                hm.val -= GameManagement.Instance.energy_lose_injury;
            }
        }

        hits.Clear();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Food") )
        {
            hits.Add(other.gameObject);
            solveCompete();
        }
    }
}
