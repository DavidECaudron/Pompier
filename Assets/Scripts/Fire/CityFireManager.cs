using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CityFireManager : MonoBehaviour
{
    public static CityFireManager Instance {get;set;}
    private UnityEvent _fireTimeOut = new UnityEvent();

    [SerializeField] private float TIME_INTERVAL_SECONDS = 1.0f;

    [SerializeField] private bool _enable = true;
    [SerializeField] private City city;
    
    [SerializeField]
    private List<Position> housesFired = new List<Position>();
    private Dictionary<Position, CellData> housesLeft;

    public void SetHouses()
    {
        housesLeft = GameManager.Instance.city.GetComponent<City>().GetHouses();
    }

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
        if(housesLeft != null)
        {
            var housesList = housesLeft.ToList();
            int x = city.Size;
            int y = city.Size;

            var index = Random.Range(0, housesList.Count);

            var nextHouseFired = housesList[index];

            housesFired.Add(nextHouseFired.Key);
            GameManager.Instance.city.GetComponent<City>().GetHouses()[nextHouseFired.Key].GetComponent<HouseFire>().StartFire();
        }
    }

    public void setEnable(bool value)
    {
        _enable = value;
    }

    public void StartFire()
    {
        StartCoroutine(FireTimeOut());
    }
}