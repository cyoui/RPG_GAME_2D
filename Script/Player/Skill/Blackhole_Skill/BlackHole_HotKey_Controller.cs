using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackHole_Skill_Controller blackHole;

    public void SetupHotKey(KeyCode _myHotkey, Transform _myEnemy, BlackHole_Skill_Controller _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy.transform;
        blackHole = _myBlackHole;

        myHotkey = _myHotkey;
        myText.text = _myHotkey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            blackHole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
