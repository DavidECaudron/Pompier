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

    void Start()
    {
        EnumElementCity[,] map = CityGenerator.GeneratorMap(width, height);

        DebugShowMap(map);
        GenerateVisualMap(map);
    }

    private void DebugShowMap(EnumElementCity[,] map)
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


    private void GenerateVisualMap(EnumElementCity[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.localScale = new Vector3(.1f, .1f, .1f);
                plane.transform.position = new Vector3(x, 0, y);

                switch (map[x, y])
                {
                    case EnumElementCity.GROUND:
                        plane.GetComponent<MeshRenderer>().material = groundMat;
                        break;
                    case EnumElementCity.STREET:
                        plane.GetComponent<MeshRenderer>().material = streetMat;
                        break;
                    case EnumElementCity.HOUSE:
                        plane.GetComponent<MeshRenderer>().material = houseMat;

                        GameObject houseInstance = InstantiateHousePrefab(housePrefabs, new Vector3(x, 0, y));
                        break;

                    case EnumElementCity.CORNER_HOUSE:
                        plane.GetComponent<MeshRenderer>().material = cornerHouseMat;

                        GameObject cornerHouseInstance = InstantiateHousePrefab(cornerHousePrefabs, new Vector3(x, 0, y));
                        break;

                }

            }
        }
    }

    private GameObject InstantiateHousePrefab(GameObject[] prefabs, Vector3 pos)
    {
        int index = Random.Range(0, prefabs.Length);
        return Instantiate(prefabs[index], pos, Quaternion.identity, gameObject.transform);
    }

}
