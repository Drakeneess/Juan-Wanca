using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class DoorItem : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Action(Transform player)
    {
        GetComponent<Door>().OpenDoor();
    }
}
