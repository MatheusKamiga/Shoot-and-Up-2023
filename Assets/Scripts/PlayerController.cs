using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerHUD))]


public class PlayerController : SpaceShip
{
    bool firing;
    float damageTimer, fireRateTimer, speedTimer;
    bool damageBuff, fireRateBuff, speedBuff;

    [SerializeField] Transform firePointLeft, firePointRight;

    
    PlayerHUD hud;
    InputSystem inputSystem;
    Joystick joystick;

    

    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        
        inputSystem = new InputSystem();

        inputSystem.Player.Move.performed += ctx => direction = ctx.ReadValue<Vector2>();
        inputSystem.Player.Fire.started += ctx => firing = true;
        inputSystem.Player.Fire.canceled += ctx => firing = false;
        inputSystem.Player.Turbo.started += ctx => speed *= 2;
        inputSystem.Player.Turbo.canceled += ctx => speed /= 2;
       
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = PlayerStats.health.Value;
        base.Start();
        hud = GetComponent<PlayerHUD>();

        damage = PlayerStats.damage.Value;
        speed = PlayerStats.speed.Value;
        fireRate = PlayerStats.fireRate.Value;

        hud.UpdateHealthBar(health, maxHealth);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        SpeedBuff();
        DamageBuff();
        FireRateBuff();


        //direction = joystick.Direction;
    }

    private void LateUpdate()
    {
        FitSpaceShip();
    }

    void FitSpaceShip()
    {
        Vector2 viewPosition = transform.position;

        viewPosition.x = Mathf.Clamp(viewPosition.x, -screenBounds.x + spriteBounds.x, screenBounds.x - spriteBounds.x);
        viewPosition.y = Mathf.Clamp(viewPosition.y, -screenBounds.y + spriteBounds.y, screenBounds.y - spriteBounds.y);

        transform.position = viewPosition;
    }

    protected override void Fire()
    {
        
            if (Time.time > timer + 1 / fireRate)
            {
                Instantiate(projectilePrefab, firePointLeft.position, Quaternion.identity).GetComponent<Projectile>().Damage = damage;
                Instantiate(projectilePrefab, firePointRight.position, Quaternion.identity).GetComponent<Projectile>().Damage = damage;
                timer = Time.time;
            }
        
    }

    void SpeedBuff()
    {
        if(speedBuff)
        {
            speedTimer -= Time.deltaTime;
            hud.UpdateSpeedBar(speedTimer);
            if(speedTimer <= 0)
            {
                speed = PlayerStats.speed.Value;
                speedBuff = false;
            }
        }
    }

    void FireRateBuff()
    {
        if (fireRateBuff)
        {
            fireRateTimer -= Time.deltaTime;
            hud.UpdateFireRateBar(fireRateTimer);
            if (fireRateTimer <= 0)
            {
                fireRate = PlayerStats.fireRate.Value;
                fireRateBuff = false;
            }
        }
    }

    void DamageBuff()
    {
        if (damageBuff)
        {
            damageTimer -= Time.deltaTime;
            hud.UpdateDamageBar(damageTimer);
            if (damageTimer <= 0)
            {
                damage = PlayerStats.damage.Value;
                damageBuff = false;
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        hud.UpdateHealthBar(health, maxHealth);
    }

    protected override void Destroy()
    {
        base.Destroy();
        GameManager.instance.GameOver();
    }

    public void ActiveBuff(int buffType, float buffDuration, float buffPower)
    {
       

        switch(buffType)
        {
            case 0:
                speedBuff = true;
                speed *= buffPower;
                speedTimer = buffDuration;
                hud.SetSpeedDuration(buffDuration);
                break;

                case 1:
                fireRateBuff = true;
                fireRate *= buffPower;
                fireRateTimer = buffDuration;
                hud.SetFireRateDuration(buffDuration);
                break;

                case 2:
                damageBuff = true;
                damage *= buffPower;
                damageTimer = buffDuration;
                hud.SetDamageDuration(buffDuration);
                break;

                case 3:
                health += buffPower;
                if(health > maxHealth)
                {
                    health = maxHealth;
                }
                break;

                default: 
                break;
        }
    }

    bool controllerOn = true;
    [PunRPC]
    private void Initialize()
    {
        if (!photonView.IsMine)
        {
            Color color = Color.white;
            color.a = 0.1f;
            GetComponent<SpriteRenderer>().color = color;
            rigidbody2D.isKinematic = true;
            controllerOn = false;
        }
    }

}
