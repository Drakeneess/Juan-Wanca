using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    [SerializeField]
    private Item item;
    private ItemController itemController;
    // Start is called before the first frame update
    void Start()
    {
        // Iniciar una coroutine para retrasar la búsqueda del item por un frame
        StartCoroutine(FindItemNextFrame());
    }

    // Coroutine para esperar un frame antes de buscar el item
    IEnumerator FindItemNextFrame()
    {
        yield return null;  // Espera un frame
        item = GetComponentInChildren<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        if(item != null){
            if(item.IsPickedUp()){
                Destroy(gameObject);
            }
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
