using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsAndAnswers : MonoBehaviour
{
    string[] QuestionOne;
    string[] QuestionTwo;
    string[] AnswerOne;
    string[] AnswerTwo;

    public TextMeshProUGUI Question1TMP, Question2TMP, AnswersTMP;
    public Button QuestionsButton, Question1Button, Question2Button;


    public void Questions()
    {
        // Birinci soruyu string dizisi içinden rastgele çeker ve yazdýrýr.
        QuestionOne = new string[] { "Nereden geliyorsunuz?", "Nereye gidiyorsunuz?", "Burada ne iþiniz var?" };
        int RandomQuestion1 = Random.Range(0, QuestionOne.Length);
        PlayerPrefs.SetInt("FirstQuestion", RandomQuestion1);
        Question1TMP.text = QuestionOne[RandomQuestion1];
        Question1Button.interactable = true;

        // Ýkinci soruyu string dizisi içinden rastgele çeker ve yazdýrýr.
        QuestionTwo = new string[] { "Ne iþ yapýyorsunuz?", "Tek mi yaþýyorsunuz?", "Çalýþýyor musunuz?" };
        int RandomQuestion2 = Random.Range(0, QuestionOne.Length);
        PlayerPrefs.SetInt("SecondQuestion", RandomQuestion2);
        Question2TMP.text = QuestionTwo[RandomQuestion2];
        Question2Button.interactable = true;
    }

    public void FirstAnswers()
    {
        int RandomAnswer = Random.Range(0, QuestionOne.Length);
        Question1Button.interactable = false;
        switch (PlayerPrefs.GetInt("FirstQuestion")) // Sorulan sorudan gelen index'e göre sorulan soruyla alakalý cevaplar string dizisinden rastgele çekilir.
        {
            case 0:
                AnswerOne = new string[] { "Evde bunaldým, hava almaya çýktým.", "Ýþten erken çýktým.", "Aileme ziyarete gidiyorum." };
                AnswersTMP.text = AnswerOne[RandomAnswer];
                break;
            case 1:
                AnswerOne = new string[] { "Evde bunalýnca yürüyüþe çýkayým dedim.", "Ýþten erken çýktým, eve gidiyorum.", "Ailemin yanýna ziyarete gidiyorum." };
                AnswersTMP.text = AnswerOne[RandomAnswer];
                break;
            case 2:
                AnswerOne = new string[] { "Bir iþim yok, öyle dolaþýyorum.", "Bir iþim yok, evime gidiyorum.", "Ailemin yanýna gidiyordum, beni durdurdunuz." };
                AnswersTMP.text = AnswerOne[RandomAnswer];
                break;
        }
    }

    public void SecondAnswers()
    {
        int RandomAnswer = Random.Range(0, QuestionTwo.Length);
        Question2Button.interactable = false;
        switch (PlayerPrefs.GetInt("SecondQuestion")) // Sorulan sorudan gelen index'e göre sorulan soruyla alakalý cevaplar string dizisinden rastgele çekilir.
        {
            case 0:
                AnswerTwo = new string[] { "Oyun geliþtiricisiyim. Kendi oyunlarýmý yapýyorum.", "Kendi iþimin patronuyum. Oyun yapýyorum.", "Yazýlým iþleriyle uðraþýyorum." };
                AnswersTMP.text = AnswerTwo[RandomAnswer];
                break;
            case 1:
                AnswerTwo = new string[] { "Sevgilimle birlikte yaþýyorum.", "Hayýr, ailemle birlikte yaþýyorum.", "Evet, tek yaþýyorum." };
                AnswersTMP.text = AnswerTwo[RandomAnswer];
                break;
            case 2:
                AnswerTwo = new string[] { "Ziraat mühendisiyim.", "Bankada çalýþýyorum.", "Evet, lisedeki ergenlere öðretmenlik yapýyorum." };
                AnswersTMP.text = AnswerTwo[RandomAnswer];
                break;
        }
    }
}
