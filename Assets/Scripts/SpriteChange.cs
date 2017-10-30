using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpriteChange : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	[SerializeField] private Sprite clicked;
	[SerializeField] private Sprite released;

	private Image img;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
	}

	public virtual void OnPointerDown(PointerEventData ped){

		img.sprite = clicked;
	}

	public virtual void OnPointerUp(PointerEventData ped){

		img.sprite = released;
	}
}
