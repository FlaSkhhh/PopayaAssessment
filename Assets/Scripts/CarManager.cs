using UnityEngine;
using UnityEngine.InputSystem;

public class CarManager : MonoBehaviour
{
    [SerializeField] GameManager gm;

    Animator animator;
    Rigidbody rb;
    [SerializeField] float laneDistance = 3;
    [SerializeField] float moveSpeed = 3;
    [SerializeField] float jumpSpeed = 3;
    CarLane currentLane;
    bool isJumping;
    bool isDead;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentLane = CarLane.Center;
    }

    void Update()
    {
        if (isDead) return;
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            LaneShift(false);
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)         //else if because only left or right input at one time
        {
            LaneShift(true);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isJumping)
        {
            Jump();
        }
    }

    void LaneShift(bool rightSide)
    {
        if (rightSide)
        {
            if (currentLane == CarLane.Right) return;
            currentLane = currentLane == CarLane.Center ? CarLane.Right : CarLane.Center;       //change to right if its in center or change to center if its left\
            animator.SetInteger("Car Turn", 1);
        }
        else
        {
            if (currentLane == CarLane.Left) return;
            currentLane = currentLane == CarLane.Center ? CarLane.Left : CarLane.Center;
            animator.SetInteger("Car Turn", -1);
        }
    }
       
    //for rigidbody movement of car
    void FixedUpdate()
    {
        if (isDead) return;
        if (gm.GameOverGetter()) return;        //to stop mvoving into obstacles after winning
        Vector3 targetPosition = new Vector3((int)currentLane * laneDistance, rb.position.y, rb.position.z);

        Vector3 newPos = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        //stop when its near
        if (Mathf.Approximately(rb.position.x, targetPosition.x))
        {
            rb.MovePosition(targetPosition);
            animator.SetInteger("Car Turn", 0);
        }
        
    }

    void Jump()
    {
        if (!isJumping)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            isJumping = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && isJumping)
        {
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Obstacle") && !isDead  && !gm.GameOverGetter())
        {
            isDead = true;
            collision.gameObject.GetComponent<Collider>().enabled = false;
            ObstacleManager.Instance.StopObstacles();
            rb.AddForce((Vector3.up + Vector3.back) * 2f, ForceMode.Impulse);
            Invoke(nameof(GameOver), 1f);
            Debug.LogError("DEAD");
        }
    }

    void GameOver()
    {
        gm.GameOver(false);
    }
}

public enum CarLane
{
    Left = -1,
    Center = 0,
    Right = 1
}
