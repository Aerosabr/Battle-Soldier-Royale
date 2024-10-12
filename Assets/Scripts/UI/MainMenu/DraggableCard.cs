using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
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
	private Vector3 startingPosition = Vector3.zero;
	private bool isDragging = false;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (EquipSlot || !transform.GetComponent<CardSlotVisual>().CheckVisualEquipCard())
		{
            SoundManager.Instance.CardPickedUp();
			currentTransform = transform.GetComponent<RectTransform>();
			positionOnList = new Vector2(currentTransform.anchoredPosition.x, currentTransform.anchoredPosition.y);
			transform.SetParent(parentDuringDrag);
			isDragging = true;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (EquipSlot || !transform.GetComponent<CardSlotVisual>().CheckVisualEquipCard())
		{
			transform.position = Input.mousePosition;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (EquipSlot || !transform.GetComponent<CardSlotVisual>().CheckVisualEquipCard())
		{
			transform.SetParent(parentBeforeDrag);
			transform.SetSiblingIndex(childIndex);
			transform.GetComponent<RectTransform>().anchoredPosition = positionOnList;

			if (IsDroppedOverSpecificUI())
			{
				if (!EquipSlot)
				{
					if (EquippedLoadoutManager.Instance.AddCard(transform.GetComponent<CardSlotVisual>().cardSO))
						transform.GetComponent<CardSlotVisual>().EnableVisualEquipCard();
				}
				else
				{
					EquippedLoadoutManager.Instance.RemoveCard(transform.GetComponent<EquippedCardSlotVisual>().cardSO);
				}
			}
            SoundManager.Instance.CardEquipped();
            isDragging = false;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!isDragging)
		{
            SoundManager.Instance.CardPickedUp();
            OpenCardViewer();
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

	private void OpenCardViewer()
	{
		if (!EquipSlot)
			CardInformationBox.Instance.CardOpenViewer(transform.GetComponent<CardSlotVisual>().cardSO, transform.GetComponent<CardSlotVisual>().cardSO.level);
	}

}
