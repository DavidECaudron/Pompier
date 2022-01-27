using UnityEngine;
using System.Collections.Generic;
public class HouseFire
{
    protected  Dictionary<Vector2Int, GameObject> allFiresPosition;
    protected  List<GameObject> currentFirePosition;

    protected static int MAX_FIRE_START = 3;

    public void StartFire()
    {
        var numberFire = Random.Range(0, 3);

        for (var i = 0; i < numberFire; i++)
        {
            //currentFirePosition.Add(allFiresPosition[new Vector2Int(Random.Range(0, ))])
        }
    }
}