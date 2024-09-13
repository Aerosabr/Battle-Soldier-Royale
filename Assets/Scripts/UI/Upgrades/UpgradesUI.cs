using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject upgradesTab;
    [SerializeField] private Transform container;
    [SerializeField] GridLayoutGroup gridLayout;

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

        InitializeGridsize();

        LoadLoadout();

        closeButton.gameObject.SetActive(false);
        upgradesTab.SetActive(false);
    }

    private void InitializeGridsize()
    {
        Vector2 gridSize = gridLayout.gameObject.GetComponent<RectTransform>().rect.size;
        gridLayout.cellSize = new Vector2(gridSize.x / 4, gridSize.y / 2);
    }

    private void LoadLoadout()
    {
        foreach (LoadoutCharacter loadout in PlayerBlue.Instance.GetLoadout())
        {
            Transform charTransform = Instantiate(loadout.characterPathSO.upgradesTab, container).transform;
            charTransform.GetComponent<UpgradesSingleUI>().SetCPSO(loadout);
        }
    }

    private void OpenUpgradesTab()
    {
        upgradesTab.SetActive(true);
        closeButton.gameObject.SetActive(true);
        upgradesButton.gameObject.SetActive(false);
    }

    private void CloseUpgradesTab()
    {
        upgradesTab.SetActive(false);
        closeButton.gameObject.SetActive(false);
        upgradesButton.gameObject.SetActive(true);  
    }
}
