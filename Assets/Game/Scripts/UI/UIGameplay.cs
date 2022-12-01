using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class UIGameplay : MonoBehaviour
    {
        [SerializeField] private GameObject _imageTarget;
        [SerializeField] private Image _imageLife;

        [SerializeField] private float _maxLife = 0;
        [SerializeField] private float _currentLife = 0;
        private bool _isSelected = false;
 
        private void Update()
        {
            Spin();
        }
        public UIGameplay HealthManager(float value)
        {
            _currentLife += value;
            if (_currentLife <= 0) return this;
            _imageLife.fillAmount = _currentLife / _maxLife;
            return this;
        }

        public UIGameplay SetMaxLife(float value)
        {
            _maxLife = value;
            return this;
        }

        public UIGameplay SetPositionTarget(Vector3 targetPosition)
        {
            _imageTarget.transform.position = targetPosition;
            return this;
        }

        public UIGameplay SelectTarget()
        {
            _isSelected = true;
            _imageTarget.SetActive(true);
            return this;
        }

        public UIGameplay CancelTarget()
        {
            _isSelected = false;
            _imageTarget.SetActive(false);
            return this;
        }

        private void Spin()
        {
            if (!_isSelected) return;
            _imageTarget.transform.Rotate(0, 0, 180 * Time.deltaTime);
        }
    }
}