using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{
    [SerializeField] GameObject[] asteroidPrefabs, enemyShipPrefab, buffPrefab;
    const float asteroidCooldown = 2, buffCooldown = 4, enemyShipCooldown = 5;
    float asteroidTimer, buffTimer, enemyShipTimer;
    Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.instance.masterClient)
        {
            photonView.RPC("spawAll", RpcTarget.AllBuffered);
        }
    }

    Vector2 GeneratePosition(GameObject objectSelected)
    {
        Vector2 spriteBounds = objectSelected.GetComponent<SpriteRenderer>().bounds.size / 2;

        Vector2 bounds = new Vector2(screenBounds.x - spriteBounds.x, screenBounds.y + spriteBounds.y);

        Debug.Log(spriteBounds);
        Debug.Log(bounds);
        return new Vector2(Random.Range(-bounds.x, bounds.x), bounds.y);
    }

    GameObject ChoosenAsteroid()
    {
        int asteroidChoosed = Random.Range(0, asteroidPrefabs.Length);

        Debug.Log(asteroidChoosed);

        return asteroidPrefabs[asteroidChoosed];
    }

    GameObject ChoosenBuff()
    {
        int buffChoosed = Random.Range(0, buffPrefab.Length);

        return buffPrefab[buffChoosed];
    }

    GameObject ChoosenEnemyShip()
    {
        int enemyShipChoosed = Random.Range(0, enemyShipPrefab.Length);

        return enemyShipPrefab[enemyShipChoosed];
    }

    void SpawnAsteroid()
    {
        asteroidTimer -= Time.deltaTime;
        //Debug.Log(Time.deltaTime);

        if(asteroidTimer <= 0)
        {
            GameObject asteroidSelected = ChoosenAsteroid();
            asteroidTimer = asteroidCooldown;
            Instantiate(asteroidSelected, GeneratePosition(asteroidSelected), Quaternion.identity);
        }
    }

    void SpawnBuff()
    {
        buffTimer -= Time.deltaTime;

        if(buffTimer <= 0)
        {
            buffTimer = buffCooldown;
            GameObject buffSelected = ChoosenBuff();
            Instantiate(buffSelected, GeneratePosition(buffSelected), Quaternion.identity);

        }
    }

    void SpawnEnemyShip()
    {
        enemyShipTimer -= Time.deltaTime;

        if(enemyShipTimer <= 0)
        {
            GameObject enemyShipSelected = ChoosenEnemyShip();

            if(enemyShipSelected.GetComponent<EnemyShip>().ScoreNeeded > GameManager.instance.Score)
            {
                return;
            }

            enemyShipTimer = enemyShipCooldown;
          Instantiate(enemyShipSelected, GeneratePosition(enemyShipSelected), Quaternion.identity);
        }
    }

    [PunRPC]
    void spawAll()
    { 
            SpawnAsteroid();
            SpawnBuff();
            SpawnEnemyShip();
    }
}
