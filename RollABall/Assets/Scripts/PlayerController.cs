using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb; 
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 10; 
    public TextMeshProUGUI countText;

    public GameObject winMessageObject;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        count = 0;
        SetCountText();
        winMessageObject.SetActive(false);
    }

    void OnMove(InputValue movementValue){
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText() {
       countText.text =  "Count: " + count.ToString();
       if( count >= 17){
        winMessageObject.SetActive(true);
       }
    }
    private void FixedUpdate() 
    {
        Vector3 movement = new Vector3 (movementX , 0.0f, movementY );
        rb.AddForce(movement * speed);
    }
    void OnTriggerEnter(Collider other){
       if (other.gameObject.CompareTag("PickItUp")) 
       {
           other.gameObject.SetActive(false);
           count ++;
           SetCountText();
       }
    }
}
