using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HangmanController : MonoBehaviour
{

    [SerializeField] GameObject letterContainer;
    [SerializeField] GameObject keyboardButton;
    [SerializeField] GameObject keyboardContainer;
    [SerializeField] GameObject wordContainer;
    [SerializeField] GameObject[] hangmanStages;
    [SerializeField] TextAsset posibbleWords;

    private string word;
    private int incorrectGuesses, correctGuesses;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        InitialiseGame();
        InitializeButtons();
    }
    void InitialiseGame()
    {
        incorrectGuesses = 0;
        correctGuesses = 0;
        foreach (Button child in keyboardContainer.GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }
        foreach (Transform child in wordContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (GameObject stage in hangmanStages)
        {
            stage.SetActive(false);
        }
        word = GenerateWord().ToUpper();
        foreach (char letter in word)
        {
            Instantiate(letterContainer, wordContainer.transform);
        }
    }
    private string GenerateWord()
    {
        string[] wordlist = posibbleWords.text.Split("\n");
        string line = wordlist[Random.Range(0, wordlist.Length)];
        return line.Substring(0, line.Length - 1);
    }

    void InitializeButtons()
    {
        for (int i = 65; i <= 90; i++)
        {
            CreateButton(i);
        }
    }
    void CreateButton(int i)
    {
        GameObject temp = Instantiate(keyboardButton, keyboardContainer.transform);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = ((char)i).ToString();
        temp.GetComponent<Button>().onClick.AddListener(delegate { CheckLetter(((char)i).ToString()); });
    }
    void CheckLetter(string inputLetter)
    {
        bool letterInword = false;
        for (int i = 0; i < word.Length; i++)
        {
            if (inputLetter == word[i].ToString())
            {
                letterInword = true;
                correctGuesses++;
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].text = inputLetter;
            }
        }

        if (letterInword == false)
        {
            incorrectGuesses++;
            hangmanStages[incorrectGuesses - 1].SetActive(true);
        }
        CheckOutcome();

    }
    void CheckOutcome()
    {
        if (correctGuesses == word.Length)
        {
            for (int i = 0; i < word.Length; i++)
            {
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.green;

            }
            Invoke("InitialiseGame", 5f);
        }



        if (incorrectGuesses == hangmanStages.Length)
        {
            for (int i = 0; i < word.Length; i++)
            {
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.red;
                wordContainer.GetComponentsInChildren<TextMeshProUGUI>()[i].text = word[i].ToString();
            }
            Invoke("InitialiseGame", 3f);
        }
    }
}
