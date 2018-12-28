using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EyeBossControllerScript : MonoBehaviour
{

    enum EyeBossState { None, Idle, Telegraph, Attacking }
    enum EyeBossAttacks { Spawn, DisablePlatform, FireShots, EnergyBeam, RotatingBeams, DisablePlatformsAndSpawnMinions, DisablePlatformsAndShootExplosives, ShootExplosivesAndSpawnMinions }
    enum EyeBossPhase { None, PhaseOneIntro, PhaseOne, PhaseTwoIntro, PhaseTwo };
    EyeBossPhase phase = EyeBossPhase.None;

    delegate void EyeBehaviour(Transform t);
    delegate void ControllerBehaviour();
    ControllerBehaviour behaviour;

    [SerializeField]
    GameObject[] eyeGameobjects;

    [SerializeField]
    GameObject[] platforms;

    Eye[] eyes = new Eye[4];
    Eye mainEye;

    float timer = 0;


    /// <summary>
    /// Initialize the controller
    /// </summary>
    void Start ()
    {
        //Set up the eyes
		for (int i = 0; i < eyeGameobjects.Length; i++)
        {
            eyes[i] = new Eye(eyeGameobjects[i].transform, this);
        }
        mainEye = new Eye(transform, this);
        ChangeControllerState(EyeBossPhase.None);
	}
	
	/// <summary>
    /// Updates the controller
    /// </summary>
	void Update ()
    {
        behaviour();
    }

    /// <summary>
    /// Change the behaviour of the controller
    /// </summary>
    /// <param name="phase">What type of action we want to do</param>
    void ChangeControllerState(EyeBossPhase phase)
    {
        switch (phase)
        {
            case EyeBossPhase.None:
                behaviour = delegate ()
                {
                    if (Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) < 20)
                    {
                        ChangeControllerState(EyeBossPhase.PhaseOneIntro);
                    }
                };
                break;
            case EyeBossPhase.PhaseOneIntro:
                behaviour = delegate ()
                {
                    timer += Time.deltaTime;
                    foreach (Eye e in eyes)
                    {
                        e.transform.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.clear, Color.white, timer / Constants.EYEBALL_BOSS_TIME_FOR_PHASE_TRANSITION);
                    }
                    if (timer >= Constants.EYEBALL_BOSS_TIME_FOR_PHASE_TRANSITION)
                    {
                        ChangeControllerState(EyeBossPhase.PhaseOne);
                        timer = 0;
                        TellEyesToChangeState(EyeBossState.Telegraph);
                    }
                };
                break;
            case EyeBossPhase.PhaseOne:
                behaviour = delegate ()
                {
                    timer += Time.deltaTime;
                    if (eyes.Where(t => !t.Update()).Count() == 0) //if they all return dead
                    {
                        ChangeControllerState(EyeBossPhase.PhaseTwoIntro);
                        timer = 0;
                    }
                    else if (timer >= Constants.EYEBALL_BOSS_TELEGRAPH_DURATION + Constants.EYEBALL_BOSS_ATTACKING_DURATION)
                    {
                        timer = 0;
                        TellEyesToChangeState(EyeBossState.Telegraph);
                    }
                };
                break;
            case EyeBossPhase.PhaseTwoIntro:
                behaviour = delegate ()
                {
                    timer += Time.deltaTime;
                    GetComponent<SpriteRenderer>().color = Color.Lerp(Color.clear, Color.white, timer / Constants.EYEBALL_BOSS_TIME_FOR_PHASE_TRANSITION);
                    if (timer >= Constants.EYEBALL_BOSS_TIME_FOR_PHASE_TRANSITION)
                    {
                        ChangeControllerState(EyeBossPhase.PhaseTwo);
                        timer = 0;
                        //Do something maybe
                    }
                };

                break;
            case EyeBossPhase.PhaseTwo:
                behaviour = delegate ()
                {
                    timer += Time.deltaTime;
                    if (timer >= Constants.EYEBALL_BOSS_TELEGRAPH_DURATION + Constants.EYEBALL_BOSS_ATTACKING_DURATION)
                    {
                        timer = 0;
                        TellEyeToChangeState(EyeBossState.Telegraph);
                    }
                };
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Change the state of all of the eye bosses
    /// DONT ASSIGN ATTACK STATE HERE
    /// </summary>
    /// <param name="state">The state we want to change the eyes to</param>
    void TellEyesToChangeState(EyeBossState state)
    {
        switch(state)
        {
            case EyeBossState.Idle:
                foreach (GameObject g in platforms) //Restores all of the effected walls to their default state
                {
                    g.GetComponent<SpriteRenderer>().color = Color.white;
                    g.GetComponent<TestBadWalls>().isBadWall = false;
                }
                foreach (Eye e in eyes.Where(t => !t.isDead)) //Changes the state of all of the eyes
                {
                    e.ChangeBehaviour(state);
                }
                break;
            case EyeBossState.Telegraph:
                foreach (Eye e in eyes.Where(t => !t.isDead)) //Changes the state of all of the eyes
                {
                    e.ChangeBehaviour(state, (EyeBossAttacks)Random.Range(0, 4)); //Selects a random attack to give to the eye
                    //e.ChangeBehaviour(state, EyeBossAttacks.DisablePlatform);
                }
                break;
            default:
                foreach (Eye e in eyes.Where(t => !t.isDead)) //Changes the state of all of the eyes
                {
                    e.ChangeBehaviour(state);
                }
                break;
        }
    }

    /// <summary>
    /// Change the state of all of the eye bosses
    /// DONT ASSIGN ATTACK STATE HERE
    /// </summary>
    /// <param name="state">The state you want the eye to change to</param>
    void TellEyeToChangeState(EyeBossState state)
    {
        switch (state)
        {
            case EyeBossState.None:
                foreach (GameObject g in platforms) //Restores all of the effected walls to their default state
                {
                    g.GetComponent<SpriteRenderer>().color = Color.white;
                    g.GetComponent<TestBadWalls>().isBadWall = false;
                }
                mainEye.ChangeBehaviour(state);
                break;
            case EyeBossState.Idle:
                foreach (GameObject g in platforms) //Restores all of the effected walls to their default state
                {
                    g.GetComponent<SpriteRenderer>().color = Color.white;
                    g.GetComponent<TestBadWalls>().isBadWall = false;
                }
                mainEye.ChangeBehaviour(state);
                break;
            case EyeBossState.Telegraph:
                mainEye.ChangeBehaviour(state, (EyeBossAttacks)Random.Range(4, 8));
                break;
            case EyeBossState.Attacking:
                Debug.Log("Don't tell this method to do the attack state");
                break;
            default:
                mainEye.ChangeBehaviour(state);
                break;
        }
    }

    /// <summary>
    /// Picks a random platform for an eye to disable
    /// TODO: eventually, this should only select platforms that arent already selected
    /// </summary>
    /// <returns>The platform to disable temperarily</returns>
    public GameObject GetRandomPlatformToDisable()
    {
        return platforms[Random.Range(0, platforms.Length)];
    }

    /// <summary>
    /// One of the eyes has been destroyed
    /// </summary>
    public void DestroyEye(int i)
    {
        eyes[i].SetDisabled();
    }


    /// <summary>
    /// A class that is used for controlling the four eyes
    /// </summary>
    class Eye
    {
        public Transform transform;
        public bool isDead = false;
        delegate void Behaviour();
        Behaviour behaviour;
        EyeBossControllerScript controller;
        List<GameObject> platformsToDisable;
        List<RecycledEffectScript> attachedEffects = new List<RecycledEffectScript>();

        //We are going to need some timers
        float timerOne, timerTwo;
        bool isGlowingUp = true;
        bool ranOnce = false;

        /// <summary>
        /// Constructor for the Eye class
        /// </summary>
        /// <param name="t">The transform that this class is used to represent</param>
        /// <param name="control">The controller class so we can access things</param>
        public Eye(Transform t, EyeBossControllerScript control)
        {
            platformsToDisable = new List<GameObject>();
            transform = t;
            behaviour = None;
            controller = control;
        }

        /// <summary>
        /// Updates the eye
        /// </summary>
        public bool Update()
        {
            behaviour();
            return isDead;
        }

        /// <summary>
        /// disables this eye because it has been destroyed
        /// </summary>
        public void SetDisabled()
        {
            isDead = true;
            ranOnce = false;
            behaviour = DeadBehaviour;
            timerOne = 0;
            timerTwo = 0;
        }

        /// <summary>
        /// Changes the eyes behaviour based on the controllers state
        /// THIS SHOULD NEVER SEND EYEBOSSSTATE.ATTACK
        /// </summary>
        /// <param name="state">The state this eye should be in</param>
        /// <param name="attack">The attack this eye should be doing</param>
        public void ChangeBehaviour(EyeBossState state, EyeBossAttacks attack = EyeBossAttacks.DisablePlatform)
        {
            attachedEffects.Clear();
            ranOnce = false;
            switch (state)
            {
                case EyeBossState.Idle:
                    behaviour = Idle;
                    break;
                case EyeBossState.Telegraph:
                    switch (attack)
                    {
                        case EyeBossAttacks.Spawn:
                            behaviour = TelegraphSpawn;
                            break;
                        case EyeBossAttacks.DisablePlatform:
                            platformsToDisable.Clear();
                            for (int i = 0; i < Constants.EYEBALL_BOSS_NUM_PLATFORMS_TO_DISABLE_PER_EYE; i++)
                            {
                                platformsToDisable.Add(controller.GetRandomPlatformToDisable());
                            }
                            behaviour = TelegraphDisablePlatform;
                            break;
                        case EyeBossAttacks.FireShots:
                            behaviour = TelegraphFireShots;
                            break;
                        case EyeBossAttacks.EnergyBeam:
                            behaviour = TelegraphEnergyBeam;
                            break;
                        case EyeBossAttacks.RotatingBeams:
                            break;
                        case EyeBossAttacks.DisablePlatformsAndSpawnMinions:
                            break;
                        case EyeBossAttacks.DisablePlatformsAndShootExplosives:
                            break;
                        case EyeBossAttacks.ShootExplosivesAndSpawnMinions:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    behaviour = None;
                    break;

            }
        }

        /// <summary>
        /// Cause the bouncing glow effect here
        /// </summary>
        /// <param name="c">The color we want to glow</param>
        void TelegraphEffect(Color c)
        {
            if ((isGlowingUp && (timerOne += Time.deltaTime) < Constants.EYEBALL_BOSS_GLOW_DURATION)
                || (!isGlowingUp && (timerOne -= Time.deltaTime) >= 0))
            {
                transform.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, c, timerOne);
            }
            else
            {
                isGlowingUp = !isGlowingUp;
            }
        }

        /// <summary>
        /// Dynamically creates a tendril
        /// </summary>
        /// <param name="startPosition">The position the tendril will start</param>
        /// <param name="objectToAttachTo">The object the tendril will attach to</param>
        public void DynamicallyCreateTendril(Vector2 startPosition, GameObject objectToAttachTo)
        {
            EyeBossTendrilScript s = (EyeBossTendrilScript)EffectManager.Instance.AddNewEffect(EffectRecyclerType.TendrilMain, startPosition, 0, Vector3.zero, 0, Constants.EYEBALL_BOSS_ATTACKING_DURATION + Constants.EYEBALL_BOSS_TELEGRAPH_DURATION, 0);
            s.SecondInitialize(objectToAttachTo);
            attachedEffects.Add(s);
        }

        #region shared behaviours

        /// <summary>
        /// Literally nothing.
        /// </summary>
        void None()
        {

        }

        /// <summary>
        /// Watch the player... Thats it
        /// </summary>
        void Idle()
        {
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));
        }

        #endregion

        #region smaller eye behaviours


        /// <summary>
        /// Telegraph that you are going to be spawning enemies soon
        /// </summary>
        void TelegraphSpawn()
        {
            timerTwo += Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));
            TelegraphEffect(Constants.EYEBALL_BOSS_TELEGRAPH_COLORS[(int)EyeBossAttacks.Spawn]);

            if (timerTwo >= Constants.EYEBALL_BOSS_TELEGRAPH_DURATION)
            {
                behaviour = AttackSpawn;
                transform.GetComponent<SpriteRenderer>().color = Color.white;
                timerOne = 0;
                timerTwo = 0;
            }
        }

        /// <summary>
        /// Telegraph that you are going to be disabling a platform soon
        /// </summary>
        void TelegraphDisablePlatform()
        {
            timerTwo += Time.deltaTime;
            if (!ranOnce)
            {
                Vector2 averagedPosition = Vector2.zero;
                foreach (GameObject g in platformsToDisable)
                {
                    averagedPosition += (Vector2)g.transform.position;
                    for (int i = 0; i < Constants.EYEBALL_BOSS_TENDRILS_PER_PLATFORM; i++)
                    {
                        DynamicallyCreateTendril(transform.position, g);
                    }
                }
                averagedPosition /= platformsToDisable.Count;
                transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, averagedPosition));
                ranOnce = true;
                
            }
            TelegraphEffect(Constants.EYEBALL_BOSS_TELEGRAPH_COLORS[(int)EyeBossAttacks.DisablePlatform]);

            if (timerTwo >= Constants.EYEBALL_BOSS_TELEGRAPH_DURATION)
            {
                behaviour = AttackDisablePlatform;
                transform.GetComponent<SpriteRenderer>().color = Color.white;
                timerOne = 0;
                timerTwo = 0;
                ranOnce = false;
            }
        }

        /// <summary>
        /// Telegraph that you are going to start shooting soon
        /// </summary>
        void TelegraphFireShots()
        {
            timerTwo += Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));
            TelegraphEffect(Constants.EYEBALL_BOSS_TELEGRAPH_COLORS[(int)EyeBossAttacks.FireShots]);

            if (timerTwo >= Constants.EYEBALL_BOSS_TELEGRAPH_DURATION)
            {
                behaviour = AttackFireShots;
                transform.GetComponent<SpriteRenderer>().color = Color.white;
                timerOne = 0;
                timerTwo = Constants.EYEBALL_BOSS_PROJECTILE_TIME_BETWEEN_SHOTS; //Do this so it shoots one immediately instead of waiting the duration first
            }
        }

        /// <summary>
        /// Telegraph that you are going to start charging an energy beam soon
        /// </summary>
        void TelegraphEnergyBeam()
        {
            timerTwo += Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));
            TelegraphEffect(Constants.EYEBALL_BOSS_TELEGRAPH_COLORS[(int)EyeBossAttacks.EnergyBeam]);

            if (!ranOnce)
            {
                attachedEffects.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.IndicatorBeam, transform.position, transform.eulerAngles.z, Vector3.zero, 0, Constants.EYEBALL_BOSS_INDICATOR_BEAM_DURATION, Constants.EYEBALL_BOSS_INDICATOR_BEAM_TIME_UNTIL_FADE_START));
                attachedEffects[attachedEffects.Count - 1].transform.SetParent(transform, true);
                ranOnce = true;
            }

            if (timerTwo >= Constants.EYEBALL_BOSS_TELEGRAPH_DURATION)
            {
                behaviour = AttackEnergyBeam;
                ranOnce = false;
                transform.GetComponent<SpriteRenderer>().color = Color.white;
                timerOne = 0;
                timerTwo = 0;
            }
        }

        /// <summary>
        /// Spawn enemies
        /// </summary>
        void AttackSpawn()
        {
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));
            //TODO: Create the enemies on a short delay
            timerTwo += Time.deltaTime;
            if (timerTwo >= Constants.EYEBALL_BOSS_SPAWN_TIME_BETWEEN_SPAWNS)
            {
                attachedEffects.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.EyeBossMiniEye, transform.position, 0, Utilities.RotateDegrees(GameManager.Instance.Player.transform.position - transform.position, Random.Range(-90, 90)), 0, 30));
                timerTwo = 0;
            }

            timerOne += Time.deltaTime;
            if (timerOne >= Constants.EYEBALL_BOSS_ATTACKING_DURATION)
            {
                timerOne = 0;
                ranOnce = false;
                ChangeBehaviour(EyeBossState.Idle);
            }
        }

        /// <summary>
        /// Disable a platform
        /// </summary>
        void AttackDisablePlatform()
        {            
            if (!ranOnce)
            {
                foreach(GameObject g in platformsToDisable)
                {
                    g.GetComponent<TestBadWalls>().isBadWall = true;
                    g.GetComponent<SpriteRenderer>().color = Color.red;
                }

                ranOnce = true;
            }

            timerOne += Time.deltaTime;
            if (timerOne >= Constants.EYEBALL_BOSS_ATTACKING_DURATION)
            {
                timerOne = 0;
                ranOnce = false;
                ChangeBehaviour(EyeBossState.Idle);
            }
        }

        /// <summary>
        /// Fire shots
        /// </summary>
        void AttackFireShots()
        {
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));

            timerTwo += Time.deltaTime;
            if (timerTwo >= Constants.EYEBALL_BOSS_PROJECTILE_TIME_BETWEEN_SHOTS)
            {
                attachedEffects.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.EyeBossProjectile, transform.position, 0, Utilities.LookAtDirection(transform.eulerAngles.z).normalized, 0, Constants.EYEBALL_BOSS_PROJECTILE_DURATION));
                timerTwo = 0;
            }

            timerOne += Time.deltaTime;
            if (timerOne >= Constants.EYEBALL_BOSS_ATTACKING_DURATION)
            {
                timerOne = 0;
                ranOnce = false;
                ChangeBehaviour(EyeBossState.Idle);
            }
        }

        /// <summary>
        /// Fire an energy beam
        /// </summary>
        void AttackEnergyBeam()
        {
            timerOne += Time.deltaTime;
            if (!ranOnce && timerOne >= Constants.EYEBALL_BOSS_LAZER_BEAM_CHARGE_TIME)
            {
                attachedEffects.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.EyeBossBeam, transform.position, transform.eulerAngles.z, Vector3.zero, 0, Constants.EYEBALL_BOSS_LAZER_BEAM_DURATION, Constants.EYEBALL_BOSS_LAZER_BEAM_TIME_UNTIL_FADE_START));
                ranOnce = true;
            }

            if (timerOne >= Constants.EYEBALL_BOSS_ATTACKING_DURATION)
            {
                timerOne = 0;
                ranOnce = false;
                ChangeBehaviour(EyeBossState.Idle);
            }
        }

        /// <summary>
        /// What should this object do whenever it is dead.
        /// </summary>
        void DeadBehaviour()
        {
            if (!ranOnce)
            {
                ranOnce = true;
                for (int i = 0; i < 75; i++)
                {
                    EffectManager.Instance.AddNewEffect(EffectRecyclerType.BloodSplatter, transform.position, 0, Utilities.RotateDegrees(Vector2.right, Random.Range(0, 360)) * Random.Range(Constants.BLOOD_SPLATTER_INIT_SPEED / 2, Constants.BLOOD_SPLATTER_INIT_SPEED), 0, 0, 0);
                }
                GameManager.Instance.Camera.ApplyCameraShake(Constants.SHIP_BOSS_DEATH_CAMERA_SHAKE_INTENSITY, Constants.SHIP_BOSS_DEATH_CAMERA_SHAKE_DURATION);
                transform.GetComponent<SpriteRenderer>().color = Color.clear;
                transform.GetComponent<CircleCollider2D>().enabled = false;
                foreach (RecycledEffectScript s in attachedEffects)
                {
                    s.EndEffectEarly();
                }
            }


            if (timerTwo <= Constants.EYEBALL_BOSS_PHASE_ONE_DESTRUCTION_BLOOD_SPAWN_DURATION)
            {
                timerOne += Time.deltaTime;
                if (timerOne >= Constants.EYEBALL_BOSS_PHASE_ONE_DESTRUCTION_BLOOD_SPAWN_INTERVAL)
                {
                    timerTwo += timerOne;
                    for (int i = 0; i < 75; i++)
                    {
                        EffectManager.Instance.AddNewEffect(EffectRecyclerType.BloodSplatter, transform.position, 0, Utilities.RotateDegrees(Vector2.right, Random.Range(0, 360)) * Random.Range(Constants.BLOOD_SPLATTER_INIT_SPEED / 2, Constants.BLOOD_SPLATTER_INIT_SPEED), 0, 0, 0);
                    }
                    timerOne = 0;
                }
            }
        }


        #endregion

        #region Main eye behaviours

        /// <summary>
        /// Rotates beams around
        /// </summary>
        void TelegraphRotatingBeams()
        {
            //timerTwo += Time.deltaTime;
            //transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));
            //TelegraphEffect(Constants.EYEBALL_BOSS_TELEGRAPH_COLORS[(int)EyeBossAttacks.EnergyBeam]);

            //if (!ranOnce)
            //{
            //    attachedEffects.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.IndicatorBeam, transform.position, transform.eulerAngles.z, Vector3.zero, 0, Constants.EYEBALL_BOSS_INDICATOR_BEAM_DURATION, Constants.EYEBALL_BOSS_INDICATOR_BEAM_TIME_UNTIL_FADE_START));
            //    attachedEffects[attachedEffects.Count - 1].transform.SetParent(transform, true);
            //    ranOnce = true;
            //}

            //if (timerTwo >= Constants.EYEBALL_BOSS_TELEGRAPH_DURATION)
            //{
            //    behaviour = AttackEnergyBeam;
            //    ranOnce = false;
            //    transform.GetComponent<SpriteRenderer>().color = Color.white;
            //    timerOne = 0;
            //    timerTwo = 0;
            //}
        }

        /// <summary>
        /// Disables platforms and spawns minions
        /// </summary>
        void TelegraphDisablePlatformsAndSpawnMinions()
        {

        }

        /// <summary>
        /// Disables platforms and shoots explosives
        /// </summary>
        void TelegraphDisablePlatformsAndShootExplosives()
        {

        }

        /// <summary>
        /// Shoots explosives and spawns minions
        /// </summary>
        void TelegraphShootExplosivesAndSpawnMinions()
        {

        }


        /// <summary>
        /// Rotating beams around the eye
        /// </summary>
        void AttackRotatingBeams()
        {
            //timerOne += Time.deltaTime;
            //if (!ranOnce && timerOne >= Constants.EYEBALL_BOSS_LAZER_BEAM_CHARGE_TIME)
            //{
            //    attachedEffects.Add(EffectManager.Instance.AddNewEffect(EffectRecyclerType.EyeBossBeam, transform.position, transform.eulerAngles.z, Vector3.zero, 0, Constants.EYEBALL_BOSS_LAZER_BEAM_DURATION, Constants.EYEBALL_BOSS_LAZER_BEAM_TIME_UNTIL_FADE_START));
            //    ranOnce = true;
            //}

            //if (timerOne >= Constants.EYEBALL_BOSS_ATTACKING_DURATION)
            //{
            //    timerOne = 0;
            //    ranOnce = false;
            //    ChangeBehaviour(EyeBossState.Idle);
            //}
        }

        /// <summary>
        /// Disables platforms and spawns minions
        /// </summary>
        void AttackDisablePlatformsAndSpawnMinions()
        {

        }

        /// <summary>
        /// Disables platforms and shoots explosives
        /// </summary>
        void AttackDisablePlatformsAndShootExplosives()
        {

        }
        
        /// <summary>
        /// Shoots explosives and spawns minions
        /// </summary>
        void AttackShootExplosivesAndSpawnMinions()
        {

        }

        #endregion
    }
}

