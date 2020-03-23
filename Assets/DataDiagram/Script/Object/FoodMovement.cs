using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovement : MonoBehaviour
{
    public float val;
    public int expiredTime;
    public List<GameObject> hits;

    // Start is called before the first frame update
    void Awake()
    {
        hits = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public bool solveCompete(int foodvalue)
    {
        List<HawkMovement> tempHawk= new List<HawkMovement>();
        bool hasHawk = false;
        if (hits.Count > 1)
        {
            for (int i = 0; i < hits.Count; i++)
            {
/*                if (hits[i])
                    continue;*/

                if (hits[i].tag.Equals("Hawk"))
                {
                    tempHawk.Add(hits[i].GetComponent<HawkMovement>());
                    hasHawk = true;
                }
            }

            if (hasHawk)
            {
                int randomNum = Random.Range(0, tempHawk.Count);
                tempHawk[randomNum].val += foodvalue;
            }
            else
            {
                int randomNum = Random.Range(0, hits.Count);
                hits[randomNum].GetComponent<DoveMovement>().val += foodvalue;
            }
            hits.Clear();
            return true;
        }

        hits.Clear();
        return false;
    }

    public bool checkExpired()
    {
        if (val > expiredTime)
        {
            return true;
        }
        return false;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("food hit");
        if (!other.gameObject.tag.Equals("Food"))
        {
            hits.Add(other.gameObject);
        }
    }
}
