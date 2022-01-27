using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class City : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private ElementCityParameter _houseParameter;
    [SerializeField] private ElementCityParameter _cornerHouseParameter;
    [SerializeField] private ElementCityParameter _streetParameter;
    [SerializeField] private ElementCityParameter _groundParameter;
    [SerializeField] private ElementCityParameter _obstacleParameter;
    [SerializeField] private bool _generateLogMap = true;

    [Header("Map Size")]
    [SerializeField] private CityParameter _cityParameter;

    private Dictionary<Position, CellMap> _dictMap;

    public int Size
    {
        get { return _cityParameter.Size; }
    }

    private void Start()
    {
        if(_dictMap == null)
        {
            _dictMap = new Dictionary<Position, CellMap>();

            //charger le dico avec les objets de la map

            //boucler dans chaque enfant, et pou chaque enfant récupérer ses enfant pour les charger dans cellmap du dico
        }
    }

    public void GenerateCity()
    {
        if(_cityParameter == null)
        {
            Debug.LogError("City parameter cant be nothing");
            return;
        }

        ResetCity();
        _dictMap = CityGenerator.GenerateMap(_cityParameter.Size);

        if (_generateLogMap)
        {
            DebugShowMap(_cityParameter.Size);
        }

        GenerateVisualMap(_cityParameter.Scale);
    }

    public void ResetCity()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public Dictionary<Position, CellMap> GetMap()
    {
        return _dictMap;
    }

    private void DebugShowMap(int size)
    {
        int index = 0;
        string str = "";
        foreach (KeyValuePair<Position, CellMap> kvp in _dictMap)
        {
            if (index == 0)
            {
                str += '\n';
            }

            switch (kvp.Value.CellType)
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

    private void GeneratePlane(Transform parent, int scale, ElementCityParameter elementCityParameter)
    {
        GameObject elementCity = GameObject.CreatePrimitive(PrimitiveType.Plane);
        elementCity.transform.parent = parent;
        elementCity.transform.localScale = new Vector3(.1f * scale, .1f * scale, .1f * scale);
        elementCity.transform.localPosition = Vector3.zero;
        elementCity.GetComponent<MeshRenderer>().material = elementCityParameter.Material;
    }

    private GameObject GenerateCellGameObject(Vector3 position, string name)
    {
        GameObject cellObj = new GameObject(name, typeof(CellData));
        cellObj.transform.position = position;
        cellObj.transform.parent = gameObject.transform;

        return cellObj;
    }

    private void GenerateVisualMap(int scale)
    {
        foreach (KeyValuePair<Position, CellMap> kvp in _dictMap)
        {
            Vector3 obectPosition = new Vector3(kvp.Key.X * scale, 0, kvp.Key.Y * scale);

            GameObject cellObj = GenerateCellGameObject(obectPosition, $"{kvp.Key.X},{kvp.Key.Y}");            
            GameObject mesh = null;
            ElementCityParameter param = null;

            switch (kvp.Value.CellType)
            {
                case EnumElementCity.GROUND:
                    param = _groundParameter;
                    break;
                case EnumElementCity.STREET:
                    param = _streetParameter;
                    //mesh = InstantiatePrefab(_streetParameter.Prefabs, cellObj.transform);
                    //if (_streetParameter.UseCustomScale)
                    //{
                    //    mesh.transform.localScale = _streetParameter.Scale;
                    //}                    

                    break;
                case EnumElementCity.HOUSE:
                    param = _houseParameter;
                    //mesh = InstantiatePrefab(_houseParameter.Prefabs, cellObj.transform, GetRotationForHouse(new Vector2Int(kvp.Key.X, kvp.Key.Y)));
                    break;
                case EnumElementCity.CORNER_HOUSE:
                    param = _cornerHouseParameter;
                    //Vector3 rot = GetRotationForCornerHouse(new Vector2Int(kvp.Key.X, kvp.Key.Y));
                    //mesh = InstantiatePrefab(_cornerHouseParameter.Prefabs, cellObj.transform);
                    //mesh.transform.LookAt(rot);

                    break;
                case EnumElementCity.OBSTACLE:
                    param = _obstacleParameter;
                    //mesh = InstantiatePrefab(_obstacleParameter.Prefabs, cellObj.transform);

                    //Transform graphics = null;
                    //foreach (Transform item in mesh.transform)
                    //{
                    //    graphics = item;
                    //}

                    //int index = Random.Range(0, _obstacleParameter.Meshes.Length);
                    //Instantiate(_obstacleParameter.Meshes[index], graphics);
                    break;
            }

            //Generate Plane
            if (param.GeneratePlane)
            {
                GeneratePlane(cellObj.transform, scale, param);
            }

            //Generate prefab
            mesh = InstantiatePrefab(param.Prefabs, cellObj.transform);
            if (param.UseCustomScale)
            {
                mesh.transform.localScale = param.Scale;
            }

            //Generate mesh child for prefab
            if(param.Meshes.Length > 0)
            {
                Transform graphics = null;
                foreach (Transform item in mesh.transform)
                {
                    graphics = item;
                }

                int index = Random.Range(0, param.Meshes.Length);
                Instantiate(_obstacleParameter.Meshes[index], graphics);
            }
            kvp.Value.Mesh = mesh;

            CellData mapData = cellObj.GetComponent<CellData>();
            mapData.CellMap = kvp.Value;
        }
    }

    private GameObject InstantiatePrefab(GameObject[] prefabs, Transform parent, float rotationAngle = 0f)
    {
        if (prefabs.Length == 0) return null;
        int index = Random.Range(0, prefabs.Length);
        return InstantiatePrefab(prefabs[index], parent, rotationAngle);
    }

    private GameObject InstantiatePrefab(GameObject prefabs, Transform parent, float rotationAngle = 0f)
    {
        if (prefabs == null) return null;
        GameObject instance = Instantiate(prefabs, Vector3.zero, Quaternion.AngleAxis(rotationAngle, Vector3.up), parent);
        instance.transform.localPosition = Vector3.zero;
        return instance;
    }

    //private Vector3 GetRotationForCornerHouse(Vector2Int coord)
    //{
    //    List<Vector2Int> coordinates = CityGenerator.GetCroos(_map, coord);

    //    List<Vector2Int> coordinatesStreet = new List<Vector2Int>();

    //    foreach (Vector2Int coordinate in coordinates)
    //    {
    //        if (_map[coordinate.x, coordinate.y] == EnumElementCity.STREET)
    //        {
    //            coordinatesStreet?.Add(coordinate);
    //        }
    //    }

    //    switch (coordinatesStreet.Count)
    //    {
    //        case 2:
    //            Vector2Int offset = coordinatesStreet[1] - coordinatesStreet[0];
    //            Vector2Int coorner = coord + (coordinatesStreet[1] - coord) + (coordinatesStreet[0] - coord);
    //            return new Vector3(coorner.x, 0, coorner.y);
    //        default:
    //            return new Vector3(coord.x, 0, coord.y);
    //    }
    //}

    //private float GetRotationForHouse(Vector2Int coord)
    //{
    //    if (CityGenerator.InRangeMap(coord.x + 1, _size))
    //    {
    //        if (_map[coord.x + 1, coord.y] == EnumElementCity.STREET)
    //        {
    //            return 180f;
    //        }
    //    }


    //    if (CityGenerator.InRangeMap(coord.x - 1, _size))
    //    {
    //        if (_map[coord.x - 1, coord.y] == EnumElementCity.STREET)
    //        {
    //            return 0f;
    //        }
    //    }


    //    if (CityGenerator.InRangeMap(coord.y + 1, _size))
    //    {
    //        if (_map[coord.x, coord.y + 1] == EnumElementCity.STREET)
    //        {
    //            return 90f;
    //        }
    //    }

    //    if (CityGenerator.InRangeMap(coord.y - 1, _size))
    //    {
    //        if (_map[coord.x, coord.y - 1] == EnumElementCity.STREET)
    //        {
    //            return -90;
    //        }
    //    }

    //    return 0f;
    //}

}