using System.Collections;
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
        int slimes = Random.Range(0, 2);
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

        for (int iter = 0; iter < slimes; iter++)
        {
            GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/slimey"));
            offset.x = Random.Range(-max, max);
            offset.z = Random.Range(-max, max);
            g.transform.position = transform.position + offset;
        }

        
    }

    public void spawnDec()
    {
        int decorations = Random.Range(5, 13);
        Vector3 offset = Vector3.zero;
        float max = GetComponent<Collider>().bounds.extents.x - 10;
        for (int iter = 0; iter < decorations; iter++)
        {
            string dec = "crate";
            if (Random.Range(0, 2) > 0)
            {
                dec = "pillar";
            }
            GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/" + dec));
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
            GameObject gate = Resources.Load<GameObject>("Prefabs/Gate");
            Vector3 offset = Vector3.zero;
            string relation = "";
            string wallName = "";
            string opposite = "";
            switch (iter)
            {
                case 0:
                    offset = new Vector3(0,0,dist);
                    relation = "down";
                    wallName = "WallUp";
                    opposite = "WallDown";
                    break;
                case 1:
                    offset = new Vector3(dist, 0, 0);
                    relation = "left";
                    wallName = "WallRight";
                    opposite = "WallLeft";
                    break;
                case 2:
                    offset = new Vector3(0, 0, -dist);
                    relation = "up";
                    wallName = "WallDown";
                    opposite = "WallUp";
                    break;
                case 3:
                    offset = new Vector3(-dist, 0, 0);
                    relation = "right";
                    wallName = "WallLeft";
                    opposite = "WallRight";
                    break;
            }

            GameObject room = Instantiate(Resources.Load<GameObject>("Prefabs/Floor"),transform.position + offset,Quaternion.identity);
            room.GetComponent<Room>().neighbors.Add(relation, this);
            int nameNum = Random.Range(1, 4);
            room.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/matFloor" + nameNum);
            room.GetComponent<Room>().spawnDec();
            
            neighbors.Add(directions[iter], room.GetComponent<Room>());
            GameObject ga = Instantiate(gate, transform.Find(wallName).position, transform.Find(wallName).rotation);
            Quaternion rot = ga.transform.rotation;
            rot.eulerAngles += new Vector3(90, 0, 0);
            ga.transform.rotation = rot;
            ga.transform.parent = room.transform;


            Destroy(transform.Find(wallName).gameObject);
            Destroy(room.transform.Find(opposite).gameObject);

            num--;
            FindObjectOfType<Director>().setRoomCount(FindObjectOfType<Director>().getRoomCount() - 1);
            
        }
        GetComponentInParent<NavMeshSurface>().BuildNavMesh();
        spawnStuff();
        FindObjectOfType<Director>().roomCountEnemy();


    }
}
