using UnityEngine;

public class sniper : Weapon
{
    public GameObject firePoint;
    public AudioSource sniperAudio;

    public override void Fire()
    {
        sniperAudio.Play();

        GameObject bullet1 = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        bullet1.GetComponent<Rigidbody2D>().AddForce(-1 * firePoint.transform.up * fireForce, ForceMode2D.Impulse);
    }

    public override void FireBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, firePoint.transform.position, firePoint.transform.rotation);
        bomb.GetComponent<Rigidbody2D>().AddForce(-1 * firePoint.transform.up * fireForce * Random.Range(0.8f, 1.2f), ForceMode2D.Impulse);
    }

}
