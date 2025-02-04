using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class dialogueLibrary : MonoBehaviour
{

    public bool dev_SkipCutscene = false;
    public bool inSelection = false;
    private DialogueParsing.DialogueData mainData;

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
        
        textBox.text = text;
    }
    
    
    public IEnumerator Dialogue(DialogueParsing.Selection selection)
    {
        foreach (DialogueParsing.DialogueLine line in selection.context)
        {
            StartCoroutine( TypeWrite(line.dialogueText, playerText));
            
            playerTextEnter.enabled = true;
			
            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
            playerTextEnter.enabled = false;

			playerText.text = "";
		}
        
        dialogueChoiceBox.transform.DOLocalMoveY(-300, 1);
        isClicked = false;
        //print("moving box of choices" );
        yield return new WaitUntil(() => isClicked);
        isClicked = false;
        //print("player has chosen");
        dialogueChoiceBox.transform.DOLocalMoveY(-817, 1);

        StartCoroutine( TypeWrite(selection.choices[clickedButton].reactionText, playerText));
        
        
	}

    
    
    private bool isClicked = false;
    int clickedButton;
    public void click(int s) 
    {
        clickedButton = s;
        isClicked = true;
    }

    /*
     
    Important, stuff for typewriter effect
    
    
    
    private Coroutine dialogueRoutine;
    private IEnumerator typeWrite(DialogueParsing.Dialogue dialogueRoot, DialogueParsing.Selection line)
    {

        

        //print("finished typewriting "+ line.enemy_Text);
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        //print("finished creating choices " );
        for (int i = 0; i < 4; i++)
        {
            choiceButtons[i].text = line.choices[i].dialogueLine;
        }

        dialogueChoiceBox.transform.DOLocalMoveY(-300, 1);
        isClicked = false;
        //print("moving box of choices" );
        yield return new WaitUntil(() => isClicked);
        isClicked = false;
        //print("player has chosen");
        dialogueChoiceBox.transform.DOLocalMoveY(-817, 1);

        for (int i = 1; i <= line.choices[clickedButton].dialogueLine.Length; i++)
        {
            playerText.text = line.choices[clickedButton].dialogueLine.Substring(0, i);

            yield return new WaitForSeconds(dialogueSpeed);
        }

        //print("finished typewriting " + line.choices[clickedButton].dialogueLine);
        yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
        //print("moving on");

        if (line.choices[clickedButton].next_Dialogue_Selection == "end")
        {
            patienceMeter.transform.DOLocalMoveX(-1309, 1);
            StartCoroutine( dialogue(mainData.End));
        }
        else
        {
            navigateToSelection(dialogueRoot, line.choices[clickedButton].next_Dialogue_Selection);
        }
            
        
    }
*/
    public void navigateToSelection(DialogueParsing.Dialogue dialogueRoot, string nameOfSelection)
    {
        print(nameOfSelection);
        patienceMeter.transform.DOLocalMoveX(-714, 1);
        dialoguePlayer.transform.DOLocalMoveX(-880, 1);



        foreach (DialogueParsing.Selection data in dialogueRoot.actionSelections)
        {

			if (data.name == nameOfSelection)
            {
                //print(data.Name);
                
                //dialogueRoutine = StartCoroutine(typeWrite(dialogueRoot, data));
            }

        }

    }

    /*
    private float patience = 5;
    public IEnumerator moveMeter()
    {
        patienceMeter.value = patience / 10;
        while (patience > 0)
        {
            yield return new WaitForSeconds(3);
            patience -= 1;
            patienceMeter.value = patience/10;
        }
        StopCoroutine(dialogueRoutine);

        stopDialogue();
    }*/
    
    private void stopDialogue()
	{
        print("end");
        /*
        patienceMeter.transform.DOLocalMoveX(-1309, 1);
        dialoguePlayer.transform.DOMoveX(-1712, 1);
        dialogueEnemy.transform.DOMoveX(-1712, 1);
        dialogueChoiceBox.transform.DOLocalMoveY(-781, 1);
        zoomIn.transform.DOLocalMoveY(1635, 1);
        */
        
	}

}
