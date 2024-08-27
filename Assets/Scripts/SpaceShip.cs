using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class SpaceShip : MonoBehaviourPun, IDamageable
{
    [SerializeField] protected float speed, fireRate, maxHealth, damage;
    protected float health, timer;
    protected Vector2 direction, screenBounds, spriteBounds;

    protected Rigidbody2D rigidbody2D;
    protected SpriteRenderer spriteRender;
    [SerializeField] protected GameObject projectilePrefab;

    Transform firePoint;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();

        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        spriteBounds = spriteRender.sprite.bounds.size / 2;

        health = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Movement();
        Fire();
    }

    

    void Movement()
    {
        /*direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");*/


        rigidbody2D.velocity = direction * speed;
    }

    

    protected virtual void Fire()
    {

            if (Time.time > timer + 1 / fireRate)
            {
                Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<Projectile>().Damage = damage;
                timer = Time.time;
            }
        
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy();
        }
    }

    protected virtual void Destroy()
    {

    }
    
}
