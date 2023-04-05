using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveSlotsMenu : Menu
{
    [Header("Menu Nav")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSetSlotClicked(SaveSlot slot)
    {
        DisableMenuButtons();

        DataPersistanceManager.instance.ChangeSelectedProfileId(slot.GetProfileId());

        if (!isLoadingGame)
        {
            DataPersistanceManager.instance.NewGame();
        }

        DataPersistanceManager.instance.SaveGame();

        SceneManager.LoadSceneAsync(1);
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DataPersistanceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
        ActivateMenu(isLoadingGame);
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame) {

        this.gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.instance.GetAllProfileGameData();

        GameObject firstSelected = backButton.gameObject;
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
