using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{


    [SerializeField] private UI_SkillTreeSlot blackholeUnlockButton;
    public bool blackholeUnlocked;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    BlackHole_Skill_Controller currentBlackhole;



    private void UnlockBlackhole()
    {
        if (blackholeUnlockButton.unlocked)
            blackholeUnlocked = true;
    }


    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockBlackhole();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<BlackHole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed,shrinkSpeed,amountOfAttacks,cloneCooldown,blackholeDuration);

    }

    protected override void Start()
    {
        base.Start();

        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }


    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
