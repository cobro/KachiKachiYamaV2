using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomCharacterController : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	private Vector3 m_Velocity = Vector3.zero;
    public float CharacterSpeed = 0.5f;
    public float SparkStrikingTime = 0.2f;
    public int StrikeCounter = 0;
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
    public Animator mainCharacterAnimator;
    public float CharacteranimationSpeedUpper = 2f;
    void Start()
    {
        MainCharacterRB = GetComponent<Rigidbody2D>();
        mainCharacterAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RacoonCheckingReference = racoonMovement.RacoonChecking;
        waitForNextCollisionReference = racoonMovement.waitForNextCollision;

        if(Input.GetAxisRaw("Horizontal") >0){
            horizontalMove = Input.GetAxisRaw("Horizontal") * CharacterSpeed*3;
            mainCharacterAnimator.SetBool("WalkCycle", true);
            mainCharacterAnimator.SetBool("Idle", false);
        }
        if(Input.GetAxisRaw("Horizontal") == 0 && distanceFromRacoon < 6f){
            horizontalMove = 0;
            mainCharacterAnimator.SetBool("Idle", true);
            mainCharacterAnimator.SetBool("WalkCycle", false);
        }
        if (distanceFromRacoon > 6f && Input.GetAxisRaw("Horizontal") <= 0){
            horizontalMove = CharacterSpeed;
            mainCharacterAnimator.SetBool("WalkCycle", true);
            mainCharacterAnimator.SetBool("Idle", false);
        }

        distanceFromRacoon = Mathf.Abs(RacoonPosition.transform.position.x - transform.position.x);

        if(StrikeCounter == 5){
            YouWonUI.SetActive(true);
            StartCoroutine(WaitForRestart());
        }
        
        if (Input.GetMouseButtonUp(1)){

            if(mainCharacterAnimator.GetBool("FlintStoneOut") == false){
                mainCharacterAnimator.SetBool("FlintStoneOut", true);
            }
            else {
            mainCharacterAnimator.SetBool("FlintStoneOut", false);
            }
        }

        if (Input.GetMouseButtonUp(0) && mainCharacterAnimator.GetBool("FlintStoneOut") == true){
            StartCoroutine(StrikingSparkSpriteWait());
        }

        if(mainCharacterAnimator.GetBool("FlintStoneOut") == true && RacoonCheckingReference == true){
            busted = true;
            BustedUI.SetActive(true);
            StartCoroutine(WaitForRestart());
        }
        mainCharacterAnimator.SetFloat("WalkCycleSpeed",Remap(Input.GetAxisRaw("Horizontal") * CharacterSpeed*3,0,CharacterSpeed*3,.6f,CharacteranimationSpeedUpper));
    }

    float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
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
        mainCharacterAnimator.SetBool("StrikingSpark", true);
        yield return new WaitForSeconds(SparkStrikingTime);
        mainCharacterAnimator.SetBool("StrikingSpark", false);
        mainCharacterAnimator.SetBool("FlintStoneOut", false);
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
        Vector3 targetVelocity = new Vector2(move * 10f, MainCharacterRB.velocity.y);
        MainCharacterRB.velocity = Vector3.SmoothDamp(MainCharacterRB.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
	}


}