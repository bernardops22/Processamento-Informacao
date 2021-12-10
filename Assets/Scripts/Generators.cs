using System;
using UnityEngine;
using Random = System.Random;

public static class Generators
{
    
    public static double DamageFromAttacks()
    {
        double U = new Random().Next(0, 2);
        if(U >= 0 && U < 1/6D)
            return (-2/15D + Math.Sqrt(4/225D - 2/75D * (2/3D - U))) * 75;
        if(U >= 1/6D && U <= 5/6D)
            return (U - 0.5) * 15;
        if(U > 5/6D && U <=1)
            return (2/15D - Math.Sqrt(4/225D + 2/75D * (1/3D - U))) * 75;
        return -1500;
    }

    public static double CatchRate(){
        double U = new Random().Next(0, 2);
        return - Math.Log(1 - U);
    }

    public static double Rarity(){
        double U = new Random().Next(0, 2);
        if(U == 0)
            return 0;
        if(U > 0 && U < 11/1024D)
            return 1;
        if(U >= 11/1024D && U < 7/128D)
            return 2;
        if(U >= 7/128D && U < 11/64D)
            return 3;
        if(U >= 11/64D && U < 193/512D)
            return 4;
        if(U >= 193/512D && U < 319/512D)
            return 5;
        if(U >= 319/512D && U < 53/64D)
            return 6;
        if(U >= 53/64D && U < 121/128D)
            return 7;
        if(U >= 121/128D && U < 1013/1024D)
            return 8;
        if(U >= 1013/1024D && U < 1023/1024D)
            return 9;
        if(U >= 1023/1024D && U < 1)
            return 10;
        return -1;
    }

    public static int NewEncounter(){
        double U = new Random().Next(0, 2);
        if(U >= 0 && U < 0.4)
            return 0;
        if(U >= 0.4 && U <= 1)
            return 1;
        return -1;
    }

    public static int firstPikamon(double[] vector)
    {
        double U = new Random().Next(0, 1);
        if (U >= 0 && U < vector[0])
            return 0;
        if (U >= vector[0] && U <= vector[0] + vector[1])
            return 1;
        if (U >= vector[0] + vector[1] && U <= vector[0] + vector[1] + vector[2])
            return 2;
        return -1;
    }
}
