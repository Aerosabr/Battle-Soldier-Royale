using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject upgradesTab;

    [SerializeField] private Transform tabContainer;
    [SerializeField] private GameObject tab;

    [SerializeField] private Image infoImage;
    [SerializeField] private CharacterInfoUI characterInfoUI;

    private int currentTab = 0;
    private List<Transform> tabs = new List<Transform>();

    private void Start()
    {
        upgradesButton.onClick.AddListener(() =>
        {
            OpenUpgradesTab();
        });

        closeButton.onClick.AddListener(() =>
        {
            CloseUpgradesTab();
        });

        LoadLoadout();

        closeButton.gameObject.SetActive(false);
        upgradesTab.SetActive(false);
    }

    private void LoadLoadout()
    {
        int index = 0;
        foreach (CardSO loadout in PlayerBlue.Instance.GetLoadout())
        {
            Transform tabTransform = Instantiate(tab, tabContainer).transform;
            tabs.Add(tabTransform);
            tabTransform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = loadout.backgroundVertical[loadout.level - 1];
            tabTransform.gameObject.SetActive(true);
            int temp = index;
            tabTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
            {
                if (currentTab == temp)
                    return;

                UnloadCurrentCharacterTab();
                LoadCharacterTab(temp);
            });
            index++;
        }
    }

    private void OpenUpgradesTab()
    {
        upgradesTab.SetActive(true);
        closeButton.gameObject.SetActive(true);
        upgradesButton.gameObject.SetActive(false);
        LoadCharacterTab(currentTab);
    }

    private void CloseUpgradesTab()
    {
        upgradesTab.SetActive(false);
        closeButton.gameObject.SetActive(false);
        upgradesButton.gameObject.SetActive(true);  
    }

    private void LoadCharacterTab(int index)
    {
        tabs[index].GetChild(0).position = new Vector3(tabs[index].position.x, tabs[index].position.y + 40, tabs[index].position.z);
        currentTab = index;

        CardSO card = PlayerBlue.Instance.GetLoadout()[currentTab];
        infoImage.sprite = card.backgroundVertical[card.level - 1];
        characterInfoUI.showingCurrent = true;
        characterInfoUI.LoadCardStats(card);
    }

    private void UnloadCurrentCharacterTab()
    {
        tabs[currentTab].GetChild(0).position = tabs[currentTab].position;
    }
}
