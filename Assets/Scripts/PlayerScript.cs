using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public static Rigidbody rb;
    public float velocityZ;
    public GameObject moedaCollectPrefab;
    public GameObject obstacleHit;
    public GameObject dezPontos;
    public GameObject menosDezPontos;
    public GameObject cemPontos;
    public Animator an;
    private Vector3 movementTarget;
    public AudioClip[] audios;
    public AudioSource au;
    public AudioSource auPlayer;
    public AudioSource auMusic;
    bool triggerSound = false;
    bool triggerParticle = false;
    public GameObject paraquedas;
    float maxVelocidadeInicial;
    public ParticleSystem veloParticle;
  


    public static float timeInterval = .1f;
    // Start is called before the first frame update
    void Start()
    {
        
        paraquedas.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        triggerSound = true;
        triggerParticle = false;
        NumerosFinal.coletado = 0;



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        { 
            an.SetTrigger("pJump");
        }

        if(rb.velocity.z > 1 && triggerSound == true)
        {
            
            maxVelocidadeInicial = GameManager.playerMaxFallingSpeed;
            au.clip = audios[0];
            au.Play();
            auMusic.clip = audios[4];
            auMusic.Play();
            triggerSound = false;
        }

       

    }

    void FixedUpdate()
    {
        if (Altitude.distanceToPlayer < 3950f && triggerParticle == false)
        {
            veloParticle.Play();
            triggerParticle = true;
        }
        if (Altitude.distanceToPlayer < 400f)
        {
            an.SetTrigger("pOpening");
            paraquedas.SetActive(true);
            veloParticle.Stop();
            
        }

        if (StartGame.startGame==true)
        {
            
            rb.useGravity = true;
            float tiltX = Input.GetAxis("Horizontal");
            float tiltY = Input.GetAxis("Vertical");
            movementTarget = new Vector3(tiltX * GameManager.playerMovementRadius * 3f, tiltY * GameManager.playerMovementRadius * 3f, rb.position.z);

            if (rb.velocity.z > GameManager.playerMaxFallingSpeed)
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, GameManager.playerMaxFallingSpeed);

            Vector3 origin = new Vector3(0, 0, rb.position.z);
            float distanceToOrigin = Vector3.Distance(movementTarget, origin);
            if (distanceToOrigin > GameManager.playerMovementRadius)
            {
                Vector3 projectedTarget = Vector3.ProjectOnPlane(movementTarget, Vector3.forward).normalized;
                movementTarget = new Vector3(projectedTarget.x * GameManager.playerMovementRadius, projectedTarget.y * GameManager.playerMovementRadius, rb.position.z);
            }
            Vector3 velocityXY;
            if(Altitude.distanceToPlayer > 300)
            {
                velocityXY = (movementTarget - rb.position) * GameManager.playerMovementSpeed;
                rb.velocity = new Vector3(velocityXY.x, velocityXY.y, rb.velocity.z);
            }

            if(Altitude.distanceToPlayer < 300)
            {
                velocityXY = movementTarget * GameManager.playerMovementSpeed;
                rb.velocity = new Vector3(velocityXY.x, velocityXY.y, rb.velocity.z);

            }

            //store the falling speed
            if (rb.velocity.z > velocityZ)
                velocityZ = rb.velocity.z;
        }

       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Moeda")
        {
            auPlayer.clip = audios[1];
            auPlayer.Play();
            Instantiate(moedaCollectPrefab, other.transform.position, other.transform.rotation);
            Instantiate(dezPontos, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Score.pontos+=10;
            NumerosFinal.coletado += 10;
            

        }

        if (other.gameObject.tag == "PowerUp")
        {
            auPlayer.clip = audios[5];
            auPlayer.Play();
            Instantiate(cemPontos, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Score.pontos += 100;
            NumerosFinal.coletado += 100;

            StartCoroutine(PowerUp());
        }

        if (other.gameObject.tag == "Obstacle")
        {
            
            auPlayer.PlayOneShot(audios[3]);
            auPlayer.PlayOneShot(audios[2]);
            Instantiate(obstacleHit, transform.position, transform.rotation);
            Instantiate(menosDezPontos, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Score.pontos -= 10;

            StartCoroutine(Damage(timeInterval));

        }

        if (other.gameObject.tag == "Terreno")
        {
           SceneManager.LoadScene("FinalScene");
        }

        if(other.gameObject.tag == "Alvo")
        {
            SceneManager.LoadScene("FinalScene");
            NumerosFinal.triggerAlvo = true;
        }
    }

    IEnumerator PowerUp()
    {
        yield return new WaitForSeconds(1);
        auPlayer.Stop();
        GameManager.playerMaxFallingSpeed = 100;
        yield return new WaitForSeconds(5);
        GameManager.playerMaxFallingSpeed = maxVelocidadeInicial;
    }

    IEnumerator Damage(float interval)
    {
        int flashTimes = 0;

        while(flashTimes < 5)
        {
            Renderer[] RendererArray = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in RendererArray)
                r.enabled = false;
            GameManager.playerMaxFallingSpeed = 5f;


            yield return new WaitForSeconds(interval);

            foreach (Renderer r in RendererArray)
                r.enabled = true;
            
            yield return new WaitForSeconds(interval);
            flashTimes++;
        }
        GameManager.playerMaxFallingSpeed = maxVelocidadeInicial;
    }

 
}
