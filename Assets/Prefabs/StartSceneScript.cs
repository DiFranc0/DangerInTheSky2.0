using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartSceneScript : MonoBehaviour
{
    public Image txtDangerInTheSky;
    public TextMeshProUGUI ludensCopy;
    public Animator anStartScreen;
    public Transform tStartButton;
    AudioSource musicaInicial;
    bool triggerStart = false;
    // Start is called before the first frame update
    void Start()
    {
        musicaInicial = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && triggerStart == false)
        {
            anStartScreen.SetTrigger("pTouch");

            StartCoroutine(FadeIn());
            triggerStart = true;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

   

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1);
        musicaInicial.Play();
        
        txtDangerInTheSky.DOFade(1, 2);

        yield return new WaitForSeconds(1);

        tStartButton.DOLocalMoveY(-718, 1);

        yield return new WaitForSeconds(1);

        ludensCopy.DOFade(1, 1);


    }
}
