using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Objectives
{
    private static objective[,] levelObjectives = new objective[5,10]{
        {
            new objective(5,0), new objective(6,0), new objective(8,0), new objective(10,0), new objective(10,0), new objective(10,0), new objective(10,0), new objective(10,0), new objective(10,0), new objective(10,0)
        },
        {
            new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0)
        },
        {
            new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0)
        },
        {
            new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0)
        },
        {
            new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0), new objective(0,0)
        }
    };

    public static objective levelData(int dozen, int figure){
        return levelObjectives[dozen - 1, figure - 1];
    }
}

public struct objective
{
    public int slotCollect;
    public int enemyKill;

    public objective(int slot, int kill){
        slotCollect = slot;
        enemyKill = kill;
    }
}
