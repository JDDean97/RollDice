﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    public Dictionary<string, Room> neighbors;
    bool canGenerate = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        neighbors = new Dictionary<string, Room>();
        transform.parent = GameObject.Find("Terrain").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnStuff()
    {
        int gobs = Random.Range(5, 15);
        int porcs = Random.Range(0, 3);
        int decorations = Random.Range(5, 13);
        Vector3 offset = Vector3.zero;
        float max = GetComponent<Collider>().bounds.extents.x -10;
        for(int iter = 0;iter<gobs;iter++)
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/gobbo"));
            offset.x = Random.Range(-max, max);
            offset.z = Random.Range(-max, max);
            g.transform.position = transform.position + offset;
        }

        for (int iter = 0; iter < porcs; iter++)
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/porc"));
            offset.x = Random.Range(-max, max);
            offset.z = Random.Range(-max, max);
            g.transform.position = transform.position + offset;
        }

        for(int iter = 0;iter<decorations;iter++)
        {
            string dec = "crate";
            if(Random.Range(0,2)>0)
            {
                dec = "pillar";
            }
            GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/"+dec));
            offset.x = Random.Range(-max, max);
            offset.z = Random.Range(-max, max);
            g.transform.position = transform.position + offset;
        }
    }

    public void createNeighbors(int num = 4)
    {
        if(!canGenerate)
        { return; }

        canGenerate = false;
        string[] directions = { "up", "right", "down", "left" };
        for(int iter = 0;iter<4;iter++)
        {
            if(neighbors.ContainsKey(directions[iter])||num<1)
            { continue; }

            
            float dist = GetComponent<Collider>().bounds.size.x;
            Vector3 offset = Vector3.zero;
            string relation = "";
            switch(iter)
            {
                case 0:
                    offset = new Vector3(0,0,dist);
                    relation = "down";
                    break;
                case 1:
                    offset = new Vector3(dist, 0, 0);
                    relation = "left";
                    break;
                case 2:
                    offset = new Vector3(0, 0, -dist);
                    relation = "up";
                    break;
                case 3:
                    offset = new Vector3(-dist, 0, 0);
                    relation = "right";
                    break;
            }
            GameObject room = Instantiate(Resources.Load<GameObject>("Prefabs/Floor"),transform.position + offset,Quaternion.identity);
            room.GetComponent<Room>().neighbors.Add(relation, this);
            int nameNum = Random.Range(1, 4);
            room.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/matFloor" + nameNum);
            //room.transform.position = transform.position + offset;
            neighbors.Add(directions[iter], room.GetComponent<Room>());
            num--;
            FindObjectOfType<Director>().setRoomCount(FindObjectOfType<Director>().getRoomCount() - 1);
        }
        GetComponentInParent<NavMeshSurface>().BuildNavMesh();
        spawnStuff();

        
    }
}
