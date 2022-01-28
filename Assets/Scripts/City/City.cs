using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class City : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private ElementCityParameter _houseParameter;
    [SerializeField] private ElementCityParameter _decorativeHouseParameter;
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

    public void GenerateCity()
    {
        if (_cityParameter == null)
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
        RotateCallMap();
    }

    public void ResetCity()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void LoadDictMap()
    {
        _dictMap = new Dictionary<Position, CellMap>();

        foreach (Transform child in gameObject.transform)
        {
            CellData data = child.GetComponent<CellData>();
            if (data == null) continue;

            _dictMap.Add(data.Position, data.CellMap);
        }
    }

    public Dictionary<Position, CellMap> GetMap()
    {
        if (_dictMap == null)
        {
            LoadDictMap();
        }
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
        GameObject cellObj = new GameObject(name);
        cellObj.transform.position = position;
        cellObj.transform.parent = gameObject.transform;

        cellObj.AddComponent<CellData>();

        return cellObj;
    }


    //TODO need refacto
    private Transform GetTranformToLook(KeyValuePair<Position, CellMap> kvp)
    {
        if (kvp.Value.CellType == EnumElementCity.GROUND) return null;
        if (kvp.Value.CellType == EnumElementCity.CORNER_HOUSE)
        {
            //autres
            return _dictMap[kvp.Key].Mesh.transform;
        }
        else
        {
            //regarde la route à coté de sois
            Position posToCheck = new Position() { X = kvp.Key.X + 1, Y = kvp.Key.Y };
            if (_dictMap.ContainsKey(posToCheck))
            {
                if (_dictMap[posToCheck].CellType == EnumElementCity.STREET)
                {
                    return _dictMap[posToCheck].Mesh.transform;
                }
            }

            posToCheck = new Position() { X = kvp.Key.X - 1, Y = kvp.Key.Y };
            if (_dictMap.ContainsKey(posToCheck))
            {
                if (_dictMap[posToCheck].CellType == EnumElementCity.STREET)
                {
                    return _dictMap[posToCheck].Mesh.transform;
                }
            }

            posToCheck = new Position() { X = kvp.Key.X, Y = kvp.Key.Y + 1 };
            if (_dictMap.ContainsKey(posToCheck))
            {
                if (_dictMap[posToCheck].CellType == EnumElementCity.STREET)
                {
                    return _dictMap[posToCheck].Mesh.transform;
                }
            }

            posToCheck = new Position() { X = kvp.Key.X, Y = kvp.Key.Y - 1 };
            if (_dictMap.ContainsKey(posToCheck))
            {
                if (_dictMap[posToCheck].CellType == EnumElementCity.STREET)
                {
                    return _dictMap[posToCheck].Mesh.transform;
                }
            }
        }

        return null;
    }

    private void RotateCallMap()
    {
        foreach (KeyValuePair<Position, CellMap> kvp in _dictMap)
        {
            Transform posToLook = GetTranformToLook(kvp);
            if (posToLook == null) continue;

            kvp.Value.Mesh.transform.LookAt(posToLook);
        }
    }

    private void GenerateVisualMap(int scale)
    {
        foreach (KeyValuePair<Position, CellMap> kvp in _dictMap)
        {
            Vector3 objectPosition = new Vector3(kvp.Key.X * scale, 0, kvp.Key.Y * scale);

            GameObject cellObj = GenerateCellGameObject(objectPosition, $"{kvp.Key.X},{kvp.Key.Y}");
            GameObject mesh = null;
            ElementCityParameter param = null;

            switch (kvp.Value.CellType)
            {
                case EnumElementCity.GROUND:
                    param = _groundParameter;
                    break;
                case EnumElementCity.STREET:
                    param = _streetParameter;
                    break;
                case EnumElementCity.HOUSE:
                    param = _houseParameter;
                    break;
                case EnumElementCity.DECORATIVE_HOUSE:
                    param = _decorativeHouseParameter;
                    break;
                case EnumElementCity.CORNER_HOUSE:
                    param = _cornerHouseParameter;
                    break;
                case EnumElementCity.OBSTACLE:
                    param = _obstacleParameter;
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
            if (param.Meshes.Length > 0)
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
            mapData.Position = kvp.Key;
        }
    }

    private GameObject InstantiatePrefab(GameObject[] prefabs, Transform parent)
    {
        if (prefabs.Length == 0) return null;
        int index = Random.Range(0, prefabs.Length);
        return InstantiatePrefab(prefabs[index], parent);
    }

    private GameObject InstantiatePrefab(GameObject prefabs, Transform parent)
    {
        if (prefabs == null) return null;
        //GameObject instance = Instantiate(prefabs, Vector3.zero, Quaternion.AngleAxis(rotationAngle, Vector3.up), parent);
        GameObject instance = Instantiate(prefabs, Vector3.zero, Quaternion.identity, parent);
        instance.transform.localPosition = Vector3.zero;
        return instance;
    }
}