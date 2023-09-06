using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public int seed;
    
    [Header("Parameters")] 
    public int roomNumber;
    public int floorNumber;

    private RoomGenerator[] _rooms;
    
    void Start()
    {
        if (seed != 0)
            RandomService.SetSeed(seed);
        else
            seed = RandomService.Seed;
    }

    void Update()
    {
        
    }
}
