using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class NumerosFinal : MonoBehaviour
{
    public static bool triggerAlvo = false;
    public static int coletado;
    public TextMeshProUGUI nColetado;
    public TextMeshProUGUI nTempo;
    public TextMeshProUGUI nBonus;
    public TextMeshProUGUI nTotal;
    public TextMeshProUGUI nNoBonus;
    public Transform _Coletado;
    public Transform _Tempo;
    public Transform _Bonus;
    public Transform _Total;
    public Transform _Nota;
    public Transform _NotaC;
    public Transform _NotaB;
    public Transform _TryAgain;
    public Transform _NoBonus;
    public Transform tempoPonto200;
    public Transform tempoPonto100;
    public Transform tempoPonto0;
    public Image notaImg;
    public Image notaCimg;
    public Image notaBimg;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WhichNumber());
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }

    IEnumerator WhichNumber()
    {
        yield return new WaitForSeconds(1);

        _Coletado.DOLocalMoveX(479, 1.5f);
        nColetado.SetText(coletado.ToString("0"));

        yield return new WaitForSeconds(1);

        _Tempo.DOLocalMoveX(300, 1.5f);
        float minutes = Mathf.FloorToInt(Timer.timeLeft / 60);
        float seconds = Mathf.FloorToInt(Timer.timeLeft % 60);
        nTempo.SetText("{0:00} : {1:00}", minutes, seconds);
        float pontosTempo = 0;
        if(Timer.timeLeft < 60)
        {
            pontosTempo = 200;
            tempoPonto200.DOLocalMoveX(578, 2f);
        }
        else if(Timer.timeLeft > 60 && Timer.timeLeft < 120)
        {
            pontosTempo = 100;
            tempoPonto100.DOLocalMoveX(578, 2f);
        }
        else
        {
            pontosTempo = 0;
            tempoPonto0.DOLocalMoveX(578, 2f);
        }

        yield return new WaitForSeconds(1);

        int pontosBonus = 0;
        if (triggerAlvo)
        {
            _Bonus.DOLocalMoveX(278, 1.5f);
            nBonus.SetText("+100");
            pontosBonus = 100;
        }
        else
        {
            _NoBonus.DOLocalMoveX(278, 1.5f);
            nNoBonus.SetText("0");
            pontosBonus = 0;
        }
        

        yield return new WaitForSeconds(1);

        _Total.DOLocalMoveX(252, 1.5f);
        float total = coletado + pontosBonus + pontosTempo;
        nTotal.SetText(total.ToString("0"));

        yield return new WaitForSeconds(1);
        if(pontosTempo == 0 && triggerAlvo == false || coletado < 300)
        {
            DOTween.Sequence().Join(_NotaC.DOScale(3, 1.5f)).Join(notaCimg.DOFade(1, 1.5f));
        }
        else if(triggerAlvo && pontosTempo == 0 || pontosTempo == 100 && triggerAlvo || coletado > 1000)
        {
            DOTween.Sequence().Join(_NotaB.DOScale(3, 1.5f)).Join(notaBimg.DOFade(1, 1.5f));
        }
        else if(pontosTempo >= 200 && triggerAlvo || coletado > 1500)
        {
            DOTween.Sequence().Join(_Nota.DOScale(3, 1.5f)).Join(notaImg.DOFade(1, 1.5f));
        }
        

        yield return new WaitForSeconds(2);

        _TryAgain.DOLocalMoveY(-851, 1);




    }
}
