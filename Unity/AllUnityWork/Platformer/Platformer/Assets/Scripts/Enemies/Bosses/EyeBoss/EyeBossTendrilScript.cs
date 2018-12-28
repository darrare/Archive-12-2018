using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeBossTendrilScript : RecycledEffectScript
{
    float length;
    int pieceCount;
    float timer = 0;
    bool isRetracting = false;

    float gravityTimer = 0;
    bool isGravityIncreasing = true;

    List<EyeBossTendrilPieceScript> pieces = new List<EyeBossTendrilPieceScript>();
    HingeJoint2D joint;
    GameObject objectToAttachTo;

    /// <summary>
    /// Initializes the effect
    /// </summary>
    /// <param name="type">The type of effect this is (used for calling methods in EffectManager)</param>
    /// <param name="startPosition">The starting position of the object</param>
    /// <param name="startRotation">The starting rotation of the object</param>
    /// <param name="velocity">The initial velocity of the effect (private update methods will handle any weird changes)</param>
    /// <param name="rotationSpeed">A rotation speed if one is required. 0 otherwise</param>
    /// <param name="timeUntilDeletion">The time that this object should be alive. After this point, this object is hidden and added to the InactiveEffects list in EffectManager</param>
    /// <param name="timeUntilFadeStart">The time until this object starts to fade out, if this object has a fade to it.</param>
    public override RecycledEffectScript Initialize(EffectRecyclerType type, Vector3 startPosition, float startRotation, Vector3 velocity, float rotationSpeed, float timeUntilDeletion, float timeUntilFadeStart)
    {
        IsActive = true;
        gravityTimer = Random.Range(-Constants.EYEBALL_BOSS_TENDRIL_GRAVITY_LERP_DURATION, Constants.EYEBALL_BOSS_TENDRIL_GRAVITY_LERP_DURATION);
        Type = type;
        transform.position = startPosition;
        TimeUntilDeletion = timeUntilDeletion;
        timer = 0;
        isRetracting = false;
        pieces.Clear();

        return this;
    }

    /// <summary>
    /// We need this for this specific effect
    /// </summary>
    /// <param name="objectToAttachTo">The object we want to attach to</param>
    public void SecondInitialize(GameObject objectToAttachTo)
    {
        this.objectToAttachTo = objectToAttachTo;
        length = Vector2.Distance(transform.position, objectToAttachTo.transform.position);
        pieceCount = 2 + (int)(length / (EffectManager.Instance.EffectPrefabs[EffectRecyclerType.TendrilPiece].GetComponent<SpriteRenderer>().sprite.texture.width / EffectManager.Instance.EffectPrefabs[EffectRecyclerType.TendrilPiece].GetComponent<SpriteRenderer>().sprite.pixelsPerUnit));

        //TODO: Dynamically create the effect
        //Create the first joint
        pieces.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.TendrilPiece, transform.position, Random.Range(0, 360), Vector3.zero, 0, 0).GetComponent<EyeBossTendrilPieceScript>());
        joint = pieces[pieces.Count - 1].GetComponent<HingeJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = new Vector2(1, 0); //hard coded bullshit, depends on size of tendril
        joint.connectedAnchor = new Vector2(1, 0); //hard coded bullshit, depends on size of tendril

        //create the second joint (the custom one to attach to the eye)
        joint = pieces[pieces.Count - 1].gameObject.AddComponent<HingeJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = GetComponent<Rigidbody2D>();
        joint.anchor = new Vector2(-1, 0); //hard coded bullshit, depends on size of tendril
        joint.connectedAnchor = Random.insideUnitCircle * 2; //more hardcoded bullshit because that is apprioriate for the small eyes

        //All of the middle pieces. We have to treat the first and last individually
        for (int i = 1; i < pieceCount - 1; i++)
        {
            pieces.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.TendrilPiece, transform.position, Random.Range(0, 360), Vector3.zero, 0, 0).GetComponent<EyeBossTendrilPieceScript>());
            joint = pieces[pieces.Count - 1].GetComponent<HingeJoint2D>();
            joint.autoConfigureConnectedAnchor = false;
            pieces[pieces.Count - 2].GetComponent<HingeJoint2D>().connectedBody = joint.GetComponent<Rigidbody2D>();
            joint.anchor = new Vector2(-1, 0); //hard coded bullshit, depends on size of tendril
            joint.connectedAnchor = new Vector2(1, 0); //hard coded bullshit, depends on size of tendril
        }

        pieces.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.TendrilPiece, transform.position, Random.Range(0, 360), Vector3.zero, 0, 0).GetComponent<EyeBossTendrilPieceScript>());
        pieces[pieces.Count - 2].GetComponent<HingeJoint2D>().connectedBody = pieces[pieces.Count - 1].GetComponent<Rigidbody2D>();
        pieces[pieces.Count - 1].GetComponent<HingeJoint2D>().enabled = false;
        pieces[pieces.Count - 1].gameObject.AddComponent<SpringJoint2D>().anchor = Vector2.left;//hard coded bullshit, depends on size of tendril
        pieces[pieces.Count - 1].GetComponent<SpringJoint2D>().autoConfigureDistance = false;
        pieces[pieces.Count - 1].GetComponent<SpringJoint2D>().distance = 0;
        pieces[pieces.Count - 1].GetComponent<SpringJoint2D>().frequency = 15;
        pieces[pieces.Count - 1].GetComponent<SpringJoint2D>().connectedBody = objectToAttachTo.GetComponent<Rigidbody2D>();
        pieces[pieces.Count - 1].GetComponent<SpringJoint2D>().connectedAnchor = Utilities.GetRandomPositionInBoxCollider2D(objectToAttachTo.transform);
    }

    // Update is called once per frame
    void Update ()
    {
        if (IsActive)
        {
            if (isRetracting)
            {
                //Don't know why this is here, saw it a few days later?
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= TimeUntilDeletion)
                {
                    timer = 0;
                    StartCoroutine(RetractTendril());
                    isRetracting = true;
                }

                if ((isGravityIncreasing && (gravityTimer += Time.deltaTime) < Constants.EYEBALL_BOSS_TENDRIL_GRAVITY_LERP_DURATION)
                    || (!isGravityIncreasing && (gravityTimer -= Time.deltaTime) >= 0))
                {
                    foreach (EyeBossTendrilPieceScript e in pieces)
                    {
                        e.GetComponent<Rigidbody2D>().gravityScale = Mathf.Lerp(-Constants.EYEBALL_BOSS_TENDRIL_GRAVITY_MAX, Constants.EYEBALL_BOSS_TENDRIL_GRAVITY_MAX, gravityTimer);
                    }
                }
                else
                {
                    isGravityIncreasing = !isGravityIncreasing;
                }
            }
        }
	}

    /// <summary>
    /// retracts the tendril
    /// </summary>
    IEnumerator RetractTendril()
    {
        objectToAttachTo.GetComponent<TestBadWalls>().isBadWall = false;
        objectToAttachTo.GetComponent<SpriteRenderer>().color = Color.white;

        int index = pieces.Count - 1;
        for(;;)
        {
            pieces[index].EndEffect();
            index--;
            if (index == -1)
            {
                break;
            }
            yield return new WaitForSeconds(Constants.EYEBALL_BOSS_TENDRIL_RETRACT_DELAY);
        }
        EffectFinished();
    }


    /// <summary>
    /// Called when the effect is finished
    /// </summary>
    protected override void EffectFinished()
    {
        IsActive = false;
        EffectManager.Instance.EffectFinished(Type, this);
    }

    /// <summary>
    /// Ends the effect early
    /// </summary>
    public override void EndEffectEarly()
    {
        timer = 0;
        StartCoroutine(RetractTendril());
        isRetracting = true;
    }
}
