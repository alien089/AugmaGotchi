using System;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.Serialization;

namespace Augma.GenerationNavMeshLinks
{
    public class GenerateNavLinks : MonoBehaviour
    {
        // Width of the generated NavMeshLink
        public float linkWidth;

        // Whether the generated links should be bidirectional
        public bool bidirectionalLinks;

        // Temporary storage for closest connection points
        private Vector3 _closestPointFromAToB;
        private Vector3 _closestPointFromBToA;

        // How far the link endpoints should be adjusted inward (compensation)
        public float linkCompenstationAmount;

        // Lists of categorized colliders
        public List<BoxCollider> surfacesOnFloor = new List<BoxCollider>();
        public List<MeshCollider> floor = new List<MeshCollider>();
        private List<Collider> _alreadyDone = new List<Collider>();
        private List<NavMeshLink> _navMeshLinks = new List<NavMeshLink>();
        
        // Toggle debug drawing of rays in the scene
        public bool debugLines;

        // Max distance threshold to consider two objects "connected"
        public float fDistanceThreshold;

        // Holds all BoxColliders found in children
        private BoxCollider[] _allBoxes;

        //trigger link generation in Editor
        public void DoGenerateLinks()
        {
            surfacesOnFloor.Clear();
            floor.Clear();
            
            GetNavLinkTagTypes();
            ConnectThemAll();
        }

        // Connects all categorized objects together based on distance rules
        private void ConnectThemAll()
        {
            IfDistanceOkThenConnect(surfacesOnFloor, surfacesOnFloor);
            _alreadyDone.Clear();
            IfDistanceOkThenConnect(floor, surfacesOnFloor);
        }

        // Finds all NavLinkTags and colliders in children
        public void GetNavLinkTagTypes()
        {
            List<MRUKAnchor> sceneAnchors = MRUK.Instance.GetCurrentRoom().Anchors;

            MRUKAnchor.SceneLabels x = GetComponent<SceneNavigation>().SceneObstacles;

            foreach (var anchor in sceneAnchors)
            {
                if (!anchor.HasAnyLabel(x)) continue;

                if (!anchor.HasAnyLabel(MRUKAnchor.SceneLabels.FLOOR))
                    surfacesOnFloor.Add(anchor.GetComponentInChildren<BoxCollider>());
                else
                    floor.Add(anchor.GetComponentInChildren<MeshCollider>());
            }
        }
        
        // Connects colliders only if they are close enough
        private void IfDistanceOkThenConnect<T, T2>(List<T> aList, List<T2> bList)
            where T : Collider 
            where T2 : Collider
        {
            foreach (var colliderX in aList)
            {
                _alreadyDone.Add(colliderX);

                foreach (var colliderY in bList)
                {
                    if (_alreadyDone.Contains(colliderY)) continue;

                    if (IsObjectCloseEnough(colliderX, colliderY))
                    {
                        ConnectTheLinks(colliderX, colliderY);
                    }
                }
            }
        }

        // Checks if two colliders are within threshold distance
        private bool IsObjectCloseEnough(Collider a, Collider b)
        {
            // Skip if it's the same object
            if (string.CompareOrdinal(a.gameObject.name, b.gameObject.name) == 0) return false;

            var aCenter = GetColliderCenter(a);

            var closestFromAToB = a.ClosestPoint(b.ClosestPoint(aCenter));
            var closestFromBToA = b.ClosestPoint(closestFromAToB);
            var distance = Vector3.Distance(closestFromAToB, closestFromBToA);

            return distance <= fDistanceThreshold;
        }

        // Creates and configures a NavMeshLink between two colliders
        private void ConnectTheLinks(Collider a, Collider b)
        {
            GetClosestPointsToEachOther(a, b);
            var link = CreateLinkOnCollider(a);
            SetNavMeshLinkData(link, a);
            AdjustLinks(link, a, b);
            
            _navMeshLinks.Add(link);
        }

        // Finds closest points between two colliders
        private void GetClosestPointsToEachOther(Collider a, Collider b)
        {
            var aCenter = GetColliderCenter(a);
            var bCenter = GetColliderCenter(b);
            _closestPointFromAToB = a.ClosestPoint(b.ClosestPoint(aCenter));
            _closestPointFromBToA = b.ClosestPoint(a.ClosestPoint(bCenter));
        }

        // Creates a NavMeshLink component on a collider
        private NavMeshLink CreateLinkOnCollider(Collider coll)
        {
            return coll.gameObject.AddComponent<NavMeshLink>();
        }

        // Sets initial NavMeshLink properties
        private void SetNavMeshLinkData(NavMeshLink link, Collider a)
        {
            link.startPoint = a.transform.InverseTransformPoint(_closestPointFromAToB);
            link.endPoint = a.transform.InverseTransformPoint(_closestPointFromBToA);
            link.bidirectional = bidirectionalLinks;
            link.width = linkWidth;
        }

        // Adjusts NavMeshLink start and end points inward for better alignment
        private void AdjustLinks(NavMeshLink link, Collider a, Collider b)
        {
            var aCenter = GetColliderCenter(a);

            var directionFromACenterToLinkStart = -(_closestPointFromAToB - aCenter).normalized;
            if (debugLines == true)
            {
                Debug.DrawRay(_closestPointFromAToB, directionFromACenterToLinkStart, Color.green, 99);
            }

            Ray aRay = new Ray(_closestPointFromAToB, directionFromACenterToLinkStart);
            var aPos = aRay.GetPoint(linkCompenstationAmount);

            var bCenter = GetColliderCenter(b);

            var directionFromBTransformToLinkEnd = -(_closestPointFromBToA - bCenter).normalized;
            if (debugLines == true)
            {
                Debug.DrawRay(_closestPointFromBToA, directionFromBTransformToLinkEnd, Color.red, 99);
            }

            Ray bRay = new Ray(_closestPointFromBToA, directionFromBTransformToLinkEnd);
            var bPos = bRay.GetPoint(linkCompenstationAmount);

            link.startPoint = a.transform.InverseTransformPoint(aPos);
            link.endPoint = a.transform.InverseTransformPoint(bPos);
        }
        
        // Returns the world position of a collider center
        private Vector3 GetColliderCenter(Collider coll)
        {
            Vector3 rtn;
            if (coll is BoxCollider box) rtn = coll.transform.TransformPoint(box.center);
            else rtn = coll.bounds.center;

            rtn.y += coll.bounds.size.y / 2;
            return rtn;
        }

        public void ClearNavMeshLinks()
        {
            foreach (NavMeshLink link in _navMeshLinks) Destroy(link);
            
            surfacesOnFloor.Clear();
            floor.Clear();
        }
    }
}

