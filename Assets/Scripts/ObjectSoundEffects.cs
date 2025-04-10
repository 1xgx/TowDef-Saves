using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ObjectSoundEffects
{
    [SerializeField] private ObjectType _type;
    [SerializeField] private AudioClip _clip;

    public ObjectType Type => _type;
    public AudioClip Clip => _clip;
}
