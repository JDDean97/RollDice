using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    int roomCount = 3;
    int enemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Cursor.visible = false;
        FindObjectOfType<Room>().createNeighbors(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void roomCountEnemy()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount - 1 > 0)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Gate"))
            {
                g.GetComponent<Animator>().SetBool("Open", false);
                FindObjectOfType<DJ>().playSound("gate");
            }
        }
        else
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Gate"))
            {
                g.GetComponent<Animator>().SetBool("Open", true);
            }
        }
    }

    public void countEnemy()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount-1 > 0)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Gate"))
            {
                g.GetComponent<Animator>().SetBool("Open", false);
            }
        }
        else
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Gate"))
            {
                g.GetComponent<Animator>().SetBool("Open", true);
            }
        }

        if(roomCount==0 && enemyCount-1 ==0)
        {
            gameOver(true);
        }

    }

    public void gameOver(bool victory)
    {
        Time.timeScale = 0;
        if(victory)
        {
            FindObjectOfType<Canvas>().transform.Find("VictoryScreen").gameObject.SetActive(true);
        }
        else
        {
            FindObjectOfType<Canvas>().transform.Find("LoserScreen").gameObject.SetActive(true);
        }
        Cursor.visible = true;
        StartCoroutine(delayLoad());
    }

    IEnumerator delayLoad()
    {
        float time = 2;
        
        while(time>0)
        {
            time -= 1 * Time.fixedUnscaledDeltaTime;
            Debug.Log("Coroutine timer: " + time);
            yield return null;
        }
        Time.timeScale = 1;
        StartCoroutine(Loader.loadScene("Menu"));
    }

    public int getRoomCount()
    {
        return roomCount;
    }

    public void setRoomCount(int i)
    {
        roomCount = i;
    }
}
