using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    void Update(){
        if(this.gameObject.transform.position.y <= 0.2f){
            this.gameObject.SetActive(false);
            
        }
    }
    
}
