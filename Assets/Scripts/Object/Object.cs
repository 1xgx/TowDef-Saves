using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Object : MonoBehaviour
{
    [SerializeField] private ObjectType _type;

    public ObjectType Type => _type;
}
