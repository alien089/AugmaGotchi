using System;
using Augma.GenerationNavMeshLinks;
using Framework.Generics.Pattern.SingletonPattern;
using Meta.XR.MRUtilityKit;
using Misc;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private EventManager _xEventManager = Factory.CreateEventManager();
        private SaveManager _xSaveManager; 
        private AudioManager _xAudioManager; 
        private EntityManager _xEntityManager; 

        public EventManager EventManager { get => _xEventManager; }
        public SaveManager SaveManager { get => _xSaveManager; }
        public AudioManager AudioManager { get => _xAudioManager; }
        public EntityManager EntityManager { get => _xEntityManager; }

        public override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            
            //_xSaveManager = GetComponentInChildren<SaveManager>();
            _xAudioManager = GetComponentInChildren<AudioManager>();
            _xEntityManager = GetComponentInChildren<EntityManager>();
        }
    }
}
