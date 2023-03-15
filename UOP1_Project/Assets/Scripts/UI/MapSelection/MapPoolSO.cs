using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MapPoolItem
{
	public LocationSO LocationSO;
	public Sprite Thumbnail;
}


[CreateAssetMenu(fileName = "NewMapPool", menuName = "Maps/New Map Pool")]
public class MapPoolSO : ScriptableObject
{
	[SerializeField] private List<MapPoolItem> _maps = new List<MapPoolItem>();

	public List<MapPoolItem> Maps => _maps;

	//save pool

	//load pool
}
