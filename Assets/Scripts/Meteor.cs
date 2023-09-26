using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour{
        
    public float maxLifeTime = 3f, timer = 0f;
    void Update(){
        timer += Time.deltaTime;
        if (timer >= maxLifeTime)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }
        
    }
}