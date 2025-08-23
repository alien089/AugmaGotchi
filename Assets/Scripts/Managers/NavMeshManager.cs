using System;
using Augma.GenerationNavMeshLinks;
using Framework.Generics.Pattern.SingletonPattern;
using Meta.XR.MRUtilityKit;
using Misc;
using Oculus.Interaction.Surfaces;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class NavMeshManager : Singleton<GameManager>
    {
        private SceneNavigation _sceneNavigation;
        private EffectMesh _effectMesh;
        private GenerateNavLinks _generateNavLinks;

        public override void Awake()
        {
            base.Awake();
            
            _sceneNavigation = FindObjectOfType<SceneNavigation>();
            _sceneNavigation.enabled = false;
            
            _effectMesh = FindObjectOfType<EffectMesh>();
            _effectMesh.enabled = false;
            
            _generateNavLinks = FindObjectOfType<GenerateNavLinks>();
            _generateNavLinks.enabled = false;
        }

        private void Start()
        {
            _effectMesh.enabled = true;
            _generateNavLinks.enabled = true;
            _sceneNavigation.enabled = true;

            NavMeshSurface x = GetComponentInChildren<NavMeshSurface>();
            int count = NavMesh.GetSettingsCount();
            int id = NavMesh.GetSettingsByIndex(count - 1).agentTypeID;
        }
    }
}
