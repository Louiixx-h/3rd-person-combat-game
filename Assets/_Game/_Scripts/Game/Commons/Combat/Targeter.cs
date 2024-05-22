using Cinemachine;
using System.Collections.Generic;   
using UnityEngine;

namespace CombatGame.Commons.Combat
{
    public class Targeter : MonoBehaviour
    {
        [HideInInspector] public Target CurrentTarget { get; private set; }
        [SerializeField] private CinemachineTargetGroup _cineTargetGroup;

        private List<Target> _targets = new List<Target>();
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out Target target)) return;

            _targets.Add(target);
            target.DestroyEvent += RemoveTarget;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out Target target)) return;

            RemoveTarget(target);
        }

        public bool SelectTarget()
        {
            if (_targets.Count == 0) return false;

            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach (var target in _targets)
            {
                Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);

                if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
                {
                    continue;
                }

                Vector3 toCenter = viewPos - new Vector2(0.5f, 0.5f);
                if (toCenter.sqrMagnitude < closestTargetDistance)
                {
                    closestTarget = target;
                    closestTargetDistance = toCenter.sqrMagnitude;
                }
            }

            if (closestTarget == null) return false;

            CurrentTarget = closestTarget;
            _cineTargetGroup.AddMember(CurrentTarget.transform, 1, 2);
            return true;
        }

        public void Cancel()
        {
            if (CurrentTarget == null) return;

            _cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        void RemoveTarget(Target target)
        {
            if (CurrentTarget == target)
            {
                _cineTargetGroup.RemoveMember(target.transform);
                CurrentTarget = null;
            }

            target.DestroyEvent -= RemoveTarget;
            _targets.Remove(target);
        }
    }
}