using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{

    public LevelManager lman;

    // Start is called before the first frame update
    void Start()
    {
        lman = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            print ("funciona");
            lman.currentCheckpoint = gameObject;
        }
    }
}
