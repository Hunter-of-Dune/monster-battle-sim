using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapSelectItem : MonoBehaviour
{
	[SerializeField] Button _button;
	[SerializeField] Image _thumbnailImage;
	[SerializeField] private MapPoolItem _mapPoolItem;
	[SerializeField] UIMapSelectController _controller;

	public void FillItem(UIMapSelectController controller, MapPoolItem item)
	{
		_mapPoolItem = item;
		_controller = controller;
	}

	private void Start()
	{
		_thumbnailImage.sprite = _mapPoolItem.Thumbnail;
		_button.onClick.AddListener(Click);
	}

	public void Click()
	{
		_controller.ChangeMap(_mapPoolItem.LocationSO);
	}
}
