using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUiControl : MonoBehaviour
{
    [SerializeField] public GameObject spellPanel;
    [SerializeField] public Button[] actionButtons;
    [SerializeField] public Button[] button;
    [SerializeField] public Text[] characterInfo;
    // Start is called before the first frame update
    void Start()
    {
        spellPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin,ray.direction);
            if(hitInfo.collider != null && hitInfo.collider.CompareTag("character"))
            {
                BattleController.Instance.SelectCharacter(hitInfo.collider.GetComponent<Character>());
            }
        }
    }


    public void ToggelSpellPanel(bool state)
    {
        if(state == true)
        {
            BuildSpellList(BattleController.Instance.GetCurrentCharacter().spells);

        }

    }

    public void Toggleactionstate(bool state)
    {
        ToggelSpellPanel(state);
        foreach(Button button in actionButtons)
        {
            button.interactable = state;
        }
    }
    
    public void BuildSpellList(List<Spell>spells)
    {
        if(spellPanel.transform.childCount > 0)
        {
            foreach(Button button in spellPanel.transform.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }

        foreach(Spell spell in spells)
        {
            Button spellButton = Instantiate<Button>(button[0], spellPanel.transform);
            spellButton.GetComponentInChildren<Text>().text = spell.spellName;
            spellButton.onClick.AddListener(() => SelectSpell(spell));
        }
    }
    void SelectSpell(Spell spell)
    {
        BattleController.Instance.playerSelectedSpell = spell;
        BattleController.Instance.playerIsAttack = false;
    }
    void SelectAttack()
    {
        BattleController.Instance.playerSelectedSpell = null;
        BattleController.Instance.playerIsAttack = true;
    }

    public void UpadteCharacterUI()
    {
        for(int i = 0; i < BattleController.Instance.characters[0].Count; i++)
        {
            Character character = BattleController.Instance.characters[0][i];
            characterInfo[i].text = string.Format("{0}HP:{1}/{2}, MP:{3}",character.CharacterName,character.health,character.maxHealth , character.manaPoint);
        }
    }
    public void Defend()
    {
        BattleController.Instance.GetCurrentCharacter().Defend();
    }
}
