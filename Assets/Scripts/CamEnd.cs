using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CamEnd : MonoBehaviour {

	[SerializeField] private Animator anim;
	[SerializeField] private GameObject panel;

	[SerializeField] AudioClip firstClip;
	[SerializeField] AudioClip secondClip;
	[SerializeField] AudioClip thirdClip;
	[SerializeField] AudioClip fourthClip;
	[SerializeField] AudioClip roarClip;
	[SerializeField] AudioClip laughClip;

	private AudioSource audioSoure;
	private Text text;

	void Start(){
		text = panel.GetComponentInChildren<Text> ();
		audioSoure = GetComponent<AudioSource> ();
	}

	public void First() {
		panel.SetActive (true);
		text.text = "You are the last of your breed. Surrounded from all corners with no escape.";
		audioSoure.PlayOneShot (firstClip);
	}

	public void Second() {
		text.text = "Surrender. You have no chance. My armies are all around you. We will make this cemetary your burial ground.";
		audioSoure.PlayOneShot (secondClip);
	}

	public void Third() {
		text.text = "Your time is over. The time of the Orc has just began.";
		audioSoure.PlayOneShot (fourthClip);
	}

	public void Fourth() {
		text.text = "Are you ready to meet your fate?";
		audioSoure.PlayOneShot (thirdClip);
	}

	public void OnRoar() {
		text.text = "Prepare to die !!";
		anim.SetTrigger ("isRoaring");
		audioSoure.PlayOneShot (roarClip);
	}

	public void OnLaugh() {
		text.text = "";
		panel.SetActive (false);
		audioSoure.PlayOneShot (laughClip);
	}

	public void OnAnimationEnd() {
		AnimationManager.instance.DestroyCamera(gameObject);
	}
}
