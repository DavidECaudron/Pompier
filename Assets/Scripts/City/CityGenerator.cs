using UnityEngine;
using System.Collections.Generic;

public class CityGenerator
{
    public static EnumElementCity[,] GeneratorMap(int width, int height)
    {
        EnumElementCity[,] map = new EnumElementCity[width, height];

        map[width / 2, height / 2] = EnumElementCity.GROUND;

        CityGenerator.GenerateStreets(map, new Vector2Int(width / 2, height / 2), width);
        CityGenerator.GenerateHouses(map, width, height);
        CityGenerator.GenerateCornerHouses(map, width, height);
        CityGenerator.GenerateObstacle(map, width, height);

        return map;
    }

    private static void GenerateStreets(EnumElementCity[,] map, Vector2Int center, int width)
    {
        for (int index = 0; index < width; index++)
        {
            map[center.x, index] = EnumElementCity.STREET;
            map[index, center.y] = EnumElementCity.STREET;
        }
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
                    axisY = map[x, y - 1] == EnumElementCity.STREET || map[x, y + 1] == EnumElementCity.STREET;
                }


                if (CityGenerator.InRangeMap(x + 1, width) && CityGenerator.InRangeMap(x - 1, width))
                {
                    axisX = map[x - 1, y] == EnumElementCity.STREET || map[x + 1, y] == EnumElementCity.STREET;
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
