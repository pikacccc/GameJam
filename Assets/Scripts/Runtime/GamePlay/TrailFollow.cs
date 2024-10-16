using System.Collections;
using UnityEngine;

namespace Runtime.GamePlay
{
    public class TrailFollow : MonoBehaviour
    {
        public Transform FollowFirstPoint;
        public Transform FollowLastPoint;
        public LineRenderer line;
        public float speed;
        public bool UseLast;
        public Vector3 force;

        private void Start()
        {
            StartCoroutine(Follow());
        }

        private void OnEnable()
        {
            StartCoroutine(Follow());
        }

        private void OnDisable()
        {
            StopCoroutine(Follow());
        }


        private void Update()
        {
            if (FollowFirstPoint) line.SetPosition(0, FollowFirstPoint.transform.position);
            if (FollowLastPoint != null && UseLast)
                line.SetPosition(line.positionCount - 1, FollowLastPoint.transform.position);
        }

        public void SetUseLast(bool use)
        {
            this.UseLast = use;
        }

        public void SetLastFollowPos(Transform point)
        {
            this.FollowLastPoint = point;
        }

        IEnumerator Follow()
        {
            while (true)
            {
                if (line == null) break;
                if (!UseLast)
                {
                    for (int i = 1; i < line.positionCount; ++i)
                    {
                        var prePoint = line.GetPosition(i - 1);
                        var curPoint = line.GetPosition(i);
                        var dis = (prePoint - curPoint);
                        var newPos = curPoint + dis * speed;
                        
                        line.SetPosition(i, newPos);
                    }
                }
                else
                {
                    for (int i = 1; i < line.positionCount - 1; ++i)
                    {
                        var prePoint = line.GetPosition(i - 1);
                        var curPoint = line.GetPosition(i);
                        var dis = (prePoint - curPoint);
                        var newPos = curPoint + dis * speed;
                        line.SetPosition(i, newPos);
                    }

                    for (int i = line.positionCount - 2; i > 0; --i)
                    {
                        var prePoint = line.GetPosition(i + 1);
                        var curPoint = line.GetPosition(i);
                        var dis = (prePoint - curPoint);
                        var newPos = curPoint + dis * speed;
                        line.SetPosition(i, newPos);
                    }
                }

                for (int i = UseLast ? line.positionCount - 2 : line.positionCount - 1; i > 0; --i)
                {
                    var curPoint = line.GetPosition(i);
                    var newPos = curPoint + force / 9.8f;
                    line.SetPosition(i, newPos);
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}