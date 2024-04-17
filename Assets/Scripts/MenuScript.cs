using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MenuScript : MonoBehaviour
{
    public Animator anMenu;
    public Button equipar;
    public GameObject equipado;
    public GameObject upado;
    public Button next;
    public Button previous;
    public Button comprar;
    public GameObject[] skins;
    public GameObject[] points;
    public TextMeshProUGUI preco;
    public TextMeshProUGUI precoVelocidade;
    public TextMeshProUGUI precoResistencia;
    public TextMeshProUGUI precoTempoReacao;
    public int selectedSkin = 0;
    Transform move;
    Transform move2;
    public RectTransform successQte;
    Sequence mySequence;
    public Vector3 scaleTo;
    Vector3 scaleInicial;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    bool comprado1 = false;
    bool comprado2 = false;
    bool equipado0 = false;
    bool equipado1 = false;
    bool equipado2 = false;
    public GameObject comprado;
    public GameObject[] melhoriasParticle;
    public AudioSource auMenu;
    public AudioClip[] menuClips;
    // Start is called before the first frame update
    void Start()
    {
        move = skins[selectedSkin].GetComponent<Transform>();
        scaleInicial = move.localScale;
        //melhoriasParticle = GameObject.FindGameObjectWithTag("ParticleMelhoria").GetComponent<ParticleSystem>();
        //Vector3 particleSpawnPos = new Vector3(points[0].transform.position.x, points[0].transform.position.y, points[0].transform.position.z);
        //Instantiate(melhoriasParticle, particleSpawnPos, Quaternion.identity);


    }

    public void ButtonPressed()
    {
        auMenu.clip = menuClips[0];
        auMenu.Play();
    }

    public void Next()
    {

        if (mySequence != null)
        {

            mySequence.Kill(true);
        }
        mySequence = DOTween.Sequence();
        move = skins[selectedSkin].GetComponent<Transform>();
        scaleTo = move.localScale * 0.5f;
        mySequence.Join(move.DOLocalMoveX(30, 2)).Join(move.DOScale(scaleTo, 1)).Join(skins[selectedSkin].GetComponent<Image>().DOFade(0, 1)).OnComplete(() => move.DOScale(scaleInicial, 0));
        selectedSkin++;
        move2 = skins[selectedSkin].GetComponent<Transform>();
        mySequence.Join(move2.DOLocalMoveX(0, 2)).Join(skins[selectedSkin].GetComponent<Image>().DOFade(1, 1));


    }

    public void Back()
    {

        if (mySequence != null)
        {

            mySequence.Kill(true);
        }
        mySequence = DOTween.Sequence();
        move = skins[selectedSkin].GetComponent<Transform>();
        scaleTo = move.localScale * 0.5f;
        mySequence.Join(move.DOLocalMoveX(-30, 2)).Join(move.DOScale(scaleTo, 1)).Join(skins[selectedSkin].GetComponent<Image>().DOFade(0, 1)).OnComplete(() => move.DOScale(scaleInicial, 0));
        selectedSkin--;
        move2 = skins[selectedSkin].GetComponent<Transform>();
        mySequence.Join(move2.DOLocalMoveX(0, 2)).Join(skins[selectedSkin].GetComponent<Image>().DOFade(1, 1));


    }

    public void Equip()
    {
        if (selectedSkin == 0)
        {
            player1.SetActive(true);
            player2.SetActive(false);
            player3.SetActive(false);
            equipado0 = true;
        }
        
        if (selectedSkin == 1 && comprado1 == true)
        {
            player1.SetActive(false);
            player2.SetActive(true);
            equipado1 = true;
            player3.SetActive(false);
        }
        
        if (selectedSkin == 2 && comprado2 == true)
        {
            player1.SetActive(false);
            player2.SetActive(false);
            player3.SetActive(true);
            equipado2 = true;
        }
    }

    public void Comprar()
    {
      
        if(selectedSkin == 1)
        {
            if(Score.pontos >= 500)
            {
                comprado1 = true;
                Score.pontos = Score.pontos - 500;
                auMenu.clip = menuClips[1];
                auMenu.Play();
                StartCoroutine(CompradoSign());
                
                
            }

        }
        if(selectedSkin == 2)
        {
            if(Score.pontos >= 1500)
            {
                comprado2 = true;
                Score.pontos = Score.pontos - 1500;
                auMenu.clip = menuClips[1];
                auMenu.Play();
                StartCoroutine(CompradoSign());
            }
        }
    }

    public void Upgrade()
    {
        
        if(Score.pontos > 100 && points[0].activeSelf == false)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[0].SetActive(true);
            melhoriasParticle[0].SetActive(true);
            Score.pontos -= 100;
            GameManager.playerMaxFallingSpeed = 50f;
            precoVelocidade.SetText("300");
            
        }
        else if(Score.pontos > 300 && points[0].activeSelf && points[1].activeSelf == false)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[1].SetActive(true);
            melhoriasParticle[1].SetActive(true);
            Score.pontos -= 300;
            GameManager.playerMaxFallingSpeed = 70f;
            precoVelocidade.SetText("500");
        }
        else if(Score.pontos > 500 && points[1].activeSelf)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[2].SetActive(true);
            melhoriasParticle[2].SetActive(true);
            Score.pontos -= 500;
            GameManager.playerMaxFallingSpeed = 90f;
            precoVelocidade.SetText("MAX");
        }
        
    }

    public void Upgrade2()
    {
        if(Score.pontos > 100 && points[3].activeSelf == false)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[3].SetActive(true);
            melhoriasParticle[3].SetActive(true);
            Score.pontos -= 100;
            precoResistencia.SetText("300");
            PlayerScript.timeInterval = .08f;
        }
        else if(Score.pontos > 300 && points[3].activeSelf && points[4].activeSelf == false)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[4].SetActive(true);
            melhoriasParticle[4].SetActive(true);
            Score.pontos -= 300;
            precoResistencia.SetText("500");
            PlayerScript.timeInterval = .05f;
        }
        else if(Score.pontos > 500 && points[4].activeSelf)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[5].SetActive(true);
            melhoriasParticle[5].SetActive(true);
            Score.pontos -= 500;
            precoResistencia.SetText("MAX");
            PlayerScript.timeInterval = .01f;
        }
    }

    public void Upgrade3()
    {
        if (Score.pontos > 100 && points[6].activeSelf == false)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[6].SetActive(true);
            melhoriasParticle[6].SetActive(true);
            Score.pontos -= 100;
            precoTempoReacao.SetText("300");
            successQte.sizeDelta = new Vector2(200, 25);

        }

        else if (Score.pontos > 300 && points[6].activeSelf && points[7].activeSelf == false)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[7].SetActive(true);
            melhoriasParticle[7].SetActive(true);
            Score.pontos -= 300;
            precoTempoReacao.SetText("500");
            successQte.sizeDelta = new Vector2(250, 25);
        }

        else if (Score.pontos > 500 && points[7].activeSelf)
        {
            auMenu.PlayOneShot(menuClips[1]);
            auMenu.PlayOneShot(menuClips[2]);
            points[8].SetActive(true);
            melhoriasParticle[8].SetActive(true);
            Score.pontos -= 500;
            precoTempoReacao.SetText("MAX");
            successQte.sizeDelta = new Vector2(300, 25);
        }
    }



    IEnumerator CompradoSign()
    {
        comprado.transform.DOLocalMoveY(73, 1);
        yield return new WaitForSeconds(2);
        comprado.transform.DOLocalMoveY(-70, 1);
    }

    private void FixedUpdate()
    {
        if (StartGame.startGame)
        {
            anMenu.SetTrigger("pStart");
        }
        if (selectedSkin == skins.Length-1)
        {

            next.interactable = false;
        }
        if (selectedSkin < skins.Length-1)
        {
            next.interactable = true;
        }

        if (selectedSkin <= 0)
        {

            previous.interactable = false;
        }

        if (selectedSkin > 0)
        {
            previous.interactable = true;
        }

        if (selectedSkin == 0)
        {
            comprar.interactable = false;
            preco.SetText("0");
            if (equipado0)
            {
                equipado.SetActive(true);
                equipado1 = false;
                equipado2 = false;
                
            }
            if (equipado1 || equipado2)
            {
                equipado.SetActive(false);
            }
        }
        else if (selectedSkin == 1)
        {
            if (comprado1 == false)
            {
                comprar.interactable = true;
            }
            else
            {
                comprar.interactable = false;
            }

            if(equipado1)
            {
                equipado.SetActive(true);
                equipado0 = false;
                equipado2 = false;
                
            }
            if (equipado0 || equipado2)
            {
                equipado.SetActive(false);
            }

            preco.SetText("500");
        }
        else if(selectedSkin == 2)
        {
            if (comprado2 == false)
            {
                comprar.interactable = true;
            }
            else
            {
                comprar.interactable = false;
            }
            preco.SetText("1500");

            if (equipado2)
            {
                equipado.SetActive(true);
                equipado0 = false;
                equipado1 = false;

            }

            if (equipado1 || equipado0)
            {
                equipado.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
