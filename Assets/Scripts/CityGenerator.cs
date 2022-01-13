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
        
        CityGenerator.generateHouses(map);

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

    private static void generateHouses(EnumElementCity [,] map)
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                EnumElementCity current = map[x,y];

                if(current == EnumElementCity.STREET)
                {
                    if(CityGenerator.inRangeMap(x+1, WIDTH) && map[x+1, y] == EnumElementCity.GROUND)
                    {
                        map[x+1, y] = EnumElementCity.HOUSE;
                    }

                    if(CityGenerator.inRangeMap(x-1, WIDTH) && map[x-1, y] == EnumElementCity.GROUND)
                    {
                        map[x-1, y] = EnumElementCity.HOUSE;
                    }

                    if(CityGenerator.inRangeMap(y+1, HEIGHT) && map[x, y+1] == EnumElementCity.GROUND)
                    {
                        map[x, y+1] = EnumElementCity.HOUSE;
                    }

                    if(CityGenerator.inRangeMap(y-1, HEIGHT) && map[x, y-1] == EnumElementCity.GROUND)
                    {
                        map[x, y-1] = EnumElementCity.HOUSE;
                    }
                }
            }
        }
    }

    private static bool inRangeMap(int index, int LEN)
    {
        return index >= 0 && index < LEN;
    }
}
