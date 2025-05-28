using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Generics.Pattern.SingletonPattern;
using Framework.Generics.Pattern.FactoryPattern;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private EventManager _xEventManager = Factory.CreateEventManager();
    private SaveManager _xSaveManager; 

    public EventManager EventManager { get => _xEventManager; }
    public SaveManager SaveManager { get => _xSaveManager; }

    private void Start()
    {
        _xSaveManager = GetComponentInChildren<SaveManager>();
    }

    public void WinGame(object[] param)
    {
        SceneManager.LoadScene((int)param[0]);
    }
}
