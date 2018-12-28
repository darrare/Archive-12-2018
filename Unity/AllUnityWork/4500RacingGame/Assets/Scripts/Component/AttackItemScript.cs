using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class AttackItemScript : NetworkBehaviour
{
    public abstract void Initialize(Vector2 position, Vector2 direction);
}
