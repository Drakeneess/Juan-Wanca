using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    private Item item;
    private ItemController itemController;
    // Start is called before the first frame update
    void Start()
    {
        item = GetComponentInChildren<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        if(item.IsPickedUp()){
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            itemController=other.GetComponent<ItemController>();
            if (itemController != null) {
                print("Player In Range");
                itemController.SetItemAwayState(false);
                itemController.GetItem(item);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if(itemController != null) {
                print("Player Out of Range");
                itemController.SetItemAwayState(true);
                itemController.RemoveItem();
                itemController=null;
            }

        }
    }
}
