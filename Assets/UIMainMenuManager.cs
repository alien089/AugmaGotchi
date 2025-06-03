using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private Vector3 MainMenuLocation;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(MainMenu, MainMenuLocation, Quaternion.identity);
    }
}
