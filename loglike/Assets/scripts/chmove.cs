using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class chmove : MonoBehaviour
{
   
    public float speed = 5f;
    private  Rigidbody2D rb;
    private Vector2 movement;
    void Start()
    {
        // Rigidbody 
        rb = GetComponent<Rigidbody2D>();
         Debug.Log("Horizontal Input");
    }

    void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
   
        movement = new Vector2(h, v);

        if (movement.magnitude > 1f)
            movement.Normalize();

   
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle; // Rigidbody2D
        }
   
    }

    void FixedUpdate()
    {
        // Rigidbody2D
        rb.velocity = movement * speed;
    }
}