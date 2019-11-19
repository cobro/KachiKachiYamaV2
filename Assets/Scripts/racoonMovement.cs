using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class racoonMovement : MonoBehaviour
{

    [Range(0, .3f)] [SerializeField] private float m_RacoonMovementSmoothing = .05f;	// How much to smooth out the movement
	private Vector3 m_RacoonVelocity = Vector3.zero;
    public float RacoonSpeed;
     float RacoonHorizontalMove = 0f;
    public float RandomRotationLowerLimit = 2f;
    public float RandomRotationUpperLimit = 10f;

    public static bool RacoonChecking = false;
    public float tempRacoonSpeed;
    float waitTime;
    public static bool waitFornextCollision = false;
    public SpriteRenderer RacoonSpriteRenderer;

    public float RacoonSpriteAnimationSpeed;

    public bool animateRacoon = true;

    public Rigidbody2D RacoonRB;
    void Start()
    {
        RacoonRB = GetComponent<Rigidbody2D>();
        RacoonSpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("RandomlyRotate");
    }

    // Update is called once per frame
    void Update()
    {
        RacoonHorizontalMove = RacoonSpeed;
        CheckForRacoonRotation();
    }

    void FixedUpdate(){
        RacoonMove(RacoonHorizontalMove * Time.fixedDeltaTime);
    }

    IEnumerator RandomlyRotate(){
        waitTime = Random.Range(RandomRotationLowerLimit,RandomRotationUpperLimit);
        yield return new WaitForSeconds(waitTime);  
        tempRacoonSpeed = RacoonSpeed;
        RacoonSpeed = 0;
        yield return new WaitForSeconds(Random.Range(.1f,.5f));
        RacoonChecking = true;
        yield return new WaitForSeconds(Random.Range(1,2));
        RacoonChecking = false;
        RacoonSpeed = 10f;
        StartCoroutine("RandomlyRotate");
    }

    void CheckForRacoonRotation(){
        if(RacoonChecking){
                RacoonSpriteRenderer.flipX = true;
            }
        else{
            RacoonSpriteRenderer.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player" && waitFornextCollision == false){
            StopCoroutine("RandomlyRotate");
            StartCoroutine("LookingBackCausedByCollision");
        }
    }

    IEnumerator LookingBackCausedByCollision(){
        waitFornextCollision = true;
        Debug.Log("Collided with Racoon");
        tempRacoonSpeed = RacoonSpeed;
        RacoonSpeed = 0;
        yield return new WaitForSeconds(Random.Range(.1f,.5f));
        RacoonChecking = true;
        yield return new WaitForSeconds(Random.Range(1,2));
        RacoonChecking = false;
        RacoonSpeed = 10f;
        waitFornextCollision = false;
        StartCoroutine("RandomlyRotate");
        
    }

        void RacoonMove(float move)
	{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, RacoonRB.velocity.y);
			// And then smoothing it out and applying it to the character
			RacoonRB.velocity = Vector3.SmoothDamp(RacoonRB.velocity, targetVelocity, ref m_RacoonVelocity, m_RacoonMovementSmoothing);

	}
}
