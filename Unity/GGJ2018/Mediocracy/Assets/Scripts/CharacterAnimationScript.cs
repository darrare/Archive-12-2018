using System;
using AUTools.Unity;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mediocracy
{
    /// <summary>Script that controls the character animations.</summary>
    public class CharacterAnimationScript : PauseObjScript
    {
        #region Fields

        [SerializeField, Range(0, 1)] float speedPct;
        [SerializeField] Anim[] animations;
        [SerializeField] SpriteRenderer[] specialOnlyRenderers;
        [SerializeField] SpriteRenderer[] normalOnlyRenderers;

        float positionShift;

        #endregion

        #region Properties

        /// <summary>Gets or sets the animation speed percentage.</summary>
        /// <value>The speed percentage.</value>
        public float SpeedPct
        {
            get { return speedPct; }
            set
            {
                speedPct = value;
                foreach (Anim anim in animations)
                {
                    anim.SetSpeedPct(speedPct);
                }
                foreach (SpriteRenderer sprite in specialOnlyRenderers)
                {
                    sprite.material.SetFloat("_Alpha", speedPct);
                }
                foreach (SpriteRenderer sprite in normalOnlyRenderers)
                {
                    sprite.material.SetFloat("_Alpha", 1 - speedPct);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>Updates the object. Only called if the object is not paused.</summary>
        protected override void NotPausedUpdate()
        {
            foreach (Anim anim in animations)
            {
                anim.Update();
            }
            positionShift += SpeedPct * Random.value * Time.deltaTime * Constants.MOVE_MULT;
            Vector3 newPosition = transform.localPosition;
            newPosition.x = Mathf.Sin(positionShift);
            transform.localPosition = newPosition;
        }

        #endregion

        #region Private Methods

        /// <summary>Awake is called immediately on creation.</summary>
        void Awake()
        {
            Initialize();
            foreach (Anim anim in animations)
            {
                anim.Initialize();
            }
        }

        /// <summary>Resets this CharacterAnimationScript.</summary>
        void Reset()
        {
            Transform[] children = GetComponentsInChildren<Transform>();
            animations = new Anim[children.Length];
            for (int i = 0; i < children.Length; i++)
            {
                animations[i] = new Anim(children[i]);
            }
        }

        #endregion

        #region Anim

        /// <summary>An animation. This class cannot be inherited.</summary>
        [Serializable]
        sealed class Anim
        {
            [SerializeField] Transform obj;
            [SerializeField] bool bidirectional;
            [SerializeField] float maxRotation;
            [SerializeField] float maxSpeed;
            float speedPct;
            Vector3 baseEulerAngles;
            float currShift;
            float currAngle;
            float currSpeed;
            float currMaxRotation;
            float defaultAngle;
            PsychedelicShaderDriver shader;

            /// <summary>Constructor.</summary>
            /// <param name="obj">The object. This cannot be null.</param>
            public Anim([NotNull] Transform obj)
            {
                this.obj = obj;
            }

            /// <summary>Sets the speed percentage.</summary>
            /// <param name="value">The speed percentage.</param>
            public void SetSpeedPct(float value)
            {
                speedPct = value;
                if (!bidirectional)
                {
                    baseEulerAngles = new Vector3(0, 0, defaultAngle + (maxRotation * speedPct));
                }
                currMaxRotation = maxRotation * speedPct;
                currSpeed = maxSpeed * speedPct;
                shader.insanity = speedPct;
            }

            /// <summary>Initializes this Anim.</summary>
            public void Initialize()
            {
                defaultAngle = obj.localEulerAngles.z;
                currShift = Random.value;
                shader = obj.GetComponent<PsychedelicShaderDriver>();
            }

            /// <summary>Updates this Anim.</summary>
            public void Update()
            {
                currAngle += currSpeed * Time.deltaTime;
                currShift += Random.value * Time.deltaTime * currSpeed;
                obj.localEulerAngles = new Vector3(0, 0, Mathf.Cos(currAngle + currShift) * currMaxRotation) + baseEulerAngles;
            }
        }

        #endregion
    }
}