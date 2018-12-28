using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CatAndMouse
{
    /// <summary>
    /// This class is ugly and dumb. It does the job, but expanding it would be horrifying. 
    /// </summary>
    class SaveLoadManager
    {
        static SaveLoadManager instance;
        List<Object> objectsToSave = new List<Object>();
        List<Object> serializableObjects = new List<Object>();

        /// <summary>
        /// Singleton for the saveloadmanager class
        /// </summary>
        public static SaveLoadManager Instance
        {
            get { return instance != null ? instance : instance = new SaveLoadManager(); }
        }

        /// <summary>
        /// property for the list of objects we want to save
        /// </summary>
        public List<Object> ObjectsToSave
        {
            get { return objectsToSave; }
            set
            {
                objectsToSave = value;
            }
        }

        /// <summary>
        /// property for the list of objects we want to save
        /// </summary>
        public List<Object> SerializableObjects
        {
            get { return serializableObjects; }
            set
            {
                serializableObjects = value;
            }
        }

        /// <summary>
        /// Updates the saveloadmanager. Handles input for saving and loading
        /// </summary>
        public void Update()
        {
            if (InputManager.Instance.IsKeyDown(Keys.F1))
            {
                Save();
            }
            if (InputManager.Instance.IsKeyDown(Keys.F2))
            {
                Load();
            }
        }

        /// <summary>
        /// Saves the objects stored in ObjectsToSave. Converts them to a saveable type
        /// </summary>
        void Save()
        {
            SerializableObjects.Clear();
            //Goes through the list and saves the objects as a type that is serializable.
            foreach (Object obj in ObjectsToSave)
            {
                if (obj is Mouse)
                {
                    SerializableObjects.Add(new SaveableMouse(((Mouse)obj).CanJump, ((Mouse)obj).IsInAttackMode, ((Mouse)obj).Position, ((Mouse)obj).Scale));
                }
                else if (obj is Cat)
                {
                    SerializableObjects.Add(new SaveableCat(((Cat)obj).Position));
                }
                else if (obj is Text)
                {
                    SerializableObjects.Add(new SaveableText(((Text)obj).TextContent));
                }
                else if (obj is RechargeBar)
                {
                    SerializableObjects.Add(new SaveableRechargeBar(((RechargeBar)obj).Timer));
                }
                else if (obj is CooldownBar)
                {
                    SerializableObjects.Add(new SaveableCooldownBar(((CooldownBar)obj).Timer));
                }
            }

            //Didn't want to do single specific things, but this was by far the easiest way to do it for now.
            SerializableObjects.Add(new SaveableKeyboardConfig(KeyboardConfiguration.MoveUp, KeyboardConfiguration.MoveDown, KeyboardConfiguration.MoveLeft, KeyboardConfiguration.MoveRight, KeyboardConfiguration.MainKey, KeyboardConfiguration.Boost, KeyboardConfiguration.AttackSequence));
            SerializableObjects.Add(GameManager.Instance.IsPaused);
            SerializableObjects.Add(GameManager.Instance.TimeRemaining);

            using (Stream stream = File.Open("data.bin", FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, SerializableObjects);
            }
        }

        /// <summary>
        /// Loads the object from the binary file and copies their data into the existing object.
        /// </summary>
        void Load()
        {
            using (Stream stream = File.Open("data.bin", FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();

                SerializableObjects = (List<Object>)bin.Deserialize(stream);
            }

            //Handle any object that has a single entity in the game world. Loads their saved data back into the object that originally saved it
            foreach (Object obj in SerializableObjects)
            {
                if (obj is SaveableMouse)
                {
                    GameManager.Instance.Player.LoadSerializableMouse((SaveableMouse)obj);
                }
                else if (obj is SaveableCooldownBar)
                {
                    GameManager.Instance.Player.CDBar.LoadSerializableCooldownBar((SaveableCooldownBar)obj);
                }
                else if (obj is SaveableKeyboardConfig)
                {
                    KeyboardConfiguration.SetConfiguration(((SaveableKeyboardConfig)obj).moveUp, ((SaveableKeyboardConfig)obj).moveLeft, ((SaveableKeyboardConfig)obj).moveDown, ((SaveableKeyboardConfig)obj).moveRight, ((SaveableKeyboardConfig)obj).mainKey, ((SaveableKeyboardConfig)obj).boost, ((SaveableKeyboardConfig)obj).attackSequence);
                }
                else if (obj is bool)
                {
                    GameManager.Instance.IsPaused = ((bool)obj);
                }
                else if (obj is float)
                {
                    GameManager.Instance.TimeRemaining = ((float)obj);
                }
            }

            //Handles all of the cats.
            int index = 0;
            foreach (Object obj in SerializableObjects.Where(t => t is SaveableCat))
            {
                ((Cat)ObjectsToSave.Where(t => t is Cat).ToList()[index]).LoadSerializableCat((SaveableCat)obj);
                index++;
            }

            //Handles the two recharge bars
            index = 0;
            foreach (Object obj in SerializableObjects.Where(t => t is SaveableRechargeBar))
            {
                ((RechargeBar)ObjectsToSave.Where(t => t is RechargeBar).ToList()[index]).LoadSerializableRechargeBar((SaveableRechargeBar)obj);
                index++;
            }

            //Handles all of the text
            index = 0;
            foreach (Object obj in SerializableObjects.Where(t => t is SaveableText))
            {
                ((Text)ObjectsToSave.Where(t => t is Text).ToList()[index]).LoadSerializableText((SaveableText)obj);
                index++;
            }


        }
    }

    /// <summary>
    /// Was oringinally planning on using this to have a list of all below classes, never ended up using but didn't want to remove. Yolo
    /// </summary>
    [Serializable]
    class SerializableObject
    {

    }

    /// <summary>
    /// Save data for the mouse
    /// </summary>
    [Serializable]
    class SaveableMouse : SerializableObject
    {
        public bool canJump;
        public bool isInAttackMode;
        public Vector2 position;
        public Vector2 scale;

        public SaveableMouse(bool canJump, bool isInAttackMode, Vector2 position, Vector2 scale)
        {
            this.canJump = canJump;
            this.isInAttackMode = isInAttackMode;
            this.position = position;
            this.scale = scale;
        }
    }

    /// <summary>
    /// save data for the cat
    /// </summary>
    [Serializable]
    class SaveableCat : SerializableObject
    {
        public Vector2 position;

        public SaveableCat(Vector2 position)
        {
            this.position = position;
        }
    }

    //save data for any text
    [Serializable]
    class SaveableText : SerializableObject
    {
        public string text;

        public SaveableText(string text)
        {
            this.text = text;
        }
    }

    //save data for the recharge bars
    [Serializable]
    class SaveableRechargeBar : SerializableObject
    {
        public float timer;

        public SaveableRechargeBar(float timer)
        {
            this.timer = timer;
        }
    }

    //save data for the cooldown bars
    [Serializable]
    class SaveableCooldownBar : SerializableObject
    {
        public float timer;

        public SaveableCooldownBar(float timer)
        {
            this.timer = timer;
        }
    }

    //save data for the keyboard config
    [Serializable]
    class SaveableKeyboardConfig : SerializableObject
    {
        public Keys moveUp;
        public Keys moveDown;
        public Keys moveLeft;
        public Keys moveRight;
        public Keys mainKey;
        public List<Keys> boost;
        public List<Keys> attackSequence;

        public SaveableKeyboardConfig(Keys moveUp, Keys moveDown, Keys moveLeft, Keys moveRight, Keys mainKey, List<Keys> boost, List<Keys> attackSequence)
        {
            this.moveUp = moveUp;
            this.moveDown = moveDown;
            this.moveLeft = moveLeft;
            this.moveRight = moveRight;
            this.mainKey = mainKey;
            this.boost = boost;
            this.attackSequence = attackSequence;
        }
    }

}
