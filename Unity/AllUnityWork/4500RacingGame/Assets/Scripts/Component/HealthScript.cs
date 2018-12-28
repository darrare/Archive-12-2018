using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthScript : NetworkBehaviour
{
    public const float maxHealth = 1;

    [SyncVar]
    public float curHealth = maxHealth;

    NetworkStartPosition[] spawnPoints;

    private void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public float HealthPercentage
    { get { return curHealth / maxHealth; } }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        UIManager.Instance.UpdateHealthBar(HealthPercentage);
    }

    public void TakeDamage(float amount)
    {
        if (!isServer)
            return;

        curHealth -= amount;
        if (curHealth <= 0)
        {
            curHealth = maxHealth;
            RpcRespawn();
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                transform.eulerAngles = new Vector3(0, 0, 90);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                Debug.LogError("There isn't any spawn points to spawn at.");
            }
        }
    }
}
