using UnityEngine;

namespace CombatGame.Audio
{
    class CombatSFX : MonoBehaviour
    {
        [SerializeField] AudioClip swordAttack;
        [SerializeField] AudioClip swordImpact;
        [SerializeField] AudioClip swordUnsheath;
        [SerializeField] AudioSource audioSource;

        public void PlaySwordAttack()
        {
            audioSource.clip = swordAttack;
            audioSource.Play();
        }

        public void PlaySwordImpactHit()
        {
            audioSource.clip = swordImpact;
            audioSource.Play();
        }

        public void PlayUnsheathSword()
        {
            audioSource.clip = swordUnsheath;
            audioSource.Play();
        }
    }
}
