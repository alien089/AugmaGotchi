using System;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using Unity.AI.Navigation;

namespace Augma.GenerationNavMeshLinks
{
    // Dynamically generates Unity NavMeshLinks between spatial anchors detected by MRUK.
    // Scans room anchors tagged as obstacles/floors and connects nearby colliders
    // so that AI agents can navigate across separate walkable areas.
    public class GenerateNavLinks : MonoBehaviour
    {
        // Width of each generated NavMeshLink.
        public float linkWidth;

        // If true, agents can traverse the link in both directions.
        public bool bidirectionalLinks;

        // Stores the closest points between the two colliders currently being linked.
        private Vector3 _closestPointFromAToB;
        private Vector3 _closestPointFromBToA;

        // Extra offset applied when positioning the start/end of each link to avoid overlap.
        public float linkCompenstationAmount;

        // BoxColliders representing non-floor surfaces detected on the floor level.
        public List<BoxCollider> surfacesOnFloor = new List<BoxCollider>();
        // MeshColliders representing the floor itself.
        public List<MeshCollider> floor = new List<MeshCollider>();

        // Keeps track of colliders that have already been processed to avoid duplicate links.
        private List<Collider> _alreadyDone = new List<Collider>();
        // Stores all created NavMeshLink components for later cleanup.
        private List<NavMeshLink> _navMeshLinks = new List<NavMeshLink>();
        
        // If true, draw debug rays in the Scene view for visual inspection.
        public bool debugLines;

        // Maximum distance allowed between two colliders to create a link.
        public float fDistanceThreshold;

        // Cached list of all box colliders in the scene if needed.
        private BoxCollider[] _allBoxes;
        
        // Parent GameObject that will contain all generated NavMeshLink components.
        private GameObject _NavMeshLinksGO;

        // Indicates when we're linking floor meshes to surface colliders.
        private bool _bfloorNow = false;
        // Stores global start and end positions of links created when connecting floor objects.
        public List<Vector3> _floorAGlobal;
        public List<Vector3> _floorBGlobal;

        // Called once on start-up: sets up callbacks and prepares a container for NavMeshLinks.
        private void Start()
        {
            // Register a callback to automatically generate links once MRUK finishes loading the scene.
            MRUK.Instance.RegisterSceneLoadedCallback(DoGenerateLinks);
            
            // Create an empty GameObject to parent all generated NavMeshLinks for easy cleanup.
            _NavMeshLinksGO = new GameObject
            {
                name = "NavMeshLinks", 
                transform =
                {
                    parent = transform
                }
            };

            // Optionally, you could hook into the RoomCreatedEvent instead of the scene callback.
            // MRUK.Instance.RoomCreatedEvent.AddListener(DoGenerateLinksEvent);
        }

        // Entry point when the scene is fully loaded: clears cached lists, collects anchors, and links them.
        public void DoGenerateLinks()
        {
            surfacesOnFloor.Clear();
            floor.Clear();
            
            GetNavLinkTagTypes(MRUK.Instance.GetCurrentRoom());
            ConnectThemAll();
        }
        
        // Alternative entry point if invoked directly from an MRUK room event.
        public void DoGenerateLinksEvent(MRUKRoom room)
        {
            surfacesOnFloor.Clear();
            floor.Clear();
            
            GetNavLinkTagTypes(room);
            ConnectThemAll();
        }

        // Connects all eligible colliders by creating NavMeshLinks wherever the distance threshold is met.
        private void ConnectThemAll()
        {
            // First, connect all obstacle surfaces to each other.
            IfDistanceOkThenConnect(surfacesOnFloor, surfacesOnFloor);
            _alreadyDone.Clear();
            _bfloorNow = true;
            // Then, connect floor meshes to obstacle surfaces.
            IfDistanceOkThenConnect(floor, surfacesOnFloor);
        }

        // Collects anchors with specific MRUK tags and separates them into floor or surface lists.
        public void GetNavLinkTagTypes(MRUKRoom room = null)
        {
            if (!MRUK.Instance)
            {
                throw new NullReferenceException("MRUK instance is not initialized.");
            }
            var rooms = room != null ? new List<MRUKRoom> { room } : MRUK.Instance.Rooms;
            if (rooms.Count == 0)
            {
                throw new InvalidOperationException("No rooms available for NavMesh building.");
            }
            
            List<MRUKAnchor> sceneAnchors = rooms[0].Anchors;

            // Fetch the obstacle label definition from a SceneNavigation object in the scene.
            MRUKAnchor.SceneLabels x = FindObjectOfType<SceneNavigation>().SceneObstacles;

            foreach (var anchor in sceneAnchors)
            {
                // Skip anchors that are not labeled as obstacles.
                if (!anchor.HasAnyLabel(x)) continue;

                // Distinguish between anchors tagged as floor and other surfaces.
                if (!anchor.HasAnyLabel(MRUKAnchor.SceneLabels.FLOOR))
                    surfacesOnFloor.Add(anchor.GetComponentInChildren<BoxCollider>());
                else
                    floor.Add(anchor.GetComponentInChildren<MeshCollider>());
            }
        }
        
