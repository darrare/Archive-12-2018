using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticleEffect : MonoBehaviour
{
    [SerializeField]
    GameObject particlePrefab;

    [SerializeField]
    float maxParticleCount = 300f;
    List<FireParticle> particles = new List<FireParticle>();

    PolygonCollider2D col;
    int pathIndex;
    int pointIndex;
    Vector2[] points;
    Vector2 spawnPoint;
    Vector2 spawnPointB;

    GameObject parentObject;
    FireParticle temp;

    // Use this for initialization
    void Start ()
    {
        parentObject = new GameObject();
        parentObject.transform.position = Vector3.zero;
        parentObject.name = "FireParticleEffectParent";

        col = GetComponent<PolygonCollider2D>();
        for (int i = 0; i < maxParticleCount; i++)
        {
            SpawnFireAlongOutline();
        }
        Destroy(temp.gameObject);
    }

    /// <summary>
    /// Spawns fire around the outline of the polygon collider
    /// </summary>
    void SpawnFireAlongOutline()
    {
        pathIndex = Random.Range(0, col.pathCount);
        points = col.GetPath(pathIndex);

        pointIndex = Random.Range(0, points.Length);

        spawnPoint = Vector2.Lerp(points[pointIndex], points[(pointIndex + 1) % points.Length], Random.Range(0f, 1f));

        temp = Instantiate(particlePrefab, spawnPoint + (Vector2)transform.position, Quaternion.identity).GetComponent<FireParticle>();
        temp.effector = this;
        temp.transform.SetParent(parentObject.transform, false);
    }

    /// <summary>
    /// Simply re-use an old particle system that has been disabled
    /// </summary>
    public void ReuseOldFireParticle(FireParticle particle)
    {
        pathIndex = Random.Range(0, col.pathCount);
        points = col.GetPath(pathIndex);

        pointIndex = Random.Range(0, points.Length);

        spawnPoint = Vector2.Lerp(points[pointIndex], points[(pointIndex + 1) % points.Length], Random.Range(0f, 1f));

        particle.transform.position = spawnPoint + (Vector2)transform.position;
    }
}
