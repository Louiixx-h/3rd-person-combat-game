using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class UIGameplay : MonoBehaviour
    {
        [SerializeField] private Image _imageLife;

        [SerializeField] private float _maxLife = 0;
        [SerializeField] private float _currentLife = 0;

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
    }
}