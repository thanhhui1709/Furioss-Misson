using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MovementSequence")]
public class MovementSequence : ScriptableObject
{
    public List<IMovementPattern> sequences;
}
