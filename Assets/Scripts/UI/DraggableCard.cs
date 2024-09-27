using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private Transform parentDuringDrag;
	[SerializeField] private Transform parentBeforeDrag;
	[SerializeField] private GraphicRaycaster raycaster;
	[SerializeField] private RectTransform targetUIElement;
	[SerializeField] private bool EquipSlot;

	private PointerEventData pointerEventData;
	private RectTransform currentTransform;
	private int childIndex;
	private Vector2 positionOnList;


	public void OnBeginDrag(PointerEventData eventData)
	{
		currentTransform = transform.GetComponent<RectTransform>();
		positionOnList = new Vector2(currentTransform.anchoredPosition.x, currentTransform.anchoredPosition.y);
		transform.SetParent(parentDuringDrag);
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}


	public void OnEndDrag(PointerEventData eventData)
	{
		transform.SetParent(parentBeforeDrag);
		transform.SetSiblingIndex(childIndex);
		transform.GetComponent<RectTransform>().anchoredPosition = positionOnList;

		if(IsDroppedOverSpecificUI())
		{
			if (!EquipSlot)
				EquippedLoadoutManager.Instance.AddCard(transform.GetComponent<CardSlotVisual>().cardSO);
			else
				EquippedLoadoutManager.Instance.RemoveCard(transform.GetComponent<EquippedCardSlotVisual>().cardSO);

		}
	}

	private bool IsDroppedOverSpecificUI()
	{
		pointerEventData.position = Input.mousePosition;
		List<RaycastResult> results = new List<RaycastResult>();
		raycaster.Raycast(pointerEventData, results);
		foreach (RaycastResult result in results)
		{
			if (result.gameObject == targetUIElement.gameObject)
			{
				return true;
			}
		}
		return false;
	}

	public void Start()
	{
		childIndex = transform.GetSiblingIndex();
		pointerEventData = new PointerEventData(EventSystem.current);
	}

}
