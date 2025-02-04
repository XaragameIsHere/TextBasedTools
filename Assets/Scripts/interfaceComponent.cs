
/// <summary>
/// this is the componenent of the UI that controls the dialogue in game
/// ye
/// </summary>
/// 
[System.Serializable]
public class DialogueParsing
{
    [System.Serializable]
    public class DialogueLine
    {
        public string sprite;
        public string dialogueText;
    }


    [System.Serializable]
    public class DialogueData
    {
        public DialogueLine[] start;
        public DialogueLine[] end;
    }

    [System.Serializable]
    public class Choice
    {
        public string reactionText;
        public string[] reaction;
        public string nextDialogueSelection;
    }

    [System.Serializable]
    public class Selection
    {
        public string name;
        public DialogueLine[] context;
        public Choice[] choices;
    }

    [System.Serializable]
    public class Dialogue
    {
        public DialogueData[] cutsceneDialogue;
        public Selection[] actionSelections;
    }


}

