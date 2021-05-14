using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _body;
    private Vector3 moveDirection = Vector3.zero;

    public GameObject ballPrefab;

    public float speed;
    public float jumpSpeed;
    public float cooldown;

    private Queue<GameObject> _worldObjects = new Queue<GameObject>();

    private Animator anim;
    private int health = 4;
    int choice = 0;
    bool curRun = false;

    float randY = 0;
    float randX = 0;

    float dt = .8f;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!curRun)
        {
            choice = 0;
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        if(dt < cooldown)
        {
            choice = Random.Range(1,4);
            dt = .8f;
            curRun = false;
        }
        else
        {
            dt -= .001f;
            //Debug.Log(dt.ToString());
        }
        
        if(_worldObjects.Count > 3)
        {
            Destroy(_worldObjects.Dequeue());
        }
        if(choice == 1) 
        {
            anim.SetTrigger("jump");
            var velocity = _body.velocity;
            velocity.y = jumpSpeed;
            _body.velocity = velocity;
            anim.SetTrigger("return");
        }

        if(choice == 2) 
        {
            anim.SetTrigger("throw");
            var ball = Instantiate(ballPrefab);
            ball.transform.position = new Vector3(0, 2, -1) + _body.position;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -speed);
            _worldObjects.Enqueue(ball);
            anim.SetTrigger("return");
        }

        if(choice == 3 && !curRun)
        {
            anim.SetTrigger("running2");
            randX = Random.Range(-1,1);
            randY = Random.Range(-1,1);
            curRun = true;
        }
        else
        { 
            anim.SetTrigger("return2");
        }

        if(choice == 3)
        {
            moveDirection = new Vector3(randX, 0, randY) * speed * .1f;
            Vector3 newposition = _body.position + moveDirection;
            transform.position = Vector3.MoveTowards(_body.position, newposition, .01f);
            float angle = Mathf.Atan2(randX, randY) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
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
