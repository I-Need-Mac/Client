using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string characterName;
    public int hp;
    public int attack;
    public int criRatio;
    public int criDamage;
    public int coolDown;
    public int hpRegen;
    public int energyRegen;
    public int shield;
    public int projectileAdd;
    public int moveSpeed;
    public int getItemRange;

    public void a()
    {

    }
}