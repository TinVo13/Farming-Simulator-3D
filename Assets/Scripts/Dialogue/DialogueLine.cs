using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueLine
{
    public string speaker;
    [TextArea(2,5)]
    public string message;

    public DialogueLine(string speaker, string message)
    {
        this.speaker = speaker;
        this.message = message;
    }
}
