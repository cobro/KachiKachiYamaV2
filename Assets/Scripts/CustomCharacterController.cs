using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomCharacterController : MonoBehaviour
{

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	private Vector3 m_Velocity = Vector3.zero;
    public Sprite FlintStoneOutSprite;
    public Sprite StrikingSparkSprite;
    public Sprite  CharacterWalkingArray;
    public float CharacterSpriteAnimationSpeed;
    public float CharacterSpeed = 0.5f;
    public float SparkStrikingTime = 0.2f;
    public SpriteRenderer CharacterSpriteRenderer;
    public int StrikeCounter = 0;
    public bool FlintStoneOut = false;
    public bool StrikingSpark = false;
    bool RacoonCheckingReference;
    bool waitForNextCollisionReference;
    public bool busted = false;
    public Text SuccessfulStrikesCounter;

    public GameObject BustedUI;
    public GameObject YouWonUI;
    public Transform RacoonPosition;
    float horizontalMove = 0f;
    public Rigidbody2D MainCharacterRB;
    public float GetAPointDistance = 7f;

    public static float distanceFromRacoon;
    void Start()
    {
        CharacterSpriteRenderer = GetComponent<SpriteRenderer>();
        MainCharacterRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        RacoonCheckingReference = racoonMovement.RacoonChecking;
        waitForNextCollisionReference = racoonMovement.waitForNextCollision;

        if(Input.GetAxisRaw("Horizontal") <0){
            horizontalMove = Input.GetAxisRaw("Horizontal") * CharacterSpeed;
        }
        if(Input.GetAxisRaw("Horizontal") >0){
            horizontalMove = Input.GetAxisRaw("Horizontal") * CharacterSpeed*3;
        }
        if(Input.GetAxisRaw("Horizontal") == 0 && distanceFromRacoon < 6f){
            horizontalMove = 0;
        }
        if (distanceFromRacoon > 6f && Input.GetAxisRaw("Horizontal") <= 0){
            horizontalMove = CharacterSpeed;
        }

        distanceFromRacoon = Mathf.Abs(RacoonPosition.transform.position.x - transform.position.x);
        Debug.Log(distanceFromRacoon);

        if(StrikeCounter == 5){
            YouWonUI.SetActive(true);
            StartCoroutine(WaitForRestart());
        }
        
        if (Input.GetMouseButtonUp(1)){

            if(FlintStoneOut == false){
                FlintStoneOut = true;
                Debug.Log("Flint Stone Out");
                CharacterSpriteRenderer.sprite = FlintStoneOutSprite;
            }
            else {
            FlintStoneOut = false;
            CharacterSpriteRenderer.sprite = CharacterWalkingArray;
            }
        }

        if (Input.GetMouseButtonUp(0) && FlintStoneOut){

            StartCoroutine(StrikingSparkSpriteWait());

        }

        if(FlintStoneOut == true && RacoonCheckingReference == true){
            busted = true;
            BustedUI.SetActive(true);
            StartCoroutine(WaitForRestart());
        }
    }

    void FixedUpdate(){
        Move(horizontalMove * Time.fixedDeltaTime);

        if(MainCharacterRB.velocity.x < 0 && Input.GetAxisRaw("Horizontal") < 0){
            MainCharacterRB.velocity = new Vector2(0,0);
        }
        if(distanceFromRacoon<2f && RacoonCheckingReference){
            MainCharacterRB.velocity = new Vector2(0,0);
        }
    }
    
    IEnumerator StrikingSparkSpriteWait(){
        Debug.Log("Struck a spark");
        CharacterSpriteRenderer.sprite = StrikingSparkSprite;
        StrikingSpark = true;
        yield return new WaitForSeconds(SparkStrikingTime);
        StrikingSpark = false;
        FlintStoneOut = false;
        CharacterSpriteRenderer.sprite = CharacterWalkingArray;
        if(!busted && distanceFromRacoon<GetAPointDistance){
            StrikeCounter++;
            SuccessfulStrikesCounter.text = StrikeCounter.ToString() +"/5";
        }
        StopCoroutine("StrikingSparkSpriteWait");
    }
    IEnumerator WaitForRestart(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Move(float move)
	{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, MainCharacterRB.velocity.y);
			// And then smoothing it out and applying it to the character
			MainCharacterRB.velocity = Vector3.SmoothDamp(MainCharacterRB.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

	}
}