using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugReportButton : MonoBehaviour
{
    public void OpenURL()
    {
        Application.OpenURL("https://forms.gle/CAjGGrMJ8gUc5ynw8");
        Debug.Log("Is this working?");
    }

}
