using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [Header("Background Sprites")]
    public Sprite[] backgrounds;
    public string[] backgroundNames;

    [Header("Portrait Sprites")]
    public Sprite[] portraits;
    public string[] portraitNames;

    [Header("UI Elements")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject choicePanel;
    public GameObject choiceButtonPrefab;

    [Header("Character Visuals")]
    public Image characterPortrait;
    public Image backgroundImage;

    [Header("Settings")]
    public float textSpeed = 0.03f;

    private DialogueRunner dialogueRunner;
    private Coroutine typingCoroutine;

    void Awake()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
        dialogueRunner.AddCommandHandler("goto_ending", GoToEnding);
        dialogueRunner.AddCommandHandler("goto_menu", GoToMenu);
        dialogueRunner.AddCommandHandler<string>("background", SetBackground);
        dialogueRunner.AddCommandHandler<string>("portrait", SetPortrait);
    }

    public void SetBackground(string bgName)
    {
        StartCoroutine(FadeBackground(bgName));
    }

    private IEnumerator FadeBackground(string bgName)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color color = backgroundImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            backgroundImage.color = color;
            yield return null;
        }

        for (int i = 0; i < backgroundNames.Length; i++)
        {
            if (backgroundNames[i] == bgName)
            {
                backgroundImage.sprite = backgrounds[i];
                break;
            }
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            backgroundImage.color = color;
            yield return null;
        }

        color.a = 1f;
        backgroundImage.color = color;
    }

    public void SetPortrait(string portraitName)
    {
        if (portraitName == "none")
        {
            characterPortrait.gameObject.SetActive(false);
            return;
        }

        // Posisi karakter berdasarkan nama
        RectTransform rt = characterPortrait.GetComponent<RectTransform>();
        if (portraitName.StartsWith("aloha"))
            rt.anchoredPosition = new Vector2(400, 100);  // kanan
        else
            rt.anchoredPosition = new Vector2(-400, 100); // kiri

        for (int i = 0; i < portraitNames.Length; i++)
        {
            if (portraitNames[i] == portraitName)
            {
                characterPortrait.sprite = portraits[i];
                characterPortrait.gameObject.SetActive(true);
                return;
            }
        }
        Debug.LogWarning("Portrait tidak ditemukan: " + portraitName);
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
    }

    public void GoToEnding()
    {
        GameSceneManager.Instance.GoToEnding();
    }

    public void GoToMenu()
    {
        GameSceneManager.Instance.GoToMainMenu();
    }

    public void ShowLine(string characterName, string line)
    {
        characterNameText.text = characterName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line));
    }

    private IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void ShowChoices(string[] choices, System.Action<int> onChoiceSelected)
    {
        Debug.Log("ShowChoices dipanggil! Jumlah pilihan: " + choices.Length);
        //choicePanel.SetActive(true);
        choicePanel.SetActive(true);

        foreach (Transform child in choicePanel.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < choices.Length; i++)
        {
            int index = i;
            GameObject btn = Instantiate(choiceButtonPrefab, choicePanel.transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                onChoiceSelected(index);
                choicePanel.SetActive(false);
            });
        }
    }

    public void ChangeBackground(Sprite newBackground)
    {
        backgroundImage.sprite = newBackground;
    }

    public void ChangePortrait(Sprite newPortrait)
    {
        characterPortrait.sprite = newPortrait;
        characterPortrait.gameObject.SetActive(newPortrait != null);
    }
}