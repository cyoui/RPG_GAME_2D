using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{

    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked {  get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirrage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirrageUnlockButton;
    public bool parryWithMirrageUnlocked { get; private set; }


    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirrageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirrage);
    }


    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirrage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }


    private void UnlockParryWithMirrage()
    {
        if (parryWithMirrageUnlockButton.unlocked)
            parryWithMirrageUnlocked = true;
    }


    public void MakeMirrageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirrageUnlocked)
            SkillManager.instance.clone.CreatCloneWithDelay(_respawnTransform);
    }
}
