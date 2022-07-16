using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    int roomCount = 8;
    int enemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        FindObjectOfType<Room>().createNeighbors(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getRoomCount()
    {
        return roomCount;
    }

    public void setRoomCount(int i)
    {
        roomCount = i;
    }

    public void generateRooms()
    {

    }
}
