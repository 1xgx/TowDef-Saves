using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundEffectsSc : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ObjectSoundEffects[] _objectSoundEffects;
    private ObjectType _currentObject;
    
    public void Play(ObjectType type)
    {
        SetObjectSound(type);
        _audioSource.Play();
    }
    private void SetObjectSound(ObjectType type)
    {
        _currentObject = type;
        _audioSource.clip = _objectSoundEffects.First(sound => sound.Type == _currentObject).Clip;
    }
}
