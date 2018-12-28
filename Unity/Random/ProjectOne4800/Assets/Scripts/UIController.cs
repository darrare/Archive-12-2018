using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public void StartButtonClick()
    {
        Manager.Instance.Start();
    }

    public void ReturnButtonClick()
    {
        Manager.Instance.BackToNormal();
    }
}
