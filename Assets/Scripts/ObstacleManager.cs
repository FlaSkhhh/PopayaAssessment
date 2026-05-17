using System.Collections.Generic;
using UnityEngine;
public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;

    [SerializeField] bool spawnObstacles;
    [SerializeField] float obstacleSpeed = 7;
    //[SerializeField] float obstacleSpawnFrequency = 4;
    [SerializeField] float obstacleDisableZVal = -10;
    [SerializeField] float firstObstacleZPos = 5;
    [SerializeField] float firstObstacleSpawnDistance = 15;
    [SerializeField] Vector3 startingPos = new Vector3(0,0,65f);
    
    [SerializeField] List<ObstacleMover> inactiveObstacles = new List<ObstacleMover>();
    [SerializeField] List<ObstacleMover> activeObstacles = new List<ObstacleMover>();

    float startingObstacleSpeed;
    void Awake()
    {
        //had to delay obstacle mover script execution so that this doesnt give null for active obstacles onenable
        Instance = this;
        startingObstacleSpeed = obstacleSpeed;
    }

    void Start()
    {
        obstacleSpeed = 0;
        for (int i = 0; i < (int)(startingPos.z / firstObstacleSpawnDistance); ++i)    //only spawn till the default spawn point
        {
            SpawnObstacleStart(i);
        }
    }

    //float spawnTimer = 0;
    void Update()
    {
        if (!spawnObstacles) return;
        /*if(spawnTimer < Time.time)
        {
            spawnTimer = Time.time + obstacleSpawnFrequency;
            if (inactiveObstacles.Count <= 0) return;       //safety check if obs run out before reaching to end
            SpawnObstacleRuntime();
            obstacleSpawnFrequency = Mathf.Clamp(obstacleSpawnFrequency - 0.3f, 0.8f, 10f);
            obstacleSpeed = Mathf.Clamp(obstacleSpeed + 0.5f,0,12f);
        }*/
        if(activeObstacles.Count > 0)
        {
            if (startingPos.z - activeObstacles[activeObstacles.Count - 1].transform.position.z >= firstObstacleSpawnDistance)
            {
                if (inactiveObstacles.Count <= 0) return;       //safety check if obs run out before reaching to end
                SpawnObstacleRuntime();
                //obstacleSpawnFrequency = Mathf.Clamp(obstacleSpawnFrequency - 0.3f, 0.8f, 10f);
                obstacleSpeed = Mathf.Clamp(obstacleSpeed + 0.5f, 0, 12f);
            } 
        }
    }

    void SpawnObstacleRuntime()
    {
        int rng = Random.Range(0, inactiveObstacles.Count);
        ObstacleMover go = inactiveObstacles[rng];
        go.gameObject.SetActive(true);
        go.SetStartingPos();
        inactiveObstacles.Remove(go);
        activeObstacles.Add(go);
    }
    void SpawnObstacleStart(int i)
    {
        int rng = Random.Range(0, inactiveObstacles.Count);
        ObstacleMover go = inactiveObstacles[rng];
        go.gameObject.SetActive(true);
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, firstObstacleZPos + i * firstObstacleSpawnDistance);
        inactiveObstacles.Remove(go);
        activeObstacles.Add(go);
    }

    public float ObstacleSpeedGetter()
    {
        return obstacleSpeed;
    }
    public float ObstacleDisablePositionGetter()
    {
        return obstacleDisableZVal;
    }

    public Vector3 StartingPosGetter()
    {
        return startingPos;
    }

    public void DisableObstacle(ObstacleMover obs)
    {
        activeObstacles.Remove(obs);
        inactiveObstacles.Add(obs);
    }

    public void StopObstacles()
    {
        spawnObstacles = false;
        obstacleSpeed = 0;
    }

    public void StartObstacles()
    {
        spawnObstacles = true;
        obstacleSpeed = startingObstacleSpeed;
    }
}
