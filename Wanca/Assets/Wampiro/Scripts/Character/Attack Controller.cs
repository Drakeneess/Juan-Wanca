using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Weapon weapon;
    private Tackle tackle;
    // Start is called before the first frame update
    void Start()
    {
        tackle=GetComponent<Tackle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack(){
        if(weapon!=null){
        }
        else{
            tackle.TackleAttack();
        }
    }
}
