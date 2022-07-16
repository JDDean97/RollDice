using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class menuDir : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TXTspeed;
    [SerializeField]
    TextMeshProUGUI TXTaccel;
    [SerializeField]
    TextMeshProUGUI TXTbrake;
    [SerializeField]
    TextMeshProUGUI TXThandle;
    [SerializeField]
    TextMeshProUGUI TXTboost;
    // Start is called before the first frame update
    void Start()
    {
        generateStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        StartCoroutine(Loader.loadScene("SampleScene"));
        
    }

    public void quit()
    {
        Application.Quit();
    }

    public void generateStats()
    {
        int points = 25;
        Dictionary<string, float> stats = new Dictionary<string, float>(){
            { "speed", 1 },
            { "accel", 1 },
            { "turn", 1 },
            { "boost", 1 },
            { "brake", 1 },
        };
        Dictionary<string, float> statsNew = new Dictionary<string, float>();
        foreach(KeyValuePair<string,float> kvp in stats)
        {
            int temp = Random.Range(1, 10);
            if(temp>points)
            { temp = points; }
            points -= temp;

            statsNew.Add(kvp.Key, 1 + temp);
            PlayerPrefs.SetFloat(kvp.Key, 1+temp);
        }
        Color col = new Color(0, 0, 0);
        int colnum = Random.Range(0, 3);
        switch(colnum)
        {
            case 0:
                col = Color.black;
                break;
            case 1:
                col = Color.magenta;
                break;
            case 2:
                col = Color.red;
                break;
        }
        Resources.Load<Material>("Materials/CarBody").color = col;

        sheetUpdate(statsNew);

    }

    void sheetUpdate(Dictionary<string,float> dic)
    {
        TXTaccel.text = dic["accel"].ToString();
        TXTspeed.text = dic["speed"].ToString();
        TXTbrake.text = dic["brake"].ToString();
        TXThandle.text = dic["turn"].ToString();
        TXTboost.text = dic["boost"].ToString();
    }
}
