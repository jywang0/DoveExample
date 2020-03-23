using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    //collection
    public GameObject dove_obj;
    public GameObject hawk_obj;
    public GameObject food_obj;

    public List<FoodMovement> foods;
    public List<DoveMovement> doves;
    public List<HawkMovement> hawks;
    //setting
    private int hawk;
    private int dove;
    private int food;

    private int population;

    public int food_value;
    public int energy_lose_injury;
    public int energy_lose_bluffing;
    public int energy_base_req;
    public int death_threshold;
    public int repoduction_threshold;
    private int food_expiration_time;
    private bool next_step = false;
    private int next_step_time = 0;
    private float step = 0;
    //diagram
    public DD_DataDiagram m_DataDiagram;
    private bool m_IsContinueInput = false;
    private float m_Input = 0f;
    private float h = 0;
    List<GameObject> lineList = new List<GameObject>();
    public CSVCreator csv;

    //random generate
    public float Min;
    public float Max;

    //singleton
    private static GameManagement instance;
    public static GameManagement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("GameManagement").GetComponent<GameManagement>();
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AddALine();
        Time.timeScale = 0;
        foods = new List<FoodMovement>();
        doves = new List<DoveMovement>();
        hawks = new List<HawkMovement>();
        Min = -28;
        Max = 28;
        InitValue();
        RandomGenerator();
    }

    private void InitValue()
    {
        hawk = int.Parse(GameObject.Find("Hawk_Input").GetComponent<TMPro.TMP_InputField>().text);
        dove = int.Parse(GameObject.Find("Dove_Input").GetComponent<TMPro.TMP_InputField>().text);
        food = int.Parse(GameObject.Find("Food_Input").GetComponent<TMPro.TMP_InputField>().text);
        //population = int.Parse(GameObject.Find("Population_Input").GetComponent<TMPro.TMP_InputField>().text);
        food_value = int.Parse(GameObject.Find("Energy Food_Input").GetComponent<TMPro.TMP_InputField>().text);
        energy_lose_injury = int.Parse(GameObject.Find("Energy Injury_Input").GetComponent<TMPro.TMP_InputField>().text);
        energy_lose_bluffing = int.Parse(GameObject.Find("Energy Bluffing_Input").GetComponent<TMPro.TMP_InputField>().text);
        energy_base_req = int.Parse(GameObject.Find("Energy Require_Input").GetComponent<TMPro.TMP_InputField>().text);
        death_threshold = int.Parse(GameObject.Find("Death Threshold_Input").GetComponent<TMPro.TMP_InputField>().text);
        repoduction_threshold = int.Parse(GameObject.Find("Reproduction Threshold_Input").GetComponent<TMPro.TMP_InputField>().text);
        food_expiration_time = int.Parse(GameObject.Find("Food Expiration_Input").GetComponent<TMPro.TMP_InputField>().text);
    }

    private void RandomGenerator()
    {
        for (int i = 0; i < hawk; i++)
        {
            float xAxis = UnityEngine.Random.Range(Min, Max);
            float yAxis = UnityEngine.Random.Range(Min, Max);
            Vector3 randomPosition = new Vector3(xAxis, yAxis, 0);
            GameObject obj = Instantiate(hawk_obj, randomPosition, Quaternion.identity);
            hawks.Add(obj.GetComponent<HawkMovement>());
            obj.GetComponent<HawkMovement>().val = 100;
        }

        for (int i = 0; i < dove; i++)
        {
            float xAxis = UnityEngine.Random.Range(Min, Max);
            float yAxis = UnityEngine.Random.Range(Min, Max);
            Vector3 randomPosition = new Vector3(xAxis, yAxis, 0);
            GameObject obj = Instantiate(dove_obj, randomPosition, Quaternion.identity);
            doves.Add(obj.GetComponent<DoveMovement>());
            obj.GetComponent<DoveMovement>().val = 100;
        }

        for (int i = 0; i < food; i++)
        {
            float xAxis = UnityEngine.Random.Range(Min, Max);
            float yAxis = UnityEngine.Random.Range(Min, Max);
            Vector3 randomPosition = new Vector3(xAxis, yAxis, 0);
            GameObject obj = Instantiate(food_obj, randomPosition, Quaternion.identity);
            obj.GetComponent<FoodMovement>().expiredTime = food_expiration_time;
            foods.Add(obj.GetComponent<FoodMovement>());
            obj.GetComponent<FoodMovement>().val = 0;
        }

    }
    void AddALine()
    {

        Color hawkc = Color.red;
        GameObject hawk = m_DataDiagram.AddLine(hawkc.ToString(), hawkc);

        lineList.Add(hawk);
        Color foodc = Color.blue;
        GameObject food = m_DataDiagram.AddLine(foodc.ToString(), foodc);

        lineList.Add(food);

        Color dovec = Color.green;
        GameObject dove = m_DataDiagram.AddLine(dovec.ToString(), dovec);

        lineList.Add(dove);
        Debug.Log(lineList.Count);
    }

    private void runDiagram(float f)
    {
        
        if (null == m_DataDiagram)
            return;

        float d = 0f;
        Debug.Log("datad"+lineList.Count);
        for(int i =0;i< lineList.Count;i++)
        {
            if( i == 0)
                m_DataDiagram.InputPoint(lineList[i], new Vector2(0.1f, hawks.Count / 40f));
            if (i == 1)
                m_DataDiagram.InputPoint(lineList[i], new Vector2(0.1f, foods.Count / 20f));
            if (i == 2)
                m_DataDiagram.InputPoint(lineList[i], new Vector2(0.1f, doves.Count / 40f));

        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("exp" + step);
        m_Input += Time.deltaTime;
        runDiagram(m_Input);
        if (step >= 100)
        {
            csv.Finished();
            Time.timeScale = 0;
        }
            
        csv.Save(doves.Count, hawks.Count, foods.Count);
        //Debug.Log(foods.Count);
        //Debug.Log(doves.Count);
        Debug.Log(hawks.Count);
        List<FoodMovement> list = new List<FoodMovement>();
        //Remove expired food
        for (int i = 0; i < foods.Count; i++)
        {
            bool temp = foods[i].checkExpired();
            if (temp == true)
            {
                list.Add(foods[i]);
            }
            else
            {
                foods[i].val += Time.deltaTime;
            }
        }

        foreach (FoodMovement f in list)
        {
            foods.Remove(f);
            if (f)
            {
                Destroy(f.gameObject);
                Destroy(f);
            }
        }
        //add new food
        int num = food - foods.Count;
        for (int i = 0; i < num; i++)
        {
            float xAxis = UnityEngine.Random.Range(Min, Max);
            float yAxis = UnityEngine.Random.Range(Min, Max);
            Vector3 randomPosition = new Vector3(xAxis, yAxis, 0);
            GameObject obj = Instantiate(food_obj, randomPosition, Quaternion.identity);
            obj.GetComponent<FoodMovement>().val = 0;
            obj.GetComponent<FoodMovement>().expiredTime = food_expiration_time;
            foods.Add(obj.GetComponent<FoodMovement>());
        }
        //for each agent solve compete
        //food
        List<FoodMovement> tempRemove = new List<FoodMovement>();
        foreach (FoodMovement fm in foods)
        {
            if (fm.solveCompete(food_value))
            {
                tempRemove.Add(fm);
            }
        }
        foreach (FoodMovement fm in tempRemove)
        {
            foods.Remove(fm);
            Destroy(fm.gameObject);
            Destroy(fm);
        }

        //dove
        List<DoveMovement> doveremove = new List<DoveMovement>();
        List<DoveMovement> doveadd = new List<DoveMovement>();
        foreach (DoveMovement dm in doves)
        {
            //reduce base energy
            dm.val -= energy_base_req * Time.deltaTime;
            //check death
            if (CheckDeath(dm.val))
            {
                doveremove.Add(dm);
            }
            //check offspring
            else if (checkOffspring(dm.val))
            {
                float xAxis = UnityEngine.Random.Range(Min, Max);
                float yAxis = UnityEngine.Random.Range(Min, Max);
                Vector3 randomPosition = new Vector3(xAxis, yAxis, 0);
                GameObject obj = GameObject.Instantiate(dove_obj, randomPosition, Quaternion.identity) as GameObject;
                dm.val = dm.val / 2;
                obj.GetComponent<DoveMovement>().val = dm.val;
                doveadd.Add(obj.GetComponent<DoveMovement>());
            }
        }
        foreach (DoveMovement dm in doveremove)
        {
            doves.Remove(dm);
            Destroy(dm.gameObject);
            Destroy(dm);
        }
        foreach (DoveMovement dm in doveadd)
        {
            doves.Add(dm);
        }

        //hawk
        List<HawkMovement> hawkremove = new List<HawkMovement>();
        List<HawkMovement> hawkadd = new List<HawkMovement>();
        foreach (HawkMovement hm in hawks)
        {
            //reduce base energy
            hm.val -= energy_base_req * Time.deltaTime;
            //Debug.Log(hm.val);
            //Debug.Log(Time.deltaTime);
            //check death
            if (CheckDeath(hm.val))
            {
                hawkremove.Add(hm);
            }
            //check offspring
            else if (checkOffspring(hm.val))
            {
                float xAxis = UnityEngine.Random.Range(Min, Max);
                float yAxis = UnityEngine.Random.Range(Min, Max);
                Vector3 randomPosition = new Vector3(xAxis, yAxis, 0);
                GameObject obj = GameObject.Instantiate(hawk_obj, randomPosition, Quaternion.identity) as GameObject;
                hm.val = hm.val / 2;
                obj.GetComponent<HawkMovement>().val = hm.val;
                hawkadd.Add(obj.GetComponent<HawkMovement>());
            }
        }
        foreach (HawkMovement hm in hawkremove)
        {
            hawks.Remove(hm);
            Destroy(hm.gameObject);
            Destroy(hm);
        }
        foreach (HawkMovement hm in hawkadd)
        {
            hawks.Add(hm);
        }
        if (next_step)
        {
            Time.timeScale = 0;
            next_step = false;
        }


        step += Time.deltaTime * 5f;

    }

    public void startBtn()
    {
        Time.timeScale = 1;
    }

    public void nextStep()
    {
        Time.timeScale = 1;
        next_step = true;
    }

    public void Stop()
    {
        Time.timeScale = 0;
    }

    private bool CheckDeath(float energy)
    {
        if (energy < death_threshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool checkOffspring(float energy)
    {
        if (energy > repoduction_threshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
