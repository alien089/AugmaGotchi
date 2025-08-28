using System;
using System.Collections;
using Augma.GenerationNavMeshLinks;
using Framework.Generics.Pattern.SingletonPattern;
using Meta.XR.MRUtilityKit;
using Misc;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using NavMeshSurface = Oculus.Interaction.Surfaces.NavMeshSurface;

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
            
            NavMeshSurface surf = GetComponentInChildren<NavMeshSurface>();
            int count = NavMesh.GetSettingsCount();
            int id = NavMesh.GetSettingsByIndex(count - 1).agentTypeID;
            
            NavMeshAgent agent = FindObjectOfType<NavMeshAgent>();
            agent.agentTypeID = id;

            NavMeshLink[] linkList = FindObjectsOfType<NavMeshLink>();
            foreach (NavMeshLink link in linkList)
            {
                link.agentTypeID = id;
            }
        }
    }
}
