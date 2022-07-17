using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DJ : MonoBehaviour
{
    [SerializeField]
    AudioMixer mixer;
    [SerializeField]
    AudioSource engine;
    [SerializeField]
    AudioSource skid;
    [SerializeField]
    AudioSource gate;
    [SerializeField]
    AudioSource crash;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        engine.pitch = 0.5f + (player.getSpeed()*0.03f);
    }

    public void playSound(string s)
    {
        switch(s)
        {
            case "skid":
                if (!skid.isPlaying)
                {
                    skid.Play();
                }
                break;
            case "gate":
                if (!gate.isPlaying)
                {
                    gate.Play();
                }
                break;
            case "crash":
                if (!crash.isPlaying)
                {
                    crash.pitch = Random.Range(0.5f, 1.5f);
                    crash.Play();
                }
                break;
        }
    }

    public void stopSound(string s)
    {
        switch(s)
        {
            case "skid":
                skid.Stop();
                break;
        }
    }
}
