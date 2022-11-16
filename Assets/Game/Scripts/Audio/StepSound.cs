using UnityEngine;

public class StepSound : MonoBehaviour
{
    [SerializeField] AudioClip[] _stepGrass;
    [SerializeField] AudioSource _stepSoundSource;

    public void PlayStep()
    {
        var clip = _stepGrass[Random.Range(0, _stepGrass.Length - 1)];
        _stepSoundSource.PlayOneShot(clip);
    }
}
