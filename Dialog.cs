using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialog : MonoBehaviour
{
    [SerializeField] Text orderingDialog;
    float timeBetweenLetters = 0.055f;

    float defaultTimeToWait = 1;
    float timeToWaitForOneLetter = 0.2f;

    public bool isSayingDialogLine { get; private set; }

    public IEnumerator ShowDialogLine(string dialogLine)
    {
        isSayingDialogLine = true;

        orderingDialog.text = "";

        for (int i = 0; i < dialogLine.Length; i++)
        {
            orderingDialog.text += dialogLine[i];
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        isSayingDialogLine = false;

        yield return new WaitForSeconds(timeToWaitForOneLetter * dialogLine.Length + defaultTimeToWait);

        if (!isSayingDialogLine) // this is for the situation when another ShowDialogLine gets called and not to wipe the text in the middle
        {
            DeleteDialog();
        }
    }

    public void DeleteDialog()
    {
        orderingDialog.text = "";
    }
}
