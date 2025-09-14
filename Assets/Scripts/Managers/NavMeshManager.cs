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
    // Central controller that initializes NavMesh-related components,
    // ensures proper agent type assignment, and activates runtime NavMesh linking.
    public class NavMeshManager : MonoBehaviour
    {
        // Cached references to scene components involved in navigation.
        private SceneNavigation _sceneNavigation;
        private EffectMesh _effectMesh;
        private GenerateNavLinks _generateNavLinks;

        // Called when the object is instantiated, before Start().
        public void Awake()
        {
            // Find and temporarily disable key components so we can enable them in a controlled order.
            _sceneNavigation = FindObjectOfType<SceneNavigation>();
            _sceneNavigation.enabled = false;
            
            _effectMesh = FindObjectOfType<EffectMesh>();
            _effectMesh.enabled = false;
            
            _generateNavLinks = FindObjectOfType<GenerateNavLinks>();
            _generateNavLinks.enabled = false;
        }

        // Called on the first frame after Awake().
        private void Start()
        {
            // Re-enable components in a defined sequence once the scene is ready.
            _effectMesh.enabled = true;
            _generateNavLinks.enabled = true;
            _sceneNavigation.enabled = true;

            GameManager.Instance.EventManager.Register(ToyEventList.GET_NAVMESH_AGENT, SetAgentNavMesh);
        }

        // Waits briefly to ensure NavMesh data is available, then sets the agent type for both the NavMeshAgent and all generated links.
        private void SetAgentNavMesh(object[] param)
        {
            // Retrieve the NavMeshSurface inside this GameObject's children.
            NavMeshSurface surf = GetComponentInChildren<NavMeshSurface>();

            // Obtain the ID of the most recently defined NavMesh agent type.
            int count = NavMesh.GetSettingsCount();
            int id = NavMesh.GetSettingsByIndex(count - 1).agentTypeID;
            
            // Assign this agent type to the single NavMeshAgent in the scene.
            NavMeshAgent agent = FindObjectOfType<NavMeshAgent>();
            agent.agentTypeID = id;

            // Apply the same agent type to every existing NavMeshLink so the agent can traverse them.
            NavMeshLink[] linkList = FindObjectsOfType<NavMeshLink>();
            foreach (NavMeshLink link in linkList)
            {
                link.agentTypeID = id;
            }
        }
    }
}
