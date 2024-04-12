using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [Header("End screen")]
    [SerializeField] private GameObject endText;
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject exitButton;

    [Space]

    [SerializeField] public GameObject characterUI;
    [SerializeField] public GameObject skillTreeUI;
    [SerializeField] public GameObject craftUI;
    [SerializeField] public GameObject optionsUI;
    [SerializeField] public GameObject inGameUI;

    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;


    private void Awake()
    {
        SwitchTo(skillTreeUI);//we need this to assign events on skill tree slots before we assign events on skill scripts
        fadeScreen.gameObject.SetActive(true);
    }

    void Start()
    {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionsUI);
    }

    public void SwitchTo(GameObject _menu)
    { 

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
            _menu.SetActive(true);

        if(GameManager.instance != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }


    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    public void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutine());
    }

    IEnumerator EndScreenCorutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
