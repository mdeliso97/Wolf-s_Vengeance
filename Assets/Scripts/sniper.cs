using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sniper : Weapon
{

    public Transform firePoint;

    public override void Fire()
    {
        GameObject bullet1 = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet1.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

    public override void FireBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, firePoint.rotation);
        bomb.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

}
