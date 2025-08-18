using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

namespace Augma.GenerationNavMeshLinks
{
    public class GenerateNavLinks : MonoBehaviour
    {
        public float linkWidth;
        public bool bidirectionalLinks;
        private Vector3 _closestPointFromAToB;
        private Vector3 _closestPointFromBToA;
        public float linkCompenstationAmount;
        public List<BoxCollider> floors = new List<BoxCollider>();
        public bool debugLines;
        public float wallConnectThreshold;
        private BoxCollider[] _allBoxes;

        
        public void DoGenerateLinks()
        {
            GetNavLinkTagTypes();
            //SeparateLinkTagTypes();
            ConnectThemAll();
        }

        public void ConnectThemAll()
        {
            IfDistanceOkThenConnect(floors, floors);
        }
        
        public void GetNavLinkTagTypes()
        {
            _allBoxes = GetComponentsInChildren<BoxCollider>();
        }

        // public void SeparateLinkTagTypes()
        // {
        //     for (int index = 0; index < _allBoxes.Length; index++)
        //     {
        //         var box = _allBoxes[index];
        //
        //         var i = box.gameObject.GetComponent<NavLinkTag>();
        //
        //         if(i != null)
        //         {
        //             var ii = i.RoomPieceType;
        //
        //             if (ii == NavLinkTag.typo.Floor)
        //             {
        //                 //var iii = i.gameObject.GetComponents<BoxCollider>();
        //                 floors.Add(box);
        //             }
        //         }
        //     }
        // }

        public void ConnectAListToBList(List<BoxCollider> aList, List<BoxCollider> bList)
        {
            for (int index = 0; index < aList.Count; index++)
            {
                var i = aList[index];

                for (int index1 = 0; index1 < bList.Count; index1++)
                {
                    var ii = bList[index1];
                    ConnectTheLinks(i, ii);
                }
            }

        }

        public void IfDistanceOkThenConnect(List<BoxCollider> aList, List<BoxCollider> bList)
        {
            for (int index = 0; index < aList.Count; index++)
            {
                var i = aList[index];

                for (int index1 = 0; index1 < bList.Count; index1++)
                {
                    var ii = bList[index1];

                    if (IsObjectCloseEnough(i, ii))
                    {
                        ConnectTheLinks(i, ii);
                    }

                }
            }

        }

        public bool IsObjectCloseEnough(Collider a, Collider b)
        {

            if (string.Compare(a.gameObject.name, b.gameObject.name) == 0)
            {
                return false;
            }

            var boxCenter = a.GetComponent<BoxCollider>().center;
            var aCenter = a.transform.TransformPoint(boxCenter);

            var closestFromAToB = a.ClosestPoint(b.ClosestPoint(aCenter));
            var closestFromBToA = b.ClosestPoint(closestFromAToB);
            var distance = Vector3.Distance(closestFromAToB, closestFromBToA);

            if (distance <= wallConnectThreshold)
            {
                return true;
            } else
            {
                return false;
            }

        }

        public void ConnectTheLinks(Collider a, Collider b)
        {
            GetClosestPointsToEachOther(a, b);
            var link = CreateLinkOnCollider(a);
            SetNavMeshLinkData(link, a);
            AdjustLinks(link, a, b);
        }

        public void GetClosestPointsToEachOther(Collider a, Collider b)
        {
            var aCenter = GetBoxCenterPosition(a, a.transform);
            _closestPointFromAToB = a.ClosestPoint(b.ClosestPoint(aCenter));
            _closestPointFromBToA = b.ClosestPoint(_closestPointFromAToB);
        }

        public NavMeshLink CreateLinkOnCollider(Collider coll)
        {
            return coll.gameObject.AddComponent<NavMeshLink>();
        }

        public void SetNavMeshLinkData(NavMeshLink link, Collider a)
        {
            link.startPoint = a.transform.InverseTransformPoint(_closestPointFromAToB);
            link.endPoint = a.transform.InverseTransformPoint(_closestPointFromBToA);
            link.bidirectional = bidirectionalLinks;
            link.width = linkWidth;
        }

        public void AdjustLinks(NavMeshLink link, Collider a, Collider b)
        {
            var aCenter = GetBoxCenterPosition(a, a.transform);

            var directionFromACenterToLinkStart = -(_closestPointFromAToB - aCenter).normalized;
            if (debugLines == true)
            {
                Debug.DrawRay(_closestPointFromAToB, directionFromACenterToLinkStart, Color.green, 99);
            }

            Ray aRay = new Ray(_closestPointFromAToB, directionFromACenterToLinkStart);
            var aPos = aRay.GetPoint(linkCompenstationAmount);

            var bCenter = GetBoxCenterPosition(b, b.transform);

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

        public Vector3 GetBoxCenterPosition(Collider coll, Transform trans)
        {
            var box = coll.GetComponent<BoxCollider>().center;
            return trans.transform.TransformPoint(box);
        }

    }
}

