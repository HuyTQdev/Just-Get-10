using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObject/DataDragon")]
public class DataDragon : ScriptableObject
{
    public List<Dragon> dragons;
}
[System.Serializable]
public class Dragon
{
    public int id;
    public GameObject dragon;
    public Sprite sprite;
    public int idFrame;
    public string effectSpawnName, soundSpawnName, idleAnimName;
}