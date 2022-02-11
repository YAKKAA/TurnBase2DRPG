using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string CharacterName;
    public int health;
    public int maxHealth;
    public int attackPower;
    public int defencePower;
    public int manaPoint;
    public List <Spell> spells;
    
    public void Hurt (int amount)
    {
        int damageAmount = Random.Range(0,1)*(amount - defencePower);
       
        health = Mathf.Max(health-damageAmount,0);
        if(health == 0)
        {
            Die();
        }
    }

    public void Heal (int amount)
    {
        int healAmount = Random.Range(0,1)* (int) (amount + (maxHealth * 0.11f)) ;
       
        health = Mathf.Min(health+healAmount,maxHealth);
    }

    public void Defend()
    {
        defencePower +=(int)(defencePower * 0.11f );
    }
    public bool CastSpell (Spell spell, Character targetCharacter)
    {
        bool sucessCast = manaPoint >= spell.manaCost;
        if(sucessCast)
        {
            Spell spellToCast = Instantiate<Spell>(spell, transform.position, Quaternion.identity);
            manaPoint -= spell.manaCost;
            spellToCast.CastSpell(targetCharacter);
        }

        return sucessCast; 
    }
    public virtual void Die()
    {
        Destroy (this.gameObject);
    }

}

