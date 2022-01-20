using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class City : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _houseMat;
    [SerializeField] private Material _cornerHouseMat;
    [SerializeField] private Material _streetMat;
    [SerializeField] private Material _groundMat;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] _housePrefabs;
    [SerializeField] private GameObject[] _cornerHousePrefabs;

    [Header("Map Size")]
    [SerializeField] [Min(10)] private int _width = 10;
    [SerializeField] [Min(10)] private int _height = 10;
    [SerializeField] [Min(1)] private int _scale = 1;

    private EnumElementCity[,] _map;

    void Start()
    {
        GenerateCity();
    }

    public void GenerateCity()
    {
        ResetCity();

        _map = CityGenerator.GeneratorMap(_width, _height);

        DebugShowMap();
        GenerateVisualMap();
    }

    public void ResetCity()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void DebugShowMap()
    {
        int index = 0;
        string str = "";
        foreach (EnumElementCity item in _map)
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
    }


    private void GenerateVisualMap()
    {
        for (int x = 0; x < _map.GetLength(0); x++)
        {
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                GameObject elementCity = GameObject.CreatePrimitive(PrimitiveType.Plane);
                elementCity.transform.parent = gameObject.transform;
                elementCity.transform.localScale = new Vector3(.1f* _scale, .1f * _scale, .1f * _scale);
                elementCity.transform.position = new Vector3(x * _scale, 0, y * _scale);

                switch (_map[x, y])
                {
                    case EnumElementCity.GROUND:
                        elementCity.GetComponent<MeshRenderer>().material = _groundMat;
                        break;
                    case EnumElementCity.STREET:
                        elementCity.GetComponent<MeshRenderer>().material = _streetMat;
                        break;
                    case EnumElementCity.HOUSE:
                        elementCity.GetComponent<MeshRenderer>().material = _houseMat;

                        GameObject houseInstance = InstantiateHousePrefab(_housePrefabs, new Vector3(x * _scale, 0, y * _scale), GetRotationForHouse(new Vector2Int(x, y)));
                        break;

                    case EnumElementCity.CORNER_HOUSE:
                        elementCity.GetComponent<MeshRenderer>().material = _cornerHouseMat;

                        Vector3 rot = GetRotationForCornerHouse(new Vector2Int(x, y));

                        GameObject cornerHouseInstance = Instantiate(_cornerHousePrefabs[0], new Vector3(x * _scale, 0, y * _scale), Quaternion.Euler(0, 0, 0), gameObject.transform);
                        cornerHouseInstance.transform.LookAt(rot);
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

    private Vector3 GetRotationForCornerHouse(Vector2Int coord)
    {
        List<Vector2Int> coordinates = CityGenerator.GetCroos(_map, coord);

        List<Vector2Int> coordinatesStreet = new List<Vector2Int>();

        foreach (Vector2Int coordinate in coordinates)
        {
            if (_map[coordinate.x, coordinate.y] == EnumElementCity.STREET)
            {
                coordinatesStreet?.Add(coordinate);
            }
        }

        switch (coordinatesStreet.Count)
        {
            case 2:
                Vector2Int offset = coordinatesStreet[1] - coordinatesStreet[0];
                Vector2Int coorner = coord + (coordinatesStreet[1] - coord) + (coordinatesStreet[0] - coord);
                return new Vector3(coorner.x, 0, coorner.y);
            default:
                return new Vector3(coord.x, 0, coord.y);
        }
    }

    private float GetRotationForHouse(Vector2Int coord)
    {
        if (CityGenerator.InRangeMap(coord.x + 1, _width))
        {
            if (_map[coord.x + 1, coord.y] == EnumElementCity.STREET)
            {
                return 180f;
            }
        }


        if (CityGenerator.InRangeMap(coord.x - 1, _width))
        {
            if (_map[coord.x - 1, coord.y] == EnumElementCity.STREET)
            {
                return 0f;
            }
        }


        if (CityGenerator.InRangeMap(coord.y + 1, _height))
        {
            if (_map[coord.x, coord.y + 1] == EnumElementCity.STREET)
            {
                return 90f;
            }
        }

        if (CityGenerator.InRangeMap(coord.y - 1, _height))
        {
            if (_map[coord.x, coord.y - 1] == EnumElementCity.STREET)
            {
                return -90;
            }
        }

        return 0f;
    }

}