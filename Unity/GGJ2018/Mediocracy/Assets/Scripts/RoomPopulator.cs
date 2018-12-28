using System.Collections.Generic;
using AUTools.Unity;
using UnityEngine;

public class RoomPopulator : MonoBehaviour
{
    [SerializeField] GameObject bedObj;
    [SerializeField] GameObject paintingObj;
    [SerializeField] GameObject deskObj;
    [SerializeField] GameObject doorObj;
    [SerializeField] GameObject[] people;

    public List<GameObject> RoomSpawnedObjects { get; private set; }

    public List<GameObject> Transposers { get; private set; }

    List<TransposerMinigame> minigames;

    public GameObject Person { get; private set; }

    // Use this for initialization
    void Start()
    {
        RoomSpawnedObjects = new List<GameObject>();
        GenerateRoomConfiguration();
    }

    public void GenerateRoomConfiguration()
    {
        Transposers = new List<GameObject>();
        if (RoomSpawnedObjects == null)
        {
            RoomSpawnedObjects = new List<GameObject>();
        }
        foreach (GameObject obj in RoomSpawnedObjects)
        {
            Destroy(obj);
        }
        RoomSpawnedObjects.Clear();
        List<GameObject> spawnAnchors = new List<GameObject>();
        List<GameObject> decorationAnchors = new List<GameObject>();

        //Anchors
        Transform bedAnchors = transform.Find("Bed_Spawn_Areas");
        for (int i = -4; i <= 5; i += 8)
        {
            Vector3 newPos = bedAnchors.position;
            newPos.x += i;
            GameObject temp = Instantiate(new GameObject(), newPos, Quaternion.identity);
            temp.transform.parent = bedAnchors;
            spawnAnchors.Add(temp);
        }

        Transform decAnchors = transform.Find("Decoration_Anchors");
        for (int i = -8; i < 8; i += 1)
        {
            Vector3 newPos = decAnchors.position;
            newPos.x += i;
            GameObject temp = Instantiate(new GameObject(), newPos, Quaternion.identity);
            temp.transform.parent = decAnchors;
            decorationAnchors.Add(temp);
        }

        //People
        Person = Instantiate(people.GetRandom(), decorationAnchors.GetRandom().transform.position + Vector3.forward, Quaternion.identity);
        Person.transform.parent = transform;

        //Bed
        GameObject bed = null;
        if (Random.Range(0f, 1f) > .5f)
        {
            int ind = Random.Range(0, spawnAnchors.Count);
            bed = Instantiate(bedObj, spawnAnchors[ind].transform, false);
            if (transform.position.x - bed.transform.position.x < 0)
            {
                bed.transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = true;
            }
            spawnAnchors.RemoveAt(ind);
            RoomSpawnedObjects.Add(bed);
        }

        for (int i = 0; i < decorationAnchors.Count; i++)
        {
            if (decorationAnchors[i].transform != null && bed != null && Vector3.Distance(bed.transform.position, decorationAnchors[i].transform.position) < 4f)
            {
                Destroy(decorationAnchors[i]);
                decorationAnchors.RemoveAt(i);
                i--;
            }
            else if (decorationAnchors[i] == null)
            {
                decorationAnchors.RemoveAt(i);
                i--;
            }
        }

        //Decorations
        List<int> exVals = new List<int>();
        for (int i = 0; i < 1; i++)
        {
            int ind = Random.Range(0, decorationAnchors.Count);
            GameObject dec = null;
            if (decorationAnchors.Count > 0)
            {
                dec = Instantiate(deskObj, decorationAnchors[ind].transform, false);
                dec.GetComponent<SpriteSpecifier>().SetSpriteRandomly(exVals);
                exVals.Add(dec.GetComponent<SpriteSpecifier>().curVal);
                decorationAnchors.RemoveAt(ind);
                RoomSpawnedObjects.Add(dec);
                for (int j = 0; j < decorationAnchors.Count; j++)
                {
                    if (decorationAnchors[j].transform != null && Vector3.Distance(dec.transform.position, decorationAnchors[j].transform.position) < 4f)
                    {
                        Destroy(decorationAnchors[j]);
                        decorationAnchors.RemoveAt(j);
                        j--;
                    }
                    else if (decorationAnchors[j] == null)
                    {
                        decorationAnchors.RemoveAt(j);
                        j--;
                    }
                }
            }
            if (dec.GetComponent<SpriteSpecifier>().CheckIfTransposer())
            {
                Transposers.Add(dec);
            }
        }

        //Paintings
        //if (Random.Range(0f, 1f) > .5f || true)
        //{
        SpawnArea paintingSpawnLocation = transform.Find("Painting_Spawn_Area").GetComponent<SpawnArea>();
        Vector3 spawnLocation = paintingSpawnLocation.transform.position;
        if (bed == null)
        {
            spawnLocation.x = decorationAnchors[Random.Range(0, decorationAnchors.Count)].transform.position.x;
        }
        else
        {
            spawnLocation.x = bed.transform.position.x;
        }
        spawnLocation.y += paintingSpawnLocation.bounds.y * .5f * Mathf.Sign(Random.Range(-1f, 1f));
        GameObject spawnedPainting = Instantiate(paintingObj, spawnLocation, Quaternion.identity);
        spawnedPainting.transform.parent = transform.Find("Painting_Spawn_Area");

        if (spawnedPainting.GetComponent<PaintingSelector>().CheckIfRouter())
        {
            Transposers.Add(spawnedPainting);
        }
        else if (Transposers.Count == 0)
        {
            spawnedPainting.GetComponent<PaintingSelector>().ChangeSprite(2);
            Transposers.Add(spawnedPainting);
        }

        RoomSpawnedObjects.Add(spawnedPainting);
        for (int j = 0; j < decorationAnchors.Count; j++)
        {
            if (decorationAnchors[j].transform != null && spawnedPainting.transform.position.x - decorationAnchors[j].transform.position.x < 6f)
            {
                Destroy(decorationAnchors[j]);
                decorationAnchors.RemoveAt(j);
                j--;
            }
            else if (decorationAnchors[j] == null)
            {
                decorationAnchors.RemoveAt(j);
                j--;
            }
        }

        //}

        //Doors
        if (decorationAnchors.Count > 0)
        {
            int val = Random.Range(0, decorationAnchors.Count);
            GameObject door = Instantiate(doorObj, decorationAnchors[val].transform.position, Quaternion.identity);
            door.transform.parent = decorationAnchors[val].transform;
            decorationAnchors.RemoveAt(val);
            RoomSpawnedObjects.Add(door);
        }

        foreach (GameObject obj in decorationAnchors)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in spawnAnchors)
        {
            Destroy(obj);
        }

        minigames = new List<TransposerMinigame>();
        foreach (GameObject obj in Transposers)
        {
            minigames.Add(obj.AddComponent<TransposerMinigame>());
        }
    }
}