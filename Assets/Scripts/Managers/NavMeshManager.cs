using System;
using System.Collections;
using Augma.GenerationNavMeshLinks;
using Framework.Generics.Pattern.SingletonPattern;
using Meta.XR.MRUtilityKit;
using Misc;
using Oculus.Interaction.Surfaces;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class NavMeshManager : MonoBehaviour
    {
        private SceneNavigation _sceneNavigation;
        private EffectMesh _effectMesh;
        private GenerateNavLinks _generateNavLinks;

        public void Awake()
        {
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

            StartCoroutine(SetAgentNavMesh());
        }

        private IEnumerator SetAgentNavMesh()
        {
            yield return new WaitForSeconds(1f);
            
            NavMeshSurface x = GetComponentInChildren<NavMeshSurface>();
            int count = NavMesh.GetSettingsCount();
            int id = NavMesh.GetSettingsByIndex(count - 1).agentTypeID;
            
            NavMeshAgent agent = FindObjectOfType<NavMeshAgent>();
            agent.agentTypeID = id;
        }
    }
}
