using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public float fireForce;

    public abstract void Fire();

    public abstract void FireBomb();
}