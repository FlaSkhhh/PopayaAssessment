using UnityEngine;

public class EnvironmentMover : MonoBehaviour
{
    [SerializeField] EnvironmentManager evManager;

    //differnt reset pos for right side as it is more visible due to curvature
    [SerializeField] float resetPosRightSideBuilding;
    [SerializeField] bool rightSideBuilding;
    
    float moveSpeed;
    float endPos;
    float resetPos;
    void Start()
    {
        resetPos = evManager.RoadStripResetPosGetter();
        endPos = evManager.RoadStripEndPosGetter();
    }

    //moving them in fixed update so it looks same as obstacles
    void FixedUpdate()
    {
        moveSpeed = evManager.EnvironmentSpeedGetter();
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, endPos);

        transform.position = Vector3.MoveTowards(transform.position,targetPosition, moveSpeed * Time.fixedDeltaTime);

        if (transform.position.z <= targetPosition.z)       //when target has been passed 
        {
            transform.position = new(transform.position.x, transform.position.y, rightSideBuilding ? resetPosRightSideBuilding : resetPos);
        }
    }
}
