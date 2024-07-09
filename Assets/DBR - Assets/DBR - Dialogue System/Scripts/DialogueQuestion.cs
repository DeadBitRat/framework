using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[Serializable]
public class DialogueQuestion
{
    // Optional: You might still want to include a question text or ID if it fits your design
    // public string questionID;
    // public string questionText;

    public DialogueOption[] options;  // Using an array to hold options

    public DialogueQuestion()  // Constructor to initialize the options array if necessary
    {
        options = new DialogueOption[5];  // Assuming you want exactly 5 options
        for (int i = 0; i < options.Length; i++)
        {
            options[i] = new DialogueOption();
        }
    }
}

[Serializable]
public class DialogueOption
{
    public string optionText;
    public UnityEvent optionEvent;  // Ensure this is initialized properly to avoid null reference issues
    public int nextLineIndex;  // Index of the next dialogue line if this option is selected

    public DialogueOption()
    {
        optionEvent = new UnityEvent();  // Initialization of UnityEvent to prevent null references
    }
}


