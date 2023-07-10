using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private int _damage = -1;

    public int GetInfo() 
    {
        return _damage;
    }
}
