using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CityFireManager : MonoBehaviour
{
    public static CityFireManager Instance {get;set;}
    private UnityEvent _fireTimeOut = new UnityEvent();

    [SerializeField] private float TIME_INTERVAL_SECONDS = 1.0f;

    [SerializeField] private bool _enable = true;
    [SerializeField] private City city;
    
    [SerializeField]
    private List<Vector2Int> housesFired = new List<Vector2Int>();

    private IEnumerator FireTimeOut()
    {
        this.newHouseFired();
        
        yield return new WaitForSeconds(TIME_INTERVAL_SECONDS);

        if(_enable)
        {
            StartCoroutine(FireTimeOut());
        }
    }

    private void newHouseFired()
    {
        var map = city.GetMap();
    
        int x = Random.Range(0, city.GetMap().GetLength(0));
        int y = Random.Range(0, city.GetMap().GetLength(1));

        while(housesFired.Contains(new Vector2Int(x, y)))
        {
            x = Random.Range(0, city.GetMap().GetLength(0));
            y = Random.Range(0, city.GetMap().GetLength(1));
        }

        housesFired.Add(new Vector2Int(x, y));
        Debug.Log("Fire");
    }

    public void setEnable(bool value)
    {
        _enable = value;
    }

    public void StartFiref()
    {
        StartCoroutine(FireTimeOut());
    }
}