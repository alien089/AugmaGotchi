using System.Collections;
using System.Collections.Generic;
using Enums;
using Managers;
using Oculus.Interaction;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CaressDetector : MonoBehaviour
{
    private SphereCollider _xCollider;
    private bool _bIsHovering;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_bIsHovering || other.GetComponentInParent<IInteractorView>() == null) return;
        
        IInteractorView interactable = other.GetComponentInParent<IInteractorView>();
        if (!interactable.HasSelectedInteractable)
        {
            _bIsHovering = true;
            GameManager.Instance.EventManager.TriggerEvent(CaressEventList.WANT_CARESS);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!_bIsHovering || other.GetComponentInParent<IInteractorView>() == null) return;

        IInteractorView interactable = other.GetComponentInParent<IInteractorView>();
        if (!interactable.HasSelectedInteractable)
        {
            _bIsHovering = false;
            GameManager.Instance.EventManager.TriggerEvent(CaressEventList.NOT_WANT_CARESS);
        }
    }
}
