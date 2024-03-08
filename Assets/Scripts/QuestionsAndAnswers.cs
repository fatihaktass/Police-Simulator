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
        // Birinci soruyu string dizisi içinden rastgele çeker ve yazdýrýr.
        QuestionOne = new string[] { "Nereden geliyorsunuz?", "Nereye gidiyorsunuz?", "Burada ne iþiniz var?" };
        RandomIndexQuest1 = Random.Range(0, QuestionOne.Length);
        PlayerPrefs.SetInt("FirstQuestion", RandomIndexQuest1);
        Question1TMP.text = QuestionOne[RandomIndexQuest1];
    
        // Ýkinci soruyu string dizisi içinden rastgele çeker ve yazdýrýr.
        QuestionTwo = new string[] { "Ne iþ yapýyorsunuz?", "Tek mi yaþýyorsunuz?", "Çalýþýyor musunuz?" };
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
            switch (PlayerPrefs.GetInt("FirstQuestion")) // Sorulan sorudan gelen index'e göre sorulan soruyla alakalý cevaplar string dizisinden rastgele çekilir.
            {
                case 0:
                    AnswerOne = new string[] { "Evde bunaldým, hava almaya çýktým.", "Evde bunalýnca yürüyüþe çýkayým dedim.", "Ne yapacaksýnýz bu bilgiyi?", "Ýþten erken çýktým, eve gidiyorum." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
                case 1:
                    AnswerOne = new string[] { "Sevgilimle buluþmaya gidiyorum.", "Arkadaþýmla buluþma ayarladým. Oraya gidiyorum.", "Ailemin yanýna ziyarete gidiyorum.", "Eve gidiyorum arkadaþým." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
                case 2:
                    AnswerOne = new string[] { "Sizi ilgilendirmiyor sanýrým.", "Öylee, dolaþýyorum.", "Ailemin yanýna gidiyordum, beni durdurdunuz.", "Arkadaþýmýn evinde misafirim." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
            }
        }
        if (gameManager.isCriminal)
        {
            switch (PlayerPrefs.GetInt("FirstQuestion")) // Sorulan sorudan gelen index'e göre sorulan soruyla alakalý cevaplar string dizisinden rastgele çekilir.
            {
                case 0:
                    AnswerOne = new string[] { "Ýþten güçten sýkýldým kendimi dýþarý attým.", "Evde bunalýnca yürüyüþe çýkayým dedim.", "Sana ne arkadaþ?", "Senin ne haddine bunu sormak?." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
                case 1:
                    AnswerOne = new string[] { "Sevgilimle buluþmaya gidiyorum.", "Ticaret yapmaya gidiyorum dostum.", "Arkadaþ ziyareti.", "Eve gidiyorum arkadaþým." };
                    AnswersTMP.text = AnswerOne[RandomAnswers];
                    break;
                case 2:
                    AnswerOne = new string[] { "Seni ilgilendirmez!", "Öylee, dolaþýyorum.", "Hayýrdýr benim derdimi mi çözeceksin?", "Misafirim burada." };
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
            switch (PlayerPrefs.GetInt("SecondQuestion")) // Sorulan sorudan gelen index'e göre sorulan soruyla alakalý cevaplar string dizisinden rastgele çekilir.
            {
                case 0:
                    AnswerTwo = new string[] { "Oyun geliþtiricisiyim. Kendi oyunlarýmý yapýyorum.", "Bir iþim yok.", "Kendi iþimin patronuyum. Oyun yapýyorum.", "Bankada çalýþýyorum." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
                case 1:
                    AnswerTwo = new string[] { "Evet, tek yaþýyorum.", "Hayýr, ailemle birlikte yaþýyorum.", "Sevgilimle birlikte yaþýyorum.", "Arkadaþýmda kalýyorum." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
                case 2:
                    AnswerTwo = new string[] { "Neden, ne yapacaksýnýz?", "Þu an bir iþte çalýþmýyorum.", "Evet, lisedeki ergenlere öðretmenlik yapýyorum.", "Ziraat mühendisiyim." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
            }
        }
        if (gameManager.isCriminal)
        {
            switch (PlayerPrefs.GetInt("SecondQuestion")) // Sorulan sorudan gelen index'e göre sorulan soruyla alakalý cevaplar string dizisinden rastgele çekilir.
            {
                case 0:
                    AnswerTwo = new string[] { "Ýllegal iþlerle uðraþmýyorum.", "Bir iþim yok.", "Kendi iþimin patronuyum.", "Ýþim ben de saklý kalsýn." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
                case 1:
                    AnswerTwo = new string[] { "Evet, tek yaþýyorum.", "Ne yapacaksýn arkadaþým?", "Sevgilimle birlikte yaþýyorum.", "Arkadaþlarla birlikte kalýyoruz. Sorun mu var?" };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
                case 2:
                    AnswerTwo = new string[] { "Ekmek teknemde yuvarlanýp gidiyorum.", "Çalýþmýyorum bana arkadaþlar bakýyor.", "Evet, gençlere nasýl piyasa patronu olunur öðretiyorum.", "Söylemek istemiyorum." };
                    AnswersTMP.text = AnswerTwo[RandomAnswers];
                    break;
            }
        }
    }
}
