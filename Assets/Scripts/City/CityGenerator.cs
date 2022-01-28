using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CityGenerator
{
    public static EnumElementCity[,] GeneratorMap(int size)
    {
        EnumElementCity[,] map = new EnumElementCity[size, size];

        map[size / 2, size / 2] = EnumElementCity.GROUND;

        CityGenerator.GenerateRandomStreets(map, new Vector2Int(size / 2, size / 2), size);
        CityGenerator.GenerateHouses(map, size, size);
        CityGenerator.GenerateCornerHouses(map, size, size);
        CityGenerator.GenerateObstacle(map, size, size);

        return map;
    }

    public static Dictionary<Position, CellMap> GenerateMap(int size)
    {
        Debug.Log("sdsd");
        EnumElementCity[,] map = GeneratorMap(size);
        Dictionary<Position, CellMap> dict = new Dictionary<Position, CellMap>();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Position pos = new Position() { X = x, Y = y };
                CellMap cell = new CellMap() { CellType = map[x, y] };
                dict.Add(pos, cell);
            }
        }

        return dict;
    }

    private static void GenerateRandomStreets(EnumElementCity[,] map, Vector2Int center, int size)
    {
        var position = 0;
        do
        {
            position = Random.Range(position + 1, size - 1);
            
            for (int i = 0; i < size; i++)
            {
                map[position, i] = EnumElementCity.STREET;
            }
            position++;
        }while(position + 1 < size -1);
        
        position = 0;
        do
        {
            position = Random.Range(position + 1, size - 1);
            
            for (int i = 0; i < size; i++)
            {
                map[i, position] = EnumElementCity.STREET;
            }
            position++;
        }while(position + 1 < size -1);
    }

    private static void GenerateHouses(EnumElementCity[,] map, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                EnumElementCity current = map[x, y];

                if (current == EnumElementCity.STREET)
                {
                    if (CityGenerator.InRangeMap(x + 1, width) && map[x + 1, y] == EnumElementCity.GROUND)
                    {
                        map[x + 1, y] = EnumElementCity.HOUSE;
                    }

                    if (CityGenerator.InRangeMap(x - 1, width) && map[x - 1, y] == EnumElementCity.GROUND)
                    {
                        map[x - 1, y] = EnumElementCity.HOUSE;
                    }

                    if (CityGenerator.InRangeMap(y + 1, height) && map[x, y + 1] == EnumElementCity.GROUND)
                    {
                        map[x, y + 1] = EnumElementCity.HOUSE;
                    }

                    if (CityGenerator.InRangeMap(y - 1, height) && map[x, y - 1] == EnumElementCity.GROUND)
                    {
                        map[x, y - 1] = EnumElementCity.HOUSE;
                    }
                }
            }
        }
    }

    private static void GenerateCornerHouses(EnumElementCity[,] map, int width, int height)
    {
        List<EnumElementCity> enums = new List<EnumElementCity>(){ EnumElementCity.STREET, EnumElementCity.OBSTACLE};
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                EnumElementCity current = map[x, y];

                if (current != EnumElementCity.HOUSE) continue;

                bool axisY = false;
                bool axisX = false;

                if (CityGenerator.InRangeMap(y + 1, height) && CityGenerator.InRangeMap(y - 1, height))
                {
                    axisY = enums.Contains(map[x, y - 1]) || enums.Contains(map[x, y + 1]);
                }

                if (CityGenerator.InRangeMap(x + 1, width) && CityGenerator.InRangeMap(x - 1, width))
                {
                    axisX = enums.Contains(map[x - 1, y])|| enums.Contains(map[x + 1, y]);
                }

                if (axisX && axisY)
                {
                    map[x, y] = EnumElementCity.CORNER_HOUSE;
                }
            }
        }
    }

    private static void GenerateObstacle(EnumElementCity[,] map, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                EnumElementCity current = map[x, y];

                if (current == EnumElementCity.STREET)
                {
                    List<Vector2Int> positions = GetCroos(map, new Vector2Int(x, y));
                    int i = 0;

                    foreach (Vector2Int pos in positions)
                    {
                        if (map[pos.x, pos.y] == EnumElementCity.STREET || map[pos.x, pos.y] == EnumElementCity.OBSTACLE) i++;
                    }

                    if (i < 2) map[x, y] = EnumElementCity.OBSTACLE;
                }
            }
        }
    }

    public static bool InRangeMap(int index, int LEN)
    {
        return index >= 0 && index < LEN;
    }

    public static List<Vector2Int> GetDiagonalCroos(EnumElementCity[,] map, Vector2Int position)
    {
        var indexs = new[] { (1, 1), (1, -1), (-1, -1), (-1, 1) };

        return GetTiles(map, position, indexs);
    }

    public static List<Vector2Int> GetCroos(EnumElementCity[,] map, Vector2Int position)
    {
        var indexs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

        return GetTiles(map, position, indexs);
    }

    public static List<Vector2Int> GetArround(EnumElementCity[,] map, Vector2Int position)
    {
        var indexs = new[] { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) };

        return GetTiles(map, position, indexs);
    }

    private static List<Vector2Int> GetTiles(EnumElementCity[,] map, Vector2Int position, (int, int)[] indexs)
    {
        var elementsArround = new List<Vector2Int>();

        int width = map.GetLength(0);
        int height = map.GetLength(1);

        foreach (var index in indexs)
        {

            var index_x = position.x + index.Item1;
            var index_y = position.y + index.Item2;

            if (InRangeMap(index_x, width) && InRangeMap(index_y, height))
            {
                elementsArround.Add(new Vector2Int(index_x, index_y));
            }
        }

        return elementsArround;
    }
}
