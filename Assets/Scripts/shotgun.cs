using UnityEngine;

public class shotgun : Weapon
{
    public GameObject firePoint1;
    public GameObject firePoint2;
    public GameObject firePoint3;
    public AudioSource shotgunAudio;

    public override void Fire()
    {
        shotgunAudio.Play();

        GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.transform.position, firePoint1.transform.rotation);
        bullet1.GetComponent<Rigidbody2D>().AddForce(-1 * firePoint1.transform.up * fireForce, ForceMode2D.Impulse);

        GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.transform.position, firePoint2.transform.rotation);
        bullet2.GetComponent<Rigidbody2D>().AddForce(-1 * firePoint2.transform.up * fireForce, ForceMode2D.Impulse);

        GameObject bullet3 = Instantiate(bulletPrefab, firePoint3.transform.position, firePoint3.transform.rotation);
        bullet3.GetComponent<Rigidbody2D>().AddForce(-1 * firePoint3.transform.up * fireForce, ForceMode2D.Impulse);
    }
    public override void FireBomb()
    {
       
    }
}
