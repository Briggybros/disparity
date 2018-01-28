﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour {

	private const string LEVEL_SELECT_NAME = "Level Select Panel";
	private const string ERROR_MESSAGE_NAME = "Error Message";

	public GameObject buttonPrefab;
	public string[] levels;

	private NetworkManager networkManager;
	private GameObject levelSelectPanel;
	private GameObject errorMessageObject;
	private IEnumerator errorHideCoroutine = null;

	private GameObject MakeButton (GameObject container, string text, Vector2 position, UnityAction clickListener = null) {
		GameObject button = Instantiate(buttonPrefab, new Vector2(0, 0), Quaternion.identity);
		button.GetComponentInChildren<Text>().text = text;
		button.GetComponent<RectTransform>().anchoredPosition = position;
		if (clickListener != null) {
			button.GetComponent<Button>().onClick.AddListener(clickListener);
		}
		button.transform.SetParent(container.transform, false);

		return button;
	}

	private IEnumerator HideError (float timeout) {
		yield return new WaitForSeconds(timeout);
		errorMessageObject.SetActive(false);
		errorHideCoroutine = null;
	}

	private void ShowError (string message) {
		if (errorHideCoroutine != null) {
			StopCoroutine(errorHideCoroutine);
		}
		errorMessageObject.GetComponent<Text>().text = message;
		errorMessageObject.SetActive(true);
		errorHideCoroutine = HideError(5);
		StartCoroutine(errorHideCoroutine);
	}

	void Start () {
		networkManager = NetworkManager.singleton;
		foreach (Transform child in transform) {
			switch (child.gameObject.name) {
				case LEVEL_SELECT_NAME:
					levelSelectPanel = child.gameObject;
					break;
				case ERROR_MESSAGE_NAME:
					errorMessageObject = child.gameObject;
					break;
			}
		}
	}

	public void StartButtonClicked () {
		networkManager.StartMatchMaker();
	}

	public void StopMatchMaker () {
		networkManager.StopMatchMaker();
	}

	public void LevelSelect (Text textObject) {
		string matchName = textObject.text != "" ? textObject.text : "default";
		foreach (string level in levels) {
			MakeButton(levelSelectPanel, level, new Vector2(0, 0), () => {
				networkManager.onlineScene = level;
				CreateInternetMatch(matchName);
			});
		}
	}

	public void CreateInternetMatch (string matchName) {
		networkManager.matchMaker.CreateMatch(matchName, 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
	}

	private void OnInternetMatchCreate (bool success, string extendedInfo, MatchInfo matchInfo) {
		if (success) {
			NetworkServer.Listen(matchInfo, 9000);

			networkManager.StartHost(matchInfo);
		} else {
			ShowError("Failed to connect to the match");
		}
	}

	public void FindInternetMatch (Text textObject) {
		string matchName = textObject.text != "" ? textObject.text : "default";
		networkManager.matchMaker.ListMatches(0 ,10, matchName, true, 0, 0, OnInternetMatchList);
	}

	private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> internetMatches) {
		if (success) {
			if (internetMatches.Count != 0) {
				networkManager.matchMaker.JoinMatch(internetMatches[0].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
			} else {
				ShowError("No matches found. Try a different name");
			}
		} else {
			ShowError("Unable to find matches");
		}
	}

	private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo) {
		if (success) {
			networkManager.StartClient(matchInfo);
		} else {
			ShowError("Failed to join the match");
		}
	}
}
