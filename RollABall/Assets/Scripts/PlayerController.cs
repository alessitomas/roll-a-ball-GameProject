using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb; 
    private int countPoints;
    private int countLifes;
    private float movementX;
    private float movementY;
    public float speed = 10; 
    public TextMeshProUGUI countText;
    public TextMeshProUGUI countLifeText;
    public TextMeshProUGUI timerText; 
    private float timer = 30.00f; 
    public AudioSource audioSource;
    public AudioClip pop;
    public AudioClip damage;
    private Scene currentScene;
    public GameObject winMessageObject;
    public GameObject loseMessageObject; 
    private bool won;
    private bool lost;


    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        rb = GetComponent<Rigidbody>(); 
        countLifes = 3;
        countPoints = 0;
        SetCountPointsText();
        SetCountLifeText();
        loseMessageObject.SetActive(false);
        winMessageObject.SetActive(false);

        won = false;
        lost = false;



        StartCoroutine(StartTimer());

    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

   void SetCountPointsText() {
        if (lost){
            return;
        }

       countText.text =  "Points: " + countPoints.ToString();

       if(currentScene.name == "Minigame" && countPoints >= 9){
        winMessageObject.SetActive(true);
        won = true;
        StartCoroutine(LoadMainMenuDelayed());
       }
       else if(currentScene.name == "Level1" && countPoints >= 18){
        winMessageObject.SetActive(true);
        won = true;
        StartCoroutine(LoadMainMenuDelayed());
       }
        else if(currentScene.name == "Level3" && countPoints >= 12){
        winMessageObject.SetActive(true);
        won = true;
        StartCoroutine(LoadMainMenuDelayed());
       }
    }

    void SetCountLifeText()
    {
        countLifeText.text = "Lifes: " + countLifes.ToString();

        if(countLifes < 0)
        {
            lost = true;
            loseMessageObject.SetActive(true);
            StartCoroutine(LoadMainMenuDelayed());
        }
    }

    IEnumerator LoadMainMenuDelayed()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator StartTimer()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1f;
            UpdateTimerText();
        }

        if (!won){
            loseMessageObject.SetActive(true);
            StartCoroutine(LoadMainMenuDelayed());
        }

    }

    void UpdateTimerText()
    {
        timerText.text = "Time: " + timer.ToString("F0"); 
    }

    private void FixedUpdate() 
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        if (transform.position.y < -10) // Assuming the map is at y = 0 and the player falls off below y = -10
        {
            PlayerFell();
        }
    }

    void PlayerFell()
    {
        if (won){
            return;
        }
        // Player has lost the game
        loseMessageObject.SetActive(true);
        StartCoroutine(LoadMainMenuDelayed());
 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (won){
            return;
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            audioSource.clip = damage;
            audioSource.Play();
            countLifes--;
            SetCountLifeText();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickItUp")) 
        {
            other.gameObject.SetActive(false);
            audioSource.clip = pop;
            audioSource.Play();
            countPoints++;

            SetCountPointsText();
        }
    }
}
