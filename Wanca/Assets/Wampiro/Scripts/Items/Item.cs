using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool isPickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Action(Transform player){
        isPickedUp=true;
        transform.SetParent(player);
    }
    public bool IsPickedUp(){
        return isPickedUp;
    }
}
