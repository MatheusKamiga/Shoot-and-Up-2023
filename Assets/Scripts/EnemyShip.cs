using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyShip : SpaceShip
{
    [SerializeField] int score, scoreNeeded;
    float destination;
    HUD hud;

    public int ScoreNeeded { get => scoreNeeded; }

    Transform player;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        destination = Random.Range(screenBounds.y - 3, screenBounds.y -1);
        direction = Vector2.down;
        hud = GetComponentInChildren<HUD>();
    }

    protected override void Update()
    {
        base.Update();

        
        photonView.RPC("CheckDestination", RpcTarget.All);
        photonView.RPC("CheckBounds", RpcTarget.All);
        photonView.RPC("LookAt", RpcTarget.All);
        
        
    }
    [PunRPC]
    private void CheckDestination()
    {
        if (direction.x == 0 && transform.position.y < destination)
        {
            direction = Vector2.right;
        }
    }

    [PunRPC]
    private void CheckBounds()
    {
        if (transform.position.x > screenBounds.x)
        {
            direction = Vector2.left;
        }else if(transform.position.x < -screenBounds.x)
        {
            direction = Vector2.right;
        }
    }

    [PunRPC]
    private void LookAt()
    {
        spriteRender.transform.up = player.position - transform.position;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        hud.UpdateHealthBar(health, maxHealth);
    }

    protected override void Destroy()
    {
        base.Destroy();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null && !collision.CompareTag(transform.tag))
        {
            damageable.TakeDamage(damage);
            Destroy();
        }
    }
}
