using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class dialogueLibrary : MonoBehaviour
{

    public bool dev_SkipCutscene = false;
    public bool inSelection = false;
    public TextAsset jsonFile;
    private DialogueParsing.DialogueData mainData;
    [HideInInspector] public DialogueParsing.Dialogue dialogueRoot;

	public RawImage dialogueChoiceBox;
    public Slider patienceMeter;
    public List<TMP_Text> choiceButtons;

    public Image dialoguePlayer;

	public TMP_Text playerText;
    [SerializeField] float dialogueSpeed = .02f;
    [SerializeField] Image playerTextEnter;
    [SerializeField] float camSize;
    [SerializeField] UnityEvent functions;

    private IEnumerator TypeWrite(string text, TMP_Text textBox)
    {
        for (int i = 1; i <= text.Length; i++)
        {
            textBox.text = text.Substring(0, i);
                
            yield return new WaitForSeconds(dialogueSpeed);
        }
        
        playerTextEnter.enabled = true;
        textBox.text = text;
    }
    
    private bool isClicked = false;
    int clickedButton;
    
    public IEnumerator Dialogue(DialogueParsing.Selection selection)
    {
         yield return TypeWrite(selection.context, playerText);
        
		
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        playerTextEnter.enabled = false;

		playerText.text = "";

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].text = selection.choices[i].dialogueLine;
        }
        
        dialogueChoiceBox.transform.DOLocalMoveX(600, 1);
        isClicked = false;
        //print("moving box of choices" );
        yield return new WaitUntil(() => isClicked);
        isClicked = false;
        print(clickedButton);
        dialogueChoiceBox.transform.DOLocalMoveX(1300, 1);
        
        yield return TypeWrite(selection.choices[clickedButton].reactionText, playerText);
        
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        playerTextEnter.enabled = false;

        if (selection.choices[clickedButton].nextDialogueSelection == "end")
        {
            StartCoroutine(Scene(dialogueRoot.cutsceneDialogue[0].end, "end"));
        }
        else if (selection.choices[clickedButton].nextDialogueSelection == "BadEnd")
        {
            
            StartCoroutine(Scene(dialogueRoot.cutsceneDialogue[0].end, "BadEnd"));
        }
        else
        {
            navigateToSelection(selection.choices[clickedButton].nextDialogueSelection);
        }
        
	}
    
    public IEnumerator Scene(DialogueParsing.DialogueLine[] sceneData, string sceneName)
    {
        if (sceneName != "BadEnd")
        {
            foreach (DialogueParsing.DialogueLine line in sceneData)
            {
                yield return TypeWrite(line.dialogueText, playerText);
            
			
                yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
                playerTextEnter.enabled = false;

                playerText.text = "";
            
            
            }
        }
        

        if (sceneName == "start")
        {
            
            navigateToSelection("SelectionStart");
            yield break;
        }
        
        if (sceneName == "end")
        {
            yield return TypeWrite("It's Finally Over. Life Goes on. Better decisions will be made.", playerText);
            
        }
        else if (sceneName == "BadEnd")
        {
            yield return TypeWrite("GAME OVER", playerText);
        }

        playerTextEnter.enabled = true;
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        playerTextEnter.enabled = false;

        playerText.text = "";
        StartCoroutine( TypeWrite("Play Again?", playerText));
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        
        dialogueRoot = JsonUtility.FromJson<DialogueParsing.Dialogue>(jsonFile.text);
        print(dialogueRoot.cutsceneDialogue[0].start);
        StartCoroutine(Scene(dialogueRoot.cutsceneDialogue[0].start, "start"));
    }

    public void click(int s) 
    {
        clickedButton = s;
        isClicked = true;
    }

    
    public void navigateToSelection(string nameOfSelection)
    {
        print(nameOfSelection);

        foreach (DialogueParsing.Selection data in dialogueRoot.actionSelections)
        {

			if (data.name == nameOfSelection)
            {
                print(data.name);
                
                StartCoroutine(Dialogue(data));
            }

        }

    }

    
}
