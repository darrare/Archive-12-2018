using UnityEngine;
using System.Collections;

public class TutorialDummy : DamagableObjectScript 
{

	public delegate void TutorialEvent();
	public static event TutorialEvent PlayerMoved;
	public static event TutorialEvent BasicAttack;
	public static event TutorialEvent SecondaryAttack;
	public static event TutorialEvent SpecialAttack;
	public static event TutorialEvent Booster;

	GameObject character;
	bool movedLeft, movedRight, jumped;

	GameObject dummyTwo;

	// Use this for initialization
    protected void Start()
    {
        maxHealth = 100000;
        hitSound = GameManager.Instance.GameSounds[Constants.ARROW_HIT_SND];
		character = GameObject.Find ("RangerPlayer(Clone)");
		InvokeRepeating ("ManualUpdate", 1, .01f);
		dummyTwo = GameObject.Find ("DummyTwo");
		dummyTwo.SetActive (false);
        base.Initialize();
	}

	void ManualUpdate()
	{
		if (GameObject.Find ("TutorialControl").GetComponent<TutorialRanger> ().GetStage () == 0) {
			if (character.GetComponent<Rigidbody2D> ().velocity.x > 0) {
				movedRight = true;
			} else if (character.GetComponent<Rigidbody2D> ().velocity.x < 0) {
				movedLeft = true;
			}
			if (character.GetComponent<Rigidbody2D> ().velocity.y < -.1f) {
				jumped = true;
			}
			
			if (movedRight && movedLeft && jumped) {
				PlayerMoved();
				CancelInvoke ("MovementCheck");
			}
		}

		if (GameObject.Find ("TutorialControl").GetComponent<TutorialRanger> ().GetStage () == 3) {
			dummyTwo.SetActive (true);
		}

		if (GameObject.Find ("TutorialControl").GetComponent<TutorialRanger> ().GetStage () == 4) 
        {
            if (GameManager.Instance.CurrentSave.Inputs[InputType.Special].Pressed)
            { Booster(); }
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
        try
        {
            if (collider.gameObject.name == "Arrow(Clone)")
            {
                BasicAttack();
            }
            else if (collider.gameObject.name == "ExpArrow(Clone)")
            {
                SecondaryAttack();
            }
            else if (collider.gameObject.name == "PierceArrow(Clone)")
            {
                SpecialAttack();
                Invoke("RemoveDummyTwo", 2);
            }
        }
        catch (System.NullReferenceException) { }
	}

	void RemoveDummyTwo()
	{
		dummyTwo.SetActive (false);
	}

    protected override void Death()
    { }
}
