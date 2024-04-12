using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer; 

    private bool canGrow = true;
    private bool cloneAttackReleased;
    private bool canShrink;
    private bool canCreatedHotkey = true;
    private bool playerCanDisappear = true;

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAtackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>();

    public bool playerCanExitState {  get; private set; }


    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisappear = false;
    }


    private void Update()
    {
        cloneAtackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;

        DestoryHotkey();
        cloneAttackReleased = true;
        canCreatedHotkey = false;

        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.fx.MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAtackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAtackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreatClone(targets[randomIndex], new Vector3(xOffset, 0));
            }

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", 1f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestoryHotkey();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestoryHotkey()
    {
        if (createdHotkey.Count <= 0)
        {
            return;
        }

        for(int i = 0;i< createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {

            collision.GetComponent<Enemy>().FreezeTime(true);
            CreatHotkey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false); */

    private void CreatHotkey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough hot keys in a key code list!");
            return;
        }

        if (!canCreatedHotkey)
            return;


        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        createdHotkey.Add(newHotkey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackHole_HotKey_Controller newHotkeyScript = newHotkey.GetComponent<BlackHole_HotKey_Controller>();

        newHotkeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
