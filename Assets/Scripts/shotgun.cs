using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgun : Weapon
{
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public GameObject GunshotSound;
    public AudioSource shotgunAudio;
    //public AudioSource gunshot;

    void Start()
    {
        //gunshot = gameObject.AddComponent<AudioSource>();
        //gunshot.clip = Resources.Load<AudioClip>("Assets/Sound Effects/Gunshot - Sound Effect.mp3");
        shotgunAudio = GameObject.Find("ShotgunSound").GetComponent<AudioSource>();
    }

    public override void Fire()
    {
        shotgunAudio.Play();

        GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
        bullet1.GetComponent<Rigidbody2D>().AddForce(firePoint1.up * fireForce, ForceMode2D.Impulse);

        GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
        bullet2.GetComponent<Rigidbody2D>().AddForce(firePoint2.up * fireForce, ForceMode2D.Impulse);

        GameObject bullet3 = Instantiate(bulletPrefab, firePoint3.position, firePoint3.rotation);
        bullet3.GetComponent<Rigidbody2D>().AddForce(firePoint3.up * fireForce, ForceMode2D.Impulse);
    }
    public override void FireBomb()
    {
       
    }
}
