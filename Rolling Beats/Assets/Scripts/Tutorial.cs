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

    [SerializeField] private float textWaitSeconds = 4;

    // Start is called before the first frame update
    void Start()
    {
        textTMP = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        controlKeys.SetActive(false);
        scoreAndCombo.SetActive(false);

        texts = new List<string>();
        texts.Add("Welcome to the tutorial!");
        texts.Add("Use the inputs shown above...");
        texts.Add("to press the notes in the right order and score a beat!");
        texts.Add("There are two lanes. Be careful!");
        texts.Add("Your accuracy determines your points");
        texts.Add("100 for perfect, 50 for great, 0 for miss!");
        texts.Add("The combo will help you score higher...");
        texts.Add("but don't loose it! A miss will reset it to one");
        texts.Add("Are you ready to play?");
        texts.Add("Let's roll that beats!");

        textIndex = 0;

        StartCoroutine(ShowTutorialText());
    }

    public IEnumerator ShowTutorialText()
    {
        while(textIndex < texts.Count)
        {
            if (textIndex == 1)
                controlKeys.SetActive(true);
            else if (textIndex == 4)
            {
                controlKeys.SetActive(false);
                scoreAndCombo.SetActive(true);
            }
            else if(textIndex == 8)
                scoreAndCombo.SetActive(false);


            textTMP.SetText(texts[textIndex]);
            textIndex++;
            yield return new WaitForSeconds(textWaitSeconds);
        }

    }

}
