using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    public static float rotationSpeed
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    public static float fireCooldown
    {
        get { return GameManager.instance.playerId == 1 ? .9f : 1f; }
    }

    public static float Damage
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    public static int Amount
    {
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}
