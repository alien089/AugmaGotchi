using System.Collections;
using System.Collections.Generic;
using Framework.Generics.Pattern.SingletonPattern;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Transform _xCenterEyeAnchor;
    
    
    private Vector3 _fplayerPosition;
    public Vector3 FPlayerPosition => _fplayerPosition;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _fplayerPosition = _xCenterEyeAnchor.position;
    }
}
