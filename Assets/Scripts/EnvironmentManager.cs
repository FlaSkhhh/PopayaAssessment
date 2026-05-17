using System.IO;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] Transform[] roadStrips;
    [SerializeField] Vector3 firstRoadStripPos = new(0,0.01f,-15f);
    [SerializeField] float roadStripDistance = 12f;
    [SerializeField] float roadStripEndZPos = -27f;     //first road strip + dist
    [SerializeField] float roadStripResetZPos = 45f;

    void Start()
    {
        for(int i = 0; i < roadStrips.Length; i++) 
        {
            roadStrips[i].transform.position = firstRoadStripPos + new Vector3(0, 0, roadStripDistance * i);   //initialize all road strips
        }
    }

    public float RoadStripResetPosGetter()
    {
        return roadStripResetZPos;
    }

    public float EnvironmentSpeedGetter()
    {
        return ObstacleManager.Instance.ObstacleSpeedGetter();
    }

    public float RoadStripEndPosGetter()
    {
        return roadStripEndZPos;
    }
}
