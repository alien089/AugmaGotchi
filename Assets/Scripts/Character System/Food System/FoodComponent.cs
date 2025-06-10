using System;
using System.Collections;
using System.Collections.Generic;
using Character_System.Food_System;
using UnityEngine;

public class FoodComponent : MonoBehaviour
{
    private SphereCollider _xSphereCollider; 
    
    void Start()
    {
        _xSphereCollider = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (TryGetComponent(out FoodInteractable hinge))
        {
            
        }
    }
}
