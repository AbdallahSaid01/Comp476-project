using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject buyMenuUI;
    private PlayerController pc;
    //If these are changed, change the text in the buy Menu!
    [SerializeField] private int HpCost;
    [SerializeField] private int ManaCost;
    [SerializeField] private int AtkSPDCost;
    [SerializeField] private int AtkDMGCost;
    /////////////////////////////////////////////////////////

    private void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void buyHP()
    {
        
        if (pc.getGold() > HpCost)
        {
            pc.Heal();
            pc.setGold(pc.getGold() - HpCost);
        }
            
    }

    public void buyMana()
    {
        if (pc.getGold() > ManaCost)
        {
            pc.RegenMana();
            pc.setGold(pc.getGold() - HpCost);
        }
            
    }

    public void buyAtkSPD()
    {
        if(pc.getGold() > AtkSPDCost)
        {
            pc.incrementLightAttackSPD();
            pc.incrementHeavyAttackSPD();
            pc.setGold(pc.getGold() - AtkSPDCost);
        }
            
    }

    public void buyAtkDMG()
    {
        if (pc.getGold() > AtkDMGCost)
        {
            pc.incrementLightAttackDMG();
            pc.incrementHeavyAttackDMG();
            pc.setGold(pc.getGold() - AtkDMGCost);
        }
    }

    public void Resume()
    {
        buyMenuUI.SetActive(false);
        pc.input.Controls.Enable();
        Time.timeScale = 1f;
        GamePaused = false;
    }

    private void Pause()
    {
        buyMenuUI.SetActive(true);
        pc.input.Controls.Disable();
        Time.timeScale = 0f;
        GamePaused = true;
    }
}
