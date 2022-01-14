using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] Material houseMat;
    [SerializeField] Material cornerHouseMat;
    [SerializeField] Material streetMat;
    [SerializeField] Material groundMat;

    [SerializeField] GameObject[] housePrefabs;
    [SerializeField] GameObject[] cornerHousePrefabs;

    [SerializeField] [Min(10)] int width = 10;
    [SerializeField] [Min(10)] int height = 10;

    [SerializeField] [Min(1)] int scale = 1;

    private EnumElementCity[,] map;

    void Start()
    {
        map = CityGenerator.GeneratorMap(width, height);

        DebugShowMap();
        GenerateVisualMap();
    }

    private void DebugShowMap()
    {
        var index = 0;
        var str = "";
        foreach (var item in map)
        {
            if (index == 0)
            {
                str += '\n';
            }

            switch (item)
            {
                case EnumElementCity.GROUND:
                    str += 'G'; break;
                case EnumElementCity.STREET:
                    str += 'S'; break;
                case EnumElementCity.HOUSE:
                    str += 'H'; break;
            }

            index++;
            index %= 10;
        }

        Debug.Log(str);
    }


    private void GenerateVisualMap()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                GameObject elementCity = GameObject.CreatePrimitive(PrimitiveType.Plane);
                elementCity.transform.localScale = new Vector3(.1f, .1f, .1f);
                elementCity.transform.position = new Vector3(x*scale, 0, y*scale);

                switch (map[x, y])
                {
                    case EnumElementCity.GROUND:
                        elementCity.GetComponent<MeshRenderer>().material = groundMat;
                        break;
                    case EnumElementCity.STREET:
                        elementCity.GetComponent<MeshRenderer>().material = streetMat;
                        break;
                    case EnumElementCity.HOUSE:
                        elementCity.GetComponent<MeshRenderer>().material = houseMat;

                        GameObject houseInstance = InstantiateHousePrefab(housePrefabs, new Vector3(x*scale, 0, y*scale), GetRotationForHouse(new Vector2Int(x, y))) ;
                        break;

                    case EnumElementCity.CORNER_HOUSE:
                        elementCity.GetComponent<MeshRenderer>().material = cornerHouseMat;

                        GameObject cornerHouseInstance = InstantiateHousePrefab(cornerHousePrefabs, new Vector3(x*scale, 0, y*scale), 0f);
                        break;

                }

            }
        }
    }

    private GameObject InstantiateHousePrefab(GameObject[] prefabs, Vector3 pos, float rotationAngle)
    {
        int index = Random.Range(0, prefabs.Length);
        return Instantiate(prefabs[index], pos, Quaternion.AngleAxis(rotationAngle, Vector3.up), gameObject.transform);
    }


    private float GetRotationForHouse(Vector2Int coord)
    {
        if (CityGenerator.InRangeMap(coord.x + 1, width))
        {
            if (map[coord.x + 1, coord.y] == EnumElementCity.STREET)
            {
                return 180f;
            }
        }


        if (CityGenerator.InRangeMap(coord.x - 1, width))
        {
            if (map[coord.x - 1, coord.y] == EnumElementCity.STREET)
            {
                return 0f;
            }
        }


        if (CityGenerator.InRangeMap(coord.y + 1, height))
        {
            if (map[coord.x, coord.y+1] == EnumElementCity.STREET)
            {
                return 90f;
            }
        }

        if (CityGenerator.InRangeMap(coord.y - 1, height))
        {
            if (map[coord.x, coord.y-1] == EnumElementCity.STREET)
            {
                return - 90;
            }
        }

        return 0f;
    }

}
