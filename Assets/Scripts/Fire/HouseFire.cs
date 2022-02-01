using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HouseFire : MonoBehaviour
{
    protected Dictionary<Vector2Int, GameObject> allFiresPosition = new Dictionary<Vector2Int, GameObject>();
    protected List<(Vector2Int, GameObject)> currentFirePosition = new List<(Vector2Int, GameObject)>();

    protected static int MAX_FIRE_START = 3;

    public void StartFire()
    {
        Debug.Log("StartFire");
        var go = new GameObject("empty");
        allFiresPosition[new Vector2Int(0,0)] = go;
        go.transform.position = new Vector3(0,0, 5);
        go.transform.parent = transform;
        var numberFire = Random.Range(1, 4);
        
        var positions = new List<(Vector2Int, GameObject)>();

        foreach (var position in allFiresPosition)
        {
            positions.Add((position.Key, position.Value));
        }

        for (var i = 0; i < numberFire; i++)
        {
            if(positions.Count > 0)
            {
                var index = Random.Range(0, allFiresPosition.Count);
                var position = positions[index];
                positions.RemoveAt(index);
                currentFirePosition.Add(position);
                InstanciateVisualFire(position.Item2.transform.position);
            }
        }

        StartCoroutine(waitNextFire());
    }

    public IEnumerator waitNextFire()
    {
        yield return new WaitForSeconds(1);

        /*foreach (var fire in currentFirePosition)
        {
            var pos = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));

            while(allFiresPosition[pos] == null && allFiresPosition.Count > 0)
            {
                pos = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
            }
            
            currentFirePosition.Add((pos, allFiresPosition[pos]));
            InstanciateVisualFire(allFiresPosition[pos].transform.position);
        }*/
    }

    private void InstanciateVisualFire(Vector3 position)
    {
        Debug.Log(transform.name);
        Instantiate(GameManager.Instance.city.GetComponent<City>().prefabHouseFire, position, Quaternion.Euler(Vector3.zero), transform);
    }
}