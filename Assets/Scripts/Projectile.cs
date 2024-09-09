using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviourPun
{

    [SerializeField] float speed, damage;


    Rigidbody2D rigidbody2D;

    public float Damage
    {
        get => damage; 
        
        set
        {
            if (damage == 0)
            {
                damage = value;
            }
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        rigidbody2D.velocity = transform.up * speed;
    }

    protected virtual void Destroy()
    {
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        speed = 0;
        Destroy(gameObject, particleSystem.startLifetime);
    }

    [PunRPC]
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!photonView.IsMine)
        { 
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null && !collision.CompareTag(transform.tag))
        {
            damageable.TakeDamage(damage);
            Destroy();
        }
        }
    }
}
