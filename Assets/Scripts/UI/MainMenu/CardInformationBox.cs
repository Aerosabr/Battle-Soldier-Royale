using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInformationBox : MonoBehaviour
{
    public static CardInformationBox Instance;

	private List<GameObject> statBox = new List<GameObject>();
	[SerializeField] private GameObject CardViewer;
	[SerializeField] private GameObject statBoxTemplate;
	[SerializeField] private Transform statBoxParent;
	[SerializeField] private Image icon;
	[SerializeField] private Button level1;
	[SerializeField] private Button level2;
	[SerializeField] private Button level3;
	private CardSO cardSO;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

	public void ViewCard(CardSO cardSO, int level)
	{
		CardViewer.SetActive(true);
		DestroyAllStatBox();
		statBoxTemplate.SetActive(true);

		this.cardSO = cardSO;
		icon.sprite = cardSO.backgroundVertical[level - 1];
		List<string> list = cardSO.ViewCard(level);
		foreach (var stat in list)
		{
			var box = Instantiate(statBoxTemplate);
			box.transform.GetChild(0).GetComponent<Text>().text = stat.ToString();
			box.transform.SetParent(statBoxParent);
			statBox.Add(box);
		}
		statBoxTemplate.SetActive(false);
	}

	public void ChangeLevel(int level)
	{
		ViewCard(cardSO, level);
	}

	public void CardOpenViewer(CardSO cardSO, int level)
	{
		level1.interactable = false;
		level2.interactable = true;
		level3.interactable = true;
		level2.transform.GetChild(1).GetComponent<Text>().text = "Cost: " + cardSO.upgradeCost[0].ToString() + " (Level 2)";
		level3.transform.GetChild(1).GetComponent<Text>().text = "Cost: " + cardSO.upgradeCost[1].ToString() + " (Level 3)";
		ViewCard(cardSO, level);
	}

	private void DestroyAllStatBox()
	{
		foreach(var box in statBox)
		{
			Destroy(box);
		}
		statBox.Clear();
	}
}
