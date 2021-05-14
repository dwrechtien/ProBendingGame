using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _body;
    private Vector3 moveDirection = Vector3.zero;

    public GameObject ballPrefab;

    public float speed;
    public float jumpSpeed;

    private Queue<GameObject> _worldObjects = new Queue<GameObject>();

    private Animator anim;
    private int health = 4;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(_worldObjects.Count > 3)
        {
            Destroy(_worldObjects.Dequeue());
        }
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            anim.SetTrigger("jump");
            var velocity = _body.velocity;
            velocity.y = jumpSpeed;
            _body.velocity = velocity;
            anim.SetTrigger("return");
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            anim.SetTrigger("throw");
            var ball = Instantiate(ballPrefab);
            ball.transform.position = new Vector3(0, 2, 1) + _body.position;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
            _worldObjects.Enqueue(ball);
            anim.SetTrigger("return");
        }

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
        transform.position = _body.position + moveDirection;
        float angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.name == "Rock(Clone)")
        {
            health -= 1;
            if(health == 0)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        
    }
}
