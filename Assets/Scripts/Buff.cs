using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Projectile
{
    /*
     * buffType:
     * 0 = Speed
     * 1 = FireRate
     * 2 = Damage
     * 3 = Heal
     * */
    [SerializeField] int buffType;
     float buffDuration, buffPower;

    [SerializeField] string prefabPath;

    public string PrefabPath { get => prefabPath;}

    protected override void Start()
    {
        base.Start();
        buffDuration = BuffStats.duration.Value;

        switch(buffType)
        {
            case 0:
                buffPower = BuffStats.speed.Value;
                break;

            case 1:
                    buffPower = BuffStats.fireRate.Value;
                break;

            case 2:
                buffPower = BuffStats.damage.Value;
                break;

            case 3:
                buffPower = BuffStats.heal.Value;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().ActiveBuff(buffType, buffDuration, buffPower);
            Destroy(gameObject);
        }
    }
}
