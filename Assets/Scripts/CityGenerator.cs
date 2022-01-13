using UnityEngine;

public class CityGenerator
{
    public static EnumElementCity[,] GeneratorMap(int width, int height)
    {
        EnumElementCity[,] map = new EnumElementCity[width, height];

        map[width / 2, height / 2] = EnumElementCity.GROUND;

        CityGenerator.GenerateStreets(map, new Vector2Int(width / 2, height / 2), width);

        CityGenerator.GenerateHouses(map, width, height);

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

    private static bool InRangeMap(int index, int LEN)
    {
        return index >= 0 && index < LEN;
    }
}
