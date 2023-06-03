using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossJaeger : MonoBehaviour
{
    public Rigidbody2D rb;
    public Weapon weapon1;
    public Weapon weapon2;
    public GameObject weapons;
    public BossHealth health;
    public GameObject attack;

    private GameObject wolf;
    private Animator animator;
    private CircleCollider2D attackCollider;
    private bool flipped = false;

    private float isShooting = 0f;

    private int biteLayer;
    private int attackInterval = 10;

    public bool active = false;

    void Start()
    {
        wolf = GameObject.Find("Wolf");
        animator = GetComponent<Animator>();
        biteLayer = LayerMask.NameToLayer("bite");
        attackCollider = attack.GetComponent<CircleCollider2D>();
        attackCollider.enabled = false;
        InvokeRepeating("chooseAttack", 1, attackInterval);
    }

    void chooseAttack()
    {
        int rand = UnityEngine.Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                StartCoroutine(SpinAttackCoroutine()); break;
            case 1:
                StartCoroutine(DashAttackCoroutine()); break;
            case 2:
                StartCoroutine(CannonAttackCoroutine()); break;
        }

    }

    IEnumerator CannonAttackCoroutine()
    {
        // TODO: balancing - adjust 'bombCount' value
        int numBombs = 4;

        float duration = (float)(attackInterval - 1) / (float)numBombs;
        rb.rotation = flipped ? 180f : 0f;

        int bombCount = 0;
        while (bombCount < numBombs)
        {
            float startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                weapons.transform.RotateAround(weapons.transform.position, new Vector3(0, 0, 1), Time.deltaTime * 100f);

                yield return null;
            }

            weapon1.FireBomb();
            weapon2.FireBomb();
            animator.SetInteger("attack", 2);

            bombCount++;
        }
    }

    IEnumerator DashAttackCoroutine()
    {
        // TODO: balancing - adjust 'dashSpeed' and 'numDashes' values
        float dashSpeed = 25;
        int numDashes = 6;

        float dashDuration = (float)(attackInterval - 1) / (float)numDashes;

        int dashCount = 0;
        while (dashCount < numDashes)
        {
            Vector2 wolfPosition = wolf.transform.position;
            Vector2 aimDirection = wolfPosition - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            rb.rotation = aimAngle;

            // flip sprite depending on current walk direction
            if ((flipped && aimDirection.x > 0) || (!flipped && aimDirection.x < 0))
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
                flipped = !flipped;
            }

            float startTime = Time.time;
            attackCollider.enabled = true;
            animator.SetBool("isWalk", true);

            while (Time.time - startTime < dashDuration)
            {
                Vector2 newPosition = Vector2.MoveTowards(rb.position, wolfPosition, dashSpeed * Time.deltaTime);

                if (Vector2.Distance(newPosition, rb.position) <= 0.001f)
                {
                    animator.SetBool("isWalk", false);
                    attackCollider.enabled = false;
                }

                rb.position = newPosition;
                yield return null;
            }

            animator.SetBool("isWalk", false);
            attackCollider.enabled = false;

            dashCount++;
        }
    }

    IEnumerator SpinAttackCoroutine()
    {
        // TODO: balancing - adjust 'shootInterval' and 'rotationSpeed' values
        float shootInterval = 0.2f;
        float rotationSpeed = 100f;

        float duration = 8f;
        float startTime = Time.time;

        rb.rotation = flipped ? 180f : 0f;
        animator.SetInteger("attack", 1);

        while (Time.time - startTime < duration)
        {
            if (isShooting > shootInterval)
            {
                weapon1.Fire();
                weapon2.Fire();
                isShooting = 0;
            }

            isShooting += Time.deltaTime;
            weapons.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        animator.SetInteger("attack", -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == biteLayer)
        {
            health.health--;

            if (health.health == 0)
            {
                animator.SetBool("isDead", true);
            }
        }
    }

    private void DeadAnimationEnd()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void ThrowAnimationEnd()
    {
        animator.SetInteger("attack", -1);
    }
}