        // Iterates through every pair of colliders in two lists and connects them if close enough.
        private void IfDistanceOkThenConnect<T, T2>(List<T> aList, List<T2> bList)
            where T : Collider 
            where T2 : Collider
        {
            foreach (var colliderX in aList)
            {
                _alreadyDone.Add(colliderX);

                foreach (var colliderY in bList)
                {
                    // Avoid processing the same pair twice.
                    if (_alreadyDone.Contains(colliderY)) continue;

                    // Check if the two colliders are within the allowed distance.
                    if (IsObjectCloseEnough(colliderX, colliderY))
                    {
                        ConnectTheLinks(colliderX, colliderY);
                    }
                }
            }
        }

        // Returns true if the shortest distance between two colliders is below the threshold.
        private bool IsObjectCloseEnough(Collider a, Collider b)
        {
            // Ignore if both colliders belong to the same GameObject.
            if (string.CompareOrdinal(a.gameObject.name, b.gameObject.name) == 0) return false;

            var aCenter = GetColliderCenter(a);

            var closestFromAToB = a.ClosestPoint(b.ClosestPoint(aCenter));
            var closestFromBToA = b.ClosestPoint(closestFromAToB);
            var distance = Vector3.Distance(closestFromAToB, closestFromBToA);

            return distance <= fDistanceThreshold;
        }

        // Creates a NavMeshLink between two colliders after computing the best connection points.
        private void ConnectTheLinks(Collider a, Collider b)
        {
            GetClosestPointsToEachOther(a, b);
            var link = CreateLinkOnCollider(a);
            SetNavMeshLinkData(link, a);
            AdjustLinks(link, a, b);
            
            _navMeshLinks.Add(link);
        }

        // Computes the closest points from each collider to the other for precise link placement.
        private void GetClosestPointsToEachOther(Collider a, Collider b)
        {
            var aCenter = GetColliderCenter(a);
            var bCenter = GetColliderCenter(b);
            _closestPointFromAToB = a.ClosestPoint(b.ClosestPoint(aCenter));
            _closestPointFromBToA = b.ClosestPoint(a.ClosestPoint(bCenter));
        }

        // Adds a NavMeshLink component under the global container object.
        private NavMeshLink CreateLinkOnCollider(Collider coll)
        {
            return _NavMeshLinksGO.gameObject.AddComponent<NavMeshLink>();
            // The following return is unreachable and can be removed.
            return coll.gameObject.AddComponent<NavMeshLink>();
        }

        // Sets the main properties of a NavMeshLink, including start/end points and width.
        private void SetNavMeshLinkData(NavMeshLink link, Collider a)
        {
            Vector3 apos = _NavMeshLinksGO.transform.InverseTransformPoint(_closestPointFromAToB);
            Vector3 bpos = _NavMeshLinksGO.transform.InverseTransformPoint(_closestPointFromBToA);
            link.startPoint = _closestPointFromAToB;
            link.endPoint = _closestPointFromBToA;
            link.bidirectional = bidirectionalLinks;
            link.width = linkWidth;

            // Store the points globally if we're currently connecting floor colliders.
            if (_bfloorNow == true)
            {
                _floorAGlobal.Add(_closestPointFromAToB);
                _floorBGlobal.Add(_closestPointFromBToA);
            }
        }

        // Applies a small positional adjustment along the inward normal to reduce clipping.
        private void AdjustLinks(NavMeshLink link, Collider a, Collider b)
        {
            Vector3 aCenter = GetColliderCenter(a);

            Vector3 directionFromACenterToLinkStart = -(_closestPointFromAToB - aCenter).normalized;
            if (debugLines == true)
            {
                Debug.DrawRay(_closestPointFromAToB, directionFromACenterToLinkStart, Color.green, 99);
            }

            Ray aRay = new Ray(_closestPointFromAToB, directionFromACenterToLinkStart);
            Vector3 aPos = aRay.GetPoint(linkCompenstationAmount);

            Vector3 bCenter = GetColliderCenter(b);

            Vector3 directionFromBTransformToLinkEnd = -(_closestPointFromBToA - bCenter).normalized;
            if (debugLines == true)
            {
                Debug.DrawRay(_closestPointFromBToA, directionFromBTransformToLinkEnd, Color.red, 99);
            }

            Ray bRay = new Ray(_closestPointFromBToA, directionFromBTransformToLinkEnd);
            Vector3 bPos = bRay.GetPoint(linkCompenstationAmount);

            link.startPoint = _NavMeshLinksGO.transform.InverseTransformPoint(aPos);
            link.endPoint = _NavMeshLinksGO.transform.InverseTransformPoint(bPos);
        }
        
        // Computes the world-space center of a collider, with a Y offset to roughly match its top surface.
        private Vector3 GetColliderCenter(Collider coll)
        {
            Vector3 rtn;
            if (coll is BoxCollider box) rtn = coll.transform.TransformPoint(box.center);
            else rtn = coll.bounds.center;

            rtn.y += coll.bounds.size.y / 2;
            return rtn;
        }

        // Removes all generated NavMeshLinks and clears cached collider lists.
        public void ClearNavMeshLinks()
        {
            foreach (NavMeshLink link in _navMeshLinks) Destroy(link);
            
            surfacesOnFloor.Clear();
            floor.Clear();
        }
    }
}
