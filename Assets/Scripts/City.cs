using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        debugShowMap(CityGenerator.GeneratorMap());
    }

    private void debugShowMap(EnumElementCity[,]  map)
    {
        var index = 0;
        var str = "";
        foreach (var item in map)
        {
            if(index == 0)
            {
                str += '\n';
            }
            
            switch (item)
            {
                case EnumElementCity.GROUND:
                    str += 'G';break;
                case EnumElementCity.STREET:
                    str += 'S';break;
                case EnumElementCity.HOUSE:
                    str += 'H';break;
            }
            
            index++;
            index%=10;
        }

        Debug.Log(str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
