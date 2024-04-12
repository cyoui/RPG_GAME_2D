using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodeg")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirrage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirrageDodgeButton;
    public bool dodgeMirrageUnlocked;


    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirrageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirrageDodge);
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockDodge();
        UnlockMirrageDodge();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirrageDodge()
    {
        if (unlockMirrageDodgeButton.unlocked)
            dodgeMirrageUnlocked = true;
    }

    public void CreateMirrageOnDodge()
    {
        if (dodgeMirrageUnlocked)
            SkillManager.instance.clone.CreatClone(player.transform, new Vector3(2 * player.facingDir,0));
    }
}
