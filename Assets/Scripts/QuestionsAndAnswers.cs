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

    GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }
    public void QuestionsButtonsActive()
    {
        AnswersTMP.text = "...";
        QuestionsButton.interactable = true;
        Question1Button.interactable = true;
        Question2Button.interactable = true;
    }

    public void IsOpening(bool isActive)
    {
        if (!isActive)
        {
            Question1Button.gameObject.SetActive(false);
            Question2Button.gameObject.SetActive(false);
        }
        if (isActive)
        {
            Question1Button.gameObject.SetActive(true);
            Question2Button.gameObject.SetActive(true);
        }
    }


    public void Questions()
    {
        // Birinci soruyu string dizisi i�inden rastgele �eker ve yazd�r�r.
        QuestionOne = new string[] { "Nereden geliyorsunuz?", "Nereye gidiyorsunuz?", "Burada ne i�iniz var?" };
        RandomIndexQuest1 = Random.Range(0, QuestionOne.Length);
        PlayerPrefs.SetInt("FirstQuestion", RandomIndexQuest1);
        Question1TMP.text = QuestionOne[RandomIndexQuest1];
    
        // �kinci soruyu string dizisi i�inden rastgele �eker ve yazd�r�r.
        QuestionTwo = new string[] { "Ne i� yap�yorsunuz?", "Tek mi ya��yorsunuz?", "�al���yor musunuz?" };
        RandomIndexQuest2 = Random.Range(0, QuestionTwo.Length);
        PlayerPrefs.SetInt("SecondQuestion", RandomIndexQuest2);
        Question2TMP.text = QuestionTwo[RandomIndexQuest2];

        RandomAnswers = Random.Range(0,4);
        QuestionsButton.interactable = false;
    }

    public void FirstAnswers()
    {
        Question1Button.interactable = false;
        if (!gameManager.isCriminal)
        {
            switch (PlayerPrefs.GetInt("FirstQuestion")) // Sorulan sorudan gelen index'e g�re sorulan soruyla alakal� cevaplar string dizisinden rastgele �ekilir.
            {
                case 0:
                    AnswerOne = new string[] { "Evde bunald�m, hava almaya ��kt�m.", "Evde bunal�nca y�r�y��e ��kay�m dedim.", "Ne yapacaks�n�z bu bilgiyi?", "��ten erken ��kt�m, eve gidiyorum." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
                case 1:
                    AnswerOne = new string[] { "Sevgilimle bulu�maya gidiyorum.", "Arkada��mla bulu�ma ayarlad�m. Oraya gidiyorum.", "Ailemin yan�na ziyarete gidiyorum.", "Eve gidiyorum arkada��m." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
                case 2:
                    AnswerOne = new string[] { "Sizi ilgilendirmiyor san�r�m.", "�ylee, dola��yorum.", "Ailemin yan�na gidiyordum, beni durdurdunuz.", "Arkada��m�n evinde misafirim." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
            }
        }
        if (gameManager.isCriminal)
        {
            switch (PlayerPrefs.GetInt("FirstQuestion")) // Sorulan sorudan gelen index'e g�re sorulan soruyla alakal� cevaplar string dizisinden rastgele �ekilir.
            {
                case 0:
                    AnswerOne = new string[] { "��ten g��ten s�k�ld�m kendimi d��ar� att�m.", "Evde bunal�nca y�r�y��e ��kay�m dedim.", "Sana ne arkada�?", "Senin ne haddine bunu sormak?." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
                case 1:
                    AnswerOne = new string[] { "Sevgilimle bulu�maya gidiyorum.", "Ticaret yapmaya gidiyorum dostum.", "Arkada� ziyareti.", "Eve gidiyorum arkada��m." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
                case 2:
                    AnswerOne = new string[] { "Seni ilgilendirmez!", "�ylee, dola��yorum.", "Hay�rd�r benim derdimi mi ��zeceksin?", "Misafirim burada." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
            }
        }
    }

    public void SecondAnswers()
    {
        Question2Button.interactable = false;
        if (!gameManager.isCriminal)
        {
            switch (PlayerPrefs.GetInt("SecondQuestion")) // Sorulan sorudan gelen index'e g�re sorulan soruyla alakal� cevaplar string dizisinden rastgele �ekilir.
            {
                case 0:
                    AnswerTwo = new string[] { "Oyun geli�tiricisiyim. Kendi oyunlar�m� yap�yorum.", "Bir i�im yok.", "Kendi i�imin patronuyum. Oyun yap�yorum.", "Bankada �al���yorum." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
                case 1:
                    AnswerTwo = new string[] { "Evet, tek ya��yorum.", "Hay�r, ailemle birlikte ya��yorum.", "Sevgilimle birlikte ya��yorum.", "Arkada��mda kal�yorum." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
                case 2:
                    AnswerTwo = new string[] { "Neden, ne yapacaks�n�z?", "�u an bir i�te �al��m�yorum.", "Evet, lisedeki ergenlere ��retmenlik yap�yorum.", "Ziraat m�hendisiyim." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
            }
        }
        if (gameManager.isCriminal)
        {
            switch (PlayerPrefs.GetInt("SecondQuestion")) // Sorulan sorudan gelen index'e g�re sorulan soruyla alakal� cevaplar string dizisinden rastgele �ekilir.
            {
                case 0:
                    AnswerTwo = new string[] { "�llegal i�lerle u�ra�m�yorum.", "Bir i�im yok.", "Kendi i�imin patronuyum.", "��im ben de sakl� kals�n." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
                case 1:
                    AnswerTwo = new string[] { "Evet, tek ya��yorum.", "Ne yapacaks�n arkada��m?", "Sevgilimle birlikte ya��yorum.", "Arkada�larla birlikte kal�yoruz. Sorun mu var?" };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
                case 2:
                    AnswerTwo = new string[] { "Ekmek teknemde yuvarlan�p gidiyorum.", "�al��m�yorum bana arkada�lar bak�yor.", "Evet, gen�lere nas�l piyasa patronu olunur ��retiyorum.", "S�ylemek istemiyorum." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
            }
        }
    }
}
