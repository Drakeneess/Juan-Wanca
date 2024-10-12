using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private Item item;
    private bool itemAway=true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void GrabItem(){
        if(item != null && !itemAway){
            item.Action(transform);
        }
    }
    public void GetItem(Item newItem){
        item=newItem;
    }
    public void RemoveItem(){
        item=null;
    }

    public bool IsItemAway(){
        return itemAway;
    }
    public void SetItemAwayState(bool state){
        itemAway=state;
    }
}
