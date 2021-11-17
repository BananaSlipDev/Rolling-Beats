using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<string> texts;

    private TextMeshProUGUI textTMP;
    [SerializeField] private GameObject controlKeys;
    [SerializeField] private GameObject scoreAndCombo;
    private int textIndex;

    [SerializeField] private float textWaitSeconds = 3;

    void Start()
    {
        textTMP = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        controlKeys.SetActive(false);
        scoreAndCombo.SetActive(false);

        texts = new List<string>();
        texts.Add("Welcome to the tutorial!");
        texts.Add("Use the inputs shown above...");
        texts.Add("to press the notes in the right order and score a beat!");
        texts.Add("Come on, prove it!");
        texts.Add(" "); texts.Add(" "); texts.Add(" ");  // 9 seconds

        texts.Add("There are two lanes. Be careful!");
        texts.Add("Your accuracy determines your points");
        texts.Add("100 for perfect, 50 for great, 0 for miss!");
        texts.Add("The combo will help you score higher...");
        texts.Add("but don't loose it! A miss will reset it to one");
        texts.Add("Here, try something more");
        texts.Add(" "); texts.Add(" "); texts.Add(" "); texts.Add(" "); // 20 seconds

        texts.Add("By the way, there are long notes too!");
        texts.Add("Hold the control until the end to score a beat");
        texts.Add("Time to try!");


        texts.Add("Good luck, have fun!");

        textIndex = 0;

        StartCoroutine(ShowTutorialText());
    }

    public IEnumerator ShowTutorialText()
    {
        while(textIndex < texts.Count)
        {
            switch(textIndex)
            {
                case 1:
                    controlKeys.SetActive(true);
                    break;
                case 3:
                    controlKeys.SetActive(false);
                    break;
                case 8:
                    controlKeys.SetActive(false);
                    scoreAndCombo.SetActive(true);
                    break;
                case 12:
                    scoreAndCombo.SetActive(false);
                    break;
                
            }

            textTMP.SetText(texts[textIndex]);
            textIndex++;
            yield return new WaitForSeconds(textWaitSeconds);
        }

    }

}
