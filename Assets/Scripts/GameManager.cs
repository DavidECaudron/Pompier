using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {        
        if(Instance != null)
        {
            Debug.LogError("Instance of GameManager already exist");
            Destroy(gameObject);
        }

        Instance = this;
    }    
}
