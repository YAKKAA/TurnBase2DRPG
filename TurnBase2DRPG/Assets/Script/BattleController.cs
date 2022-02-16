using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleController : MonoBehaviour
{   
    public static BattleController Instance {get; set;}
    public Dictionary<int,List<Character>> characters = new Dictionary<int, List<Character>>();
    public int characterTurnIndex;
    public int activeTurn;
    public Spell playerSelectedSpell;
    public bool playerIsAttack;

    [SerializeField] public BattleSpawnPoint[] spawnPoints;
    [SerializeField] public ButtonUiControl uiController;
 
    public void Start() 
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        
        characters.Add(0, new List<Character>());
        characters.Add(1, new List<Character>());

        FindObjectOfType<BattleLauncher>().Launch();
    }

    public Character GetRandomPlayer()
    {
        return characters[0][Random.Range(0,characters[0].Count -1)];
    }

    public Character GetWeakeastUnit()
    {
        Character weakestUnit = characters[1][0];
        foreach(Character character in characters [1])
        {
            if(character.health < weakestUnit.health)
            {
                weakestUnit = character;
            }
        }
        return weakestUnit;
    }

    public void NextTurn()
    {
        activeTurn = activeTurn == 0 ? 1:0;
    }

    public void NextAct()
    {
        if(characters[0].Count >0 && characters[1].Count >0) 
        {
            if (characterTurnIndex < characters[activeTurn].Count -1)
            {
                characterTurnIndex++;
            }
            else
            {
                NextTurn();
            }

            switch(activeTurn)
            {
                case 0:
                    uiController.Toggleactionstate(true);
                    uiController.BuildSpellList(GetCurrentCharacter().spells);
                    break;
                
                case 1:
                    StartCoroutine(PerformAct());
                    uiController.Toggleactionstate(false);
                    break;
                
            }
        }
        else
        {
            Debug.Log("battle is finished");
        }
    }

    IEnumerator PerformAct()
    {
        yield return new WaitForSeconds(.75f);
        if(GetCurrentCharacter().health > 0)
        {
            GetCurrentCharacter().GetComponent<Enemy>().Act();
        }
        uiController.UpadteCharacterUI();
        yield return new WaitForSeconds(1f);
        NextAct();
    }

    public void SelectCharacter(Character character)
    {
        if(playerIsAttack)
        {
            DoAttack(GetCurrentCharacter(),character); 
        }
        else if(playerSelectedSpell != null)
        {
            if(GetCurrentCharacter().CastSpell(playerSelectedSpell,character))
            {
                uiController.UpadteCharacterUI();
                NextAct();
            }
            else
            {
                Debug.LogWarning("Not Enough Mana");
            }
        }
    }

    public void DoAttack(Character attacker, Character target)
    {
        target.Hurt(attacker.attackPower);
    }

    public void StartBattle(List<Character> players, List<Character> enemies)
    {
        for(int i = 0; i< players.Count; i++)
        {
            characters[0].Add(spawnPoints[i+3].Spawn(players[i]));
        }

        for(int i = 0; i< enemies.Count; i++)
        {
            characters[1].Add(spawnPoints[i].Spawn(enemies[i]));
        }
    }
    public Character GetCurrentCharacter()
    {
        return characters[activeTurn][characterTurnIndex];
    }
}
