using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{
    //Extension for copying a string to the user's clipboard
    public static void CopyToClipboard(this string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }
    //Error code display
    public static void displayError(string errorCode) {
        GameObject errorPanel = GameObject.Find("ErrorScreen");
        GameObject errorText = errorPanel.transform.GetChild(0).gameObject;
        errorText.GetComponent<Text>().text = "Error Code " + errorCode;
        errorPanel.SetActive(true);
    }
}
