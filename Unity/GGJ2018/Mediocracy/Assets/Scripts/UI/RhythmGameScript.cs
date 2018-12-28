using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Mediocracy
{
    public class RhythmGameScript : UIElementScript
    {
        [SerializeField] GameObject tuplePrefab;

        GameObject explosionPrefab;

        [SerializeField] RectTransform eventPoint;

        [SerializeField] RectTransform panelElement;

        [SerializeField] Sprite[] udlrImages;

        List<Tuple> tuples = new List<Tuple>();
        List<Vector2> positions = new List<Vector2>();

        int numElements = 0;
        int eventIndex = 0;
        public int failedIndex = 0;
        bool isFailed = false;

        public override MenuType Type => MenuType.RhythmGame;

        // Use this for initialization
        void Start()
        {
            explosionPrefab = Resources.Load<GameObject>("Prefabs/ParticleEffects/Explosion");
            numElements = (int)Mathf.Floor((GameManager.Instance.CurRoom.GetComponent<RoomStatusScript>().Level * Constants.RHYTHM_NEW_TUPLES_PER_FLOOR) +
                Constants.RHYTHM_LEVEL_ONE_TUPLES);

            float totalGap = (Constants.RHYTHM_GAP_BETWEEN_TUPLES * Constants.RHYTHM_LOCATIONS_IN_PANEL_BAR) + 1;
            float totalWidth = panelElement.GetComponent<RectTransform>().rect.width;
            float widthOfTuples = (totalWidth - totalGap) / Constants.RHYTHM_LOCATIONS_IN_PANEL_BAR;
            float ratioOfTuple = widthOfTuples / tuplePrefab.GetComponent<RectTransform>().rect.width;
            tuplePrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(tuplePrefab.GetComponent<RectTransform>().rect.width * ratioOfTuple,
                panelElement.GetComponent<RectTransform>().rect.height * Constants.RHYTHM_PERCENTAGE_PANEL_HEIGHT_FOR_TUPPLE);

            GenerateUI();
        }

        // Update is called once per frame
        void Update()
        {
            //input handling
            if (tuples.Count >= 0 && tuples.FirstOrDefault(t => !t.isFailed)?.obj.GetComponent<RectTransform>().anchoredPosition.x < positions[positions.Count - 1].x)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Tuple temp = tuples.FirstOrDefault(t => !t.isFailed);
                    if (temp != null)
                    {
                        temp.InputKey(InputOptions.Up, udlrImages[0]);
                        if (temp.IsCorrect)
                        {
                            temp.Delete();
                            tuples.Remove(temp);
                            AudioManager.Instance.PlayOneShot(SoundEffects.ArrowHit);
                        }
                        else
                        {
                            temp.TupleFailed(positions[failedIndex].x);
                            AudioManager.Instance.PlayOneShot(SoundEffects.ArrowMiss);
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Tuple temp = tuples.FirstOrDefault(t => !t.isFailed);
                    if (temp != null)
                    {
                        temp.InputKey(InputOptions.Down, udlrImages[1]);
                        if (temp.IsCorrect)
                        {
                            temp.Delete();
                            tuples.Remove(temp);
                            AudioManager.Instance.PlayOneShot(SoundEffects.ArrowHit);
                        }
                        else
                        {
                            temp.TupleFailed(positions[failedIndex].x);
                            AudioManager.Instance.PlayOneShot(SoundEffects.ArrowMiss);
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Tuple temp = tuples.FirstOrDefault(t => !t.isFailed);
                    if (temp != null)
                    {
                        temp.InputKey(InputOptions.Left, udlrImages[2]);
                        if (temp.IsCorrect)
                        {
                            temp.Delete();
                            tuples.Remove(temp);
                            AudioManager.Instance.PlayOneShot(SoundEffects.ArrowHit);
                        }
                        else
                        {
                            temp.TupleFailed(positions[failedIndex].x);
                            AudioManager.Instance.PlayOneShot(SoundEffects.ArrowMiss);
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Tuple temp = tuples.FirstOrDefault(t => !t.isFailed);
                    if (temp != null)
                    {
                        temp.InputKey(InputOptions.Right, udlrImages[3]);
                        if (temp.IsCorrect)
                        {
                            temp.Delete();
                            tuples.Remove(temp);
                            AudioManager.Instance.PlayOneShot(SoundEffects.ArrowHit);
                        }
                        else
                        {
                            temp.TupleFailed(positions[failedIndex].x);
                            AudioManager.Instance.PlayOneShot(SoundEffects.ArrowMiss);
                        }
                    }
                }
            }

            for (int i = tuples.Count - 1; i >= 0; i--)
            {
                if (failedIndex < Constants.RHYTHM_LOCATIONS_IN_PANEL_BAR)
                {
                    tuples[i].Update(positions[failedIndex]);
                }
                else
                {
                    OnFailure();
                }
            }
            if (tuples.Count == 0 && !isFailed && failedIndex * 2 < numElements)
            {
                OnSuccess();
            }
            else if (tuples.Count == 0 && !isFailed && failedIndex * 2 > numElements)
            {
                OnFailure();
            }

        }

        /// <summary>
        /// Whenever the player is sucessful
        /// </summary>
        void OnSuccess()
        {
            AudioManager.Instance.PlayOneShot(SoundEffects.MiniGameWin);
            Instantiate(Resources.Load<GameObject>("Prefabs/ParticleEffects/PartyPopper"),
                new Vector3(GameManager.Instance.CurRoom.transform.position.x - Constants.BLOCK_WIDTH, GameManager.Instance.CurRoom.transform.position.y, -3),
                Quaternion.Euler(new Vector3(300, 90, 0)));
            Instantiate(Resources.Load<GameObject>("Prefabs/ParticleEffects/PartyPopper"),
                new Vector3(GameManager.Instance.CurRoom.transform.position.x + Constants.BLOCK_WIDTH, GameManager.Instance.CurRoom.transform.position.y, -3),
                Quaternion.Euler(new Vector3(240, 90, 0)));
            GameManager.Instance.CurRoom.GetComponent<RoomStatusScript>().Insanity -=
                (1 - ((float)failedIndex / numElements)) / GameManager.Instance.CurRoom.GetComponent<RoomPopulator>().Transposers.Count;
            UIManager.Instance.CloseUI(MenuType.RhythmGame);
            Destroy(gameObject);
        }

        IEnumerator Explosions()
        {
            int i = 0;
            while (i < 10)
            {
                i++;
                if (GameManager.Instance.CurRoom != null)
                {
                    Instantiate(explosionPrefab, GetRandomSpotInRectangle(GameManager.Instance.CurRoom.transform.position), Quaternion.identity);
                }
                yield return new WaitForSeconds(.1f);
            }
            UIManager.Instance.CloseUI(MenuType.RhythmGame);
            Destroy(gameObject);
        }

        Vector3 GetRandomSpotInRectangle(Vector2 position)
        {
            return new Vector3(Random.Range(position.x - Constants.BLOCK_WIDTH, position.x + Constants.BLOCK_WIDTH),
                Random.Range(position.y, position.y + (Constants.BLOCK_HEIGHT * 2)), -3);
        }

        /// <summary>
        /// Whenever the player has failed
        /// </summary>
        void OnFailure()
        {
            isFailed = true;
            for (int i = tuples.Count - 1; i >= 0; i--)
            {
                if (tuples[i].obj.GetComponent<Image>().color == Color.white)
                {
                    Destroy(tuples[i].obj);
                    tuples.RemoveAt(i);
                }
            }
            StartCoroutine(Explosions());
        }

        /// <summary>
        /// Generates the position of UI elements
        /// </summary>
        void GenerateUI()
        {
            float offset = tuplePrefab.GetComponent<RectTransform>().rect.width + Constants.RHYTHM_GAP_BETWEEN_TUPLES;
            Vector2 initPosition =
                new Vector2(panelElement.GetComponent<RectTransform>().rect.x + (tuplePrefab.GetComponent<RectTransform>().rect.width / 2) + Constants.RHYTHM_GAP_BETWEEN_TUPLES,
                    panelElement.GetComponent<RectTransform>().rect.y + (panelElement.GetComponent<RectTransform>().rect.height * .1f) +
                    (tuplePrefab.GetComponent<RectTransform>().rect.height / 2));

            for (int i = 0; i < Constants.RHYTHM_LOCATIONS_IN_PANEL_BAR; i++)
            {
                positions.Add(initPosition + (Vector2.right * offset * i));
            }

            for (int i = Constants.RHYTHM_LOCATIONS_IN_PANEL_BAR; i < Constants.RHYTHM_LOCATIONS_IN_PANEL_BAR + numElements; i++)
            {
                int random = Random.Range(0, 4);
                tuples.Add(new Tuple(eventIndex + 1 + i, initPosition + (i * Vector2.right * offset), Instantiate(tuplePrefab, panelElement, false), random, udlrImages[random],
                    this));
            }
        }

        /// <summary>
        /// The tuple object that contains the position, index, and gameobject
        /// </summary>
        class Tuple
        {
            public int index = 0;
            public GameObject obj;
            InputOptions displayedValue;
            InputOptions userInputValue;
            public bool isFailed = false;
            float xPositionToStopAt = 0;
            RhythmGameScript parent;

            float timer = 0;

            public Tuple(int index, Vector2 startPos, GameObject obj, int randomNum, Sprite sprite, RhythmGameScript parent)
            {
                displayedValue = (InputOptions)randomNum;
                obj.GetComponentsInChildren<Image>()[1].sprite = sprite;
                this.index = index;
                this.obj = obj;
                obj.GetComponent<RectTransform>().anchoredPosition = startPos;
                this.parent = parent;
            }

            public void TupleFailed(float xPositionToStopAt)
            {
                if (!isFailed)
                {
                    isFailed = true;
                    parent.failedIndex++;
                }

                this.xPositionToStopAt = xPositionToStopAt;
            }

            public bool IsCorrect =>
                displayedValue == InputOptions.Up && userInputValue == InputOptions.Down || displayedValue == InputOptions.Down && userInputValue == InputOptions.Up ||
                displayedValue == InputOptions.Left && userInputValue == InputOptions.Right || displayedValue == InputOptions.Right && userInputValue == InputOptions.Left;

            /// <summary>
            /// Send an input key to this tuple
            /// </summary>
            /// <param name="option">the input key to send</param>
            public void InputKey(InputOptions option, Sprite sprite)
            {
                userInputValue = option;
                obj.GetComponentsInChildren<Image>()[2].sprite = sprite;
            }

            /// <summary>
            /// slides the object to the side to explode
            /// </summary>
            /// <param name="xPositionToStopAt"></param>
            public void Update(Vector2 position)
            {
                if (obj.GetComponent<RectTransform>().anchoredPosition.x - (xPositionToStopAt != 0 ? xPositionToStopAt : position.x) < (Vector2.right *
                    ((isFailed
                        ? (Constants.RHYTHM_TUPLE_SLIDE_SPEED * Constants.RHYTHM_FAILED_SPEED_MULTIPLIER) +
                        (Constants.RHYTHM_PER_FLOOR_MULTIPLIER * GameManager.Instance.CurRoom.GetComponent<RoomStatusScript>().Level) : Constants.RHYTHM_TUPLE_SLIDE_SPEED +
                        (Constants.RHYTHM_PER_FLOOR_MULTIPLIER * GameManager.Instance.CurRoom.GetComponent<RoomStatusScript>().Level)) * Time.deltaTime)).x)
                {
                    obj.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(xPositionToStopAt != 0 ? xPositionToStopAt : position.x, obj.GetComponent<RectTransform>().anchoredPosition.y);
                    if (!isFailed)
                    {
                        isFailed = true;
                        parent.failedIndex++;
                    }
                    if (parent.tuples.Contains(this))
                    {
                        parent.tuples.Remove(this);
                        foreach (Image i in obj.GetComponentsInChildren<Image>())
                        {
                            i.color = new Color(.6f, .6f, .6f, 1);
                        }
                    }
                }
                else
                {
                    obj.GetComponent<RectTransform>().anchoredPosition -= Vector2.right * ((isFailed
                        ? (Constants.RHYTHM_TUPLE_SLIDE_SPEED * Constants.RHYTHM_FAILED_SPEED_MULTIPLIER) +
                        (Constants.RHYTHM_PER_FLOOR_MULTIPLIER * GameManager.Instance.CurRoom.GetComponent<RoomStatusScript>().Level) : Constants.RHYTHM_TUPLE_SLIDE_SPEED +
                        (Constants.RHYTHM_PER_FLOOR_MULTIPLIER * GameManager.Instance.CurRoom.GetComponent<RoomStatusScript>().Level)) * Time.deltaTime);
                }
            }

            public void Delete()
            {
                obj.GetComponent<Image>().StartCoroutine(FadeOut());
            }

            /// <summary>
            /// Fade out
            /// </summary>
            /// <returns></returns>
            IEnumerator FadeOut()
            {
                while (timer < Constants.RHYTHM_CORRECT_RESPONSE_FADE_TIME)
                {
                    timer += Time.deltaTime;
                    obj.GetComponent<RectTransform>().anchoredPosition -= Vector2.up * (Constants.RHYTHM_TUPLE_SLIDE_SPEED +
                        (Constants.RHYTHM_PER_FLOOR_MULTIPLIER * GameManager.Instance.CurRoom.GetComponent<RoomStatusScript>().Level)) * Time.deltaTime;
                    foreach (Image i in obj.GetComponentsInChildren<Image>())
                    {
                        i.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer / Constants.RHYTHM_CORRECT_RESPONSE_FADE_TIME);
                    }
                    yield return null;
                }
                Destroy(obj);
            }
        }
    }
}