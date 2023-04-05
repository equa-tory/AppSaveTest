using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    [Header("Menu navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        DisableButtonDependingOnData();
    }

    private void DisableButtonDependingOnData()
    {
        if (!DataPersistanceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        DataPersistanceManager.instance.SaveGame();
        //DataPersistanceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync(1);
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonDependingOnData();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
