using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HouseFire : MonoBehaviour
{
    protected  Dictionary<Vector2Int, GameObject> allFiresPosition;
    protected  List<(Vector2Int, GameObject)> currentFirePosition;

    protected static int MAX_FIRE_START = 3;

    public void StartFire()
    {
        var numberFire = Random.Range(0, 3);
        
        var positions = new List<(Vector2Int, GameObject)>();

        foreach (var position in allFiresPosition)
        {
            positions.Add((position.Key, position.Value));
        }

        for (var i = 0; i < numberFire; i++)
        {
            var index = Random.Range(0, allFiresPosition.Count);
            var position = positions[index];
            positions.RemoveAt(index);
            currentFirePosition.Add(position);
        }

        StartCoroutine(waitNextFire());
    }

    public IEnumerator waitNextFire()
    {
        yield return new WaitForSeconds(1);

        foreach (var fire in currentFirePosition)
        {
            var pos = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));

            while(allFiresPosition[pos] == null && allFiresPosition.Count > 0)
            {
                pos = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
            }
            
            currentFirePosition.Add((pos, allFiresPosition[pos]));
        }
    }
}