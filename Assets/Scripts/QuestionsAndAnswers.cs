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
    public int RandomIndexQuest1, RandomIndexQuest2, RandomAnswers;


    public void QuestionsButtonsActive()
    {
        AnswersTMP.text = "...";
        QuestionsButton.interactable = true;
        Question1Button.interactable = true;
        Question2Button.interactable = true;
    }

    public void Questions()
    {
        // Birinci soruyu string dizisi i�inden rastgele �eker ve yazd�r�r.
        QuestionOne = new string[] { "Nereden geliyorsunuz?", "Nereye gidiyorsunuz?", "Burada ne i�iniz var?" };
        int RandomQuestion1 = Random.Range(0, QuestionOne.Length);
        PlayerPrefs.SetInt("FirstQuestion", RandomQuestion1);
        Question1TMP.text = QuestionOne[RandomQuestion1];
    

        // �kinci soruyu string dizisi i�inden rastgele �eker ve yazd�r�r.
        QuestionTwo = new string[] { "Ne i� yap�yorsunuz?", "Tek mi ya��yorsunuz?", "�al���yor musunuz?" };
        int RandomQuestion2 = Random.Range(0, QuestionTwo.Length);
        PlayerPrefs.SetInt("SecondQuestion", RandomQuestion2);
        Question2TMP.text = QuestionTwo[RandomQuestion2];
       

        QuestionsButton.interactable = false;
    }

    public void FirstAnswers()
    {
        int RandomAnswers = Random.Range(0, QuestionOne.Length);
        Question1Button.interactable = false;
        switch (PlayerPrefs.GetInt("FirstQuestion")) // Sorulan sorudan gelen index'e g�re sorulan soruyla alakal� cevaplar string dizisinden rastgele �ekilir.
        {
            case 0:
                AnswerOne = new string[] { "Evde bunald�m, hava almaya ��kt�m.", "��ten erken ��kt�m.", "Aileme ziyarete gidiyorum." };
                AnswersTMP.text = AnswerOne[RandomAnswers];
                break;
            case 1:
                AnswerOne = new string[] { "Evde bunal�nca y�r�y��e ��kay�m dedim.", "��ten erken ��kt�m, eve gidiyorum.", "Ailemin yan�na ziyarete gidiyorum." };
                AnswersTMP.text = AnswerOne[RandomAnswers];
                break;
            case 2:
                AnswerOne = new string[] { "Bir i�im yok, �yle dola��yorum.", "Bir i�im yok, evime gidiyorum.", "Ailemin yan�na gidiyordum, beni durdurdunuz." };
                AnswersTMP.text = AnswerOne[RandomAnswers];
                break;
        }
    }

    public void SecondAnswers()
    {
        int RandomAnswers = Random.Range(0, QuestionOne.Length);
        Question2Button.interactable = false;
        switch (PlayerPrefs.GetInt("SecondQuestion")) // Sorulan sorudan gelen index'e g�re sorulan soruyla alakal� cevaplar string dizisinden rastgele �ekilir.
        {
            case 0:
                AnswerTwo = new string[] { "Oyun geli�tiricisiyim. Kendi oyunlar�m� yap�yorum.", "Kendi i�imin patronuyum. Oyun yap�yorum.", "Yaz�l�m i�leriyle u�ra��yorum." };
                AnswersTMP.text = AnswerTwo[RandomAnswers];
                break;
            case 1:
                AnswerTwo = new string[] { "Sevgilimle birlikte ya��yorum.", "Hay�r, ailemle birlikte ya��yorum.", "Evet, tek ya��yorum." };
                AnswersTMP.text = AnswerTwo[RandomAnswers];
                break;
            case 2:
                AnswerTwo = new string[] { "Ziraat m�hendisiyim.", "Bankada �al���yorum.", "Evet, lisedeki ergenlere ��retmenlik yap�yorum." };
                AnswersTMP.text = AnswerTwo[RandomAnswers];
                break;
        }
    }
}
