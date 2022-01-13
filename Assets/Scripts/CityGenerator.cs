using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator
{
    public const int WIDTH = 10;
    public const int HEIGHT = 10;

    public static EnumElementCity[,] GeneratorMap()
    {
        EnumElementCity [,] map = new EnumElementCity[WIDTH, HEIGHT];

        map[WIDTH/2, HEIGHT/2] = EnumElementCity.GROUND;

        CityGenerator.generateStreets(map, new Vector2Int(WIDTH/2, HEIGHT/2));

        return map;
    }

    private static void generateStreets(EnumElementCity [,] map, Vector2Int center)
    {
        for (int index = 0; index < WIDTH; index++)
        {
            map[center.x, index] = EnumElementCity.STREET;
            map[index, center.y] = EnumElementCity.STREET;
        }
    }
}
