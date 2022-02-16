using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLauncherDemo : MonoBehaviour
{
    [SerializeField] public List<Character> players,enemies;
    [SerializeField] public BattleLauncher launcher;

    public void Launch()
    {
        launcher. PrepareBattle(enemies, players);
    }
}
