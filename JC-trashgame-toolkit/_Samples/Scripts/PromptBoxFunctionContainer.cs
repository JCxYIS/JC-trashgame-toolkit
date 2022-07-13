using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PromptBoxFunctionContainer : MonoBehaviour
{
    public void PopMessage(string msg)
    {
        PromptBox.CreateMessageBox(msg);
    }

    public void PopConfirm()
    {
        PromptBox.Create(new PromptBoxSettings{
            Title = "",
            Content = "This is a prompt box with options",
            ConfirmButtonText = "Cool!",
            ConfirmCallback = () => {
                PromptBox.CreateMessageBox("That's Cool!");
            },
            CancelButtonText = "ok",
        });
    }
}