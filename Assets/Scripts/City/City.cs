using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class City : MonoBehaviour
{
<<<<<<< HEAD:Assets/Scripts/City/City.cs
    [HideInInspector] public GameObject center;

=======
<<<<<<< HEAD
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
=======
>>>>>>> c578a4a9d73b6bfd6a264298c05c78daee3d5b2a:Assets/Scripts/City.cs
    [Header("Materials")]
    [SerializeField] private Material _houseMat;
    [SerializeField] private Material _cornerHouseMat;
    [SerializeField] private Material _streetMat;
    [SerializeField] private Material _groundMat;
    [SerializeField] private Material _obstacleMat;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] _housePrefabs;
    [SerializeField] private GameObject[] _cornerHousePrefabs;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private GameObject[] _obstacleMeshes;
    [SerializeField] private GameObject roadPrefab;

    [Header("Map Size")]
    [SerializeField] [Min(10)] private int _width = 10;
    [SerializeField] [Min(10)] private int _height = 10;
    [SerializeField] [Min(1)] private int _scale = 1;

    private EnumElementCity[,] _map;

    public void GenerateCity()
    {
        ResetCity();
        _map = CityGenerator.GeneratorMap(_width, _height);

<<<<<<< HEAD:Assets/Scripts/City/City.cs
        DebugShowMap(_width);
=======
>>>>>>> 57857d1d46a3439bc62a28bdd27e393ad8c05dd5
        DebugShowMap();
>>>>>>> c578a4a9d73b6bfd6a264298c05c78daee3d5b2a:Assets/Scripts/City.cs
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

    public EnumElementCity[,] GetMap()
    {
        return this._map;
    }

    private void DebugShowMap(int size)
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
                case EnumElementCity.CORNER_HOUSE:
                    str += 'C'; break;
                case EnumElementCity.OBSTACLE:
                    str += 'O'; break;
            }

            index++;
            index %= size;
        }

        Debug.Log(str);
    }

    private void GenerateVisualMap()
    {
        for (int x = 0; x < _map.GetLength(0); x++)
        {
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                Vector3 obectPosition = new Vector3(x * _scale, 0, y * _scale);

                GameObject elementCity = GameObject.CreatePrimitive(PrimitiveType.Plane);
                elementCity.transform.parent = gameObject.transform;
                elementCity.transform.localScale = new Vector3(.1f * _scale, .1f * _scale, .1f * _scale);
                elementCity.transform.position = obectPosition;

                switch (_map[x, y])
                {
                    case EnumElementCity.GROUND:
                        elementCity.GetComponent<MeshRenderer>().material = _groundMat;
                        break;
                    case EnumElementCity.STREET:
                        //elementCity.GetComponent<MeshRenderer>().material = _streetMat;
                        DestroyImmediate(elementCity);
                        GameObject road = InstantiatePrefab(roadPrefab, obectPosition, 0);
                        road.transform.localScale = new Vector3(.1f * _scale, .1f * _scale, .1f * _scale);
                        break;

                    case EnumElementCity.OBSTACLE:
                        elementCity.GetComponent<MeshRenderer>().material = _obstacleMat;

                        GameObject obstacleInstance = InstantiatePrefab(_obstaclePrefab, obectPosition);

                        Transform graphics = null;
                        foreach (Transform item in obstacleInstance.transform)
                        {
                            graphics = item;
                        }

                        int index = Random.Range(0, _obstacleMeshes.Length);
                        Instantiate(_obstacleMeshes[index], graphics);
                        break;

                    case EnumElementCity.HOUSE:
                        elementCity.GetComponent<MeshRenderer>().material = _houseMat;

                        GameObject houseInstance = InstantiatePrefab(_housePrefabs, obectPosition, GetRotationForHouse(new Vector2Int(x, y)));
                        if (x == _map.GetLength(0) / 2 && y == _map.GetLength(1) / 2)
                        {
                            center = houseInstance;
                        }
                        break;

                    case EnumElementCity.CORNER_HOUSE:
                        elementCity.GetComponent<MeshRenderer>().material = _cornerHouseMat;

                        Vector3 rot = GetRotationForCornerHouse(new Vector2Int(x, y));

                        GameObject cornerHouseInstance = InstantiatePrefab(_cornerHousePrefabs, obectPosition);
                        cornerHouseInstance.transform.LookAt(rot);
                        break;
                }
            }
        }
    }

    private GameObject InstantiatePrefab(GameObject[] prefabs, Vector3 pos, float rotationAngle = 0f)
    {
        int index = Random.Range(0, prefabs.Length);
        return InstantiatePrefab(prefabs[index], pos, rotationAngle);
    }

    private GameObject InstantiatePrefab(GameObject prefabs, Vector3 pos, float rotationAngle = 0f)
    {
        return Instantiate(prefabs, pos, Quaternion.AngleAxis(rotationAngle, Vector3.up), gameObject.transform);
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