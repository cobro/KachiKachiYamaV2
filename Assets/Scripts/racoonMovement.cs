using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class racoonMovement : MonoBehaviour
{

    [Range(0, .3f)] [SerializeField] private float m_RacoonMovementSmoothing = .05f;	// How much to smooth out the movement
	private Vector3 m_RacoonVelocity = Vector3.zero;
    public float RacoonSpeed = 15;
    float RacoonHorizontalMove = 0f;
    public float RandomRotationLowerLimit = 2f;
    public float RandomRotationUpperLimit = 10f;

    public static bool RacoonChecking = false;
    public float tempRacoonSpeed;
    float waitTime;
    public static bool waitForNextCollision = false;
    public SpriteRenderer RacoonSpriteRenderer;
    public GameObject QuestionMark;
    public float RacoonSpriteAnimationSpeed;
    bool randomRotationStart = true;
    float distanceFromRacoonReference;

    public Rigidbody2D RacoonRB;
    void Start()
    {
        RacoonRB = GetComponent<Rigidbody2D>();
        RacoonSpriteRenderer = GetComponent<SpriteRenderer>();
        tempRacoonSpeed = RacoonSpeed;
        QuestionMark = this.transform.GetChild(0).gameObject;
        QuestionMark.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromRacoonReference = CustomCharacterController.distanceFromRacoon;
        RacoonHorizontalMove = tempRacoonSpeed;
        randomRotationStartCall();
        CheckForRacoonRotation();
    }

    void FixedUpdate(){
        RacoonMove(RacoonHorizontalMove * Time.fixedDeltaTime);
    }

    IEnumerator RandomlyRotate(){
        waitTime = Random.Range(RandomRotationLowerLimit,RandomRotationUpperLimit);
        yield return new WaitForSeconds(waitTime);  
        tempRacoonSpeed = 0;
        QuestionMark.SetActive(true);
        yield return new WaitForSeconds(Random.Range(.5f,1f));
        QuestionMark.SetActive(false);
        RacoonChecking = true;
        yield return new WaitForSeconds(Random.Range(1,2));
        RacoonChecking = false;
        tempRacoonSpeed = RacoonSpeed;
        randomRotationStart = true;
    }

    void randomRotationStartCall(){
        if(distanceFromRacoonReference<4 && randomRotationStart && !waitForNextCollision){
            randomRotationStart = false;
            StartCoroutine("RandomlyRotate");
        }
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
        if(collision.gameObject.tag == "Player" && waitForNextCollision == false){
            StopCoroutine("RandomlyRotate");
            StartCoroutine("LookingBackCausedByCollision");
        }
    }

    IEnumerator LookingBackCausedByCollision(){
        waitForNextCollision = true;
        Debug.Log("Collided with Racoon");
        RacoonRB.isKinematic = true;
        tempRacoonSpeed = 0;
        QuestionMark.SetActive(true);
        yield return new WaitForSeconds(Random.Range(.3f,.5f));
        QuestionMark.SetActive(false);
        RacoonChecking = true;
        yield return new WaitForSeconds(Random.Range(1,2));
        RacoonChecking = false;
        tempRacoonSpeed = RacoonSpeed;
        waitForNextCollision = false;
        RacoonRB.isKinematic = false;
        randomRotationStart = true;
    }

        void RacoonMove(float move)
	{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, RacoonRB.velocity.y);
			// And then smoothing it out and applying it to the character
			RacoonRB.velocity = Vector3.SmoothDamp(RacoonRB.velocity, targetVelocity, ref m_RacoonVelocity, m_RacoonMovementSmoothing);

	}
}
