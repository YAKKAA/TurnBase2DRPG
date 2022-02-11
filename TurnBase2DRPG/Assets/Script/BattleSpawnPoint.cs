using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpawnPoint : MonoBehaviour
{
    public Character Spawn(Character character)
    {
        Character character1ToSpawn = Instantiate<Character>(character, this.transform);
        return character1ToSpawn;
    }
}
