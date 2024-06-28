using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Door : MonoBehaviour
{
    public Vector2Int linksToRoom;

    public UnityEvent OnDoorEnter = new UnityEvent();
}
