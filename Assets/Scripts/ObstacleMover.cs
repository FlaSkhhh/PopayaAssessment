using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    Rigidbody rb;
    float moveSpeed;
    float obstacleDisableZVal;
    void OnEnable()
    {
        if(rb == null) TryGetComponent<Rigidbody>(out rb);
        
        obstacleDisableZVal = ObstacleManager.Instance.ObstacleDisablePositionGetter();
    }

    public void SetStartingPos()
    {
        transform.position = new Vector3(rb.position.x, rb.position.y, ObstacleManager.Instance.StartingPosGetter().z);
    }

    void FixedUpdate()
    {
        //move speed is fetched each fixed update to stay with current speed of all obstacles which will gradully increase to simulate car acceleration
        moveSpeed = ObstacleManager.Instance.ObstacleSpeedGetter();
        Vector3 targetPosition = new Vector3(rb.position.x, rb.position.y, obstacleDisableZVal);  

        Vector3 newPos = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Mathf.Approximately(rb.position.z, targetPosition.z))
        {
            gameObject.SetActive(false);
            ObstacleManager.Instance.DisableObstacle(this);
        }
    }
}
