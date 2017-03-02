using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpdateInfosBanner : MonoBehaviour {

	public string title = null;
	public string ticker = null;
	public GameObject titleText = null;
	public GameObject priceText = null;
	public GameObject percentText = null;
	public GameObject arrowGreen = null;
	public GameObject arrowRed = null;
	public GameObject arrowWhite = null;

	private double newPrice = 0.0f;
	private double newPercent = 0.0f;

	IEnumerator Start ()
	{
		yield return StartCoroutine("DataToImageLed", ticker);
	}

	IEnumerator DataToImageLed (string ticker)
	{
		while (true)
		{
			UnityWebRequest www = UnityWebRequest.Get(ConfigAPI.apiGoogleBasePath + ConfigAPI.getLastPrice + ConfigAPI.paramCompany + ticker);
			yield return www.Send();
			if (www.downloadHandler.text != "")
				yield return StartCoroutine("ParseRequestLastPrices", www.downloadHandler.text);
			yield return new WaitForSeconds(15.0f); // on met à jour les textes des bannieres toutes les 15 secondes
		}
	}

	IEnumerator ParseRequestLastPrices (string data)
	{
		string pattern = @"{[^}]+}";
		Match m = Regex.Match(data, pattern); // Regex pour corriger le format du json reçu
		SharePricesM sharePrice;
		sharePrice = JsonUtility.FromJson<SharePricesM>(m.Value); //enregistrement des données du json dans un objet SharePriceM
		yield return new WaitForSeconds(1.0f);
		newPrice = Math.Round(sharePrice.l, 2);
		newPercent = Math.Round(sharePrice.cp, 2);
		yield return StartCoroutine("SaveNewDataInLedImage");
	}

	IEnumerator SaveNewDataInLedImage ()
	{
		//récupérer ici les textes enfants pour enregistrer les données de la requête...
		titleText.GetComponent<Text>().text = title;
		priceText.GetComponent<Text>().text = newPrice.ToString();
		if (newPercent > 0) //le cours de l'action monte mettre en vert
		{
			percentText.GetComponent<Text>().text = "+" + newPercent.ToString();
			arrowGreen.SetActive(true);
			arrowRed.SetActive(false);
			arrowWhite.SetActive(false);
			percentText.GetComponent<Text>().color = Color.green;
		}
		else if (newPercent == 0)//le cours de l'action est identique mettre en blanc
		{
			percentText.GetComponent<Text>().text = newPercent.ToString();
			arrowGreen.SetActive(false);
			arrowRed.SetActive(false);
			arrowWhite.SetActive(true);
			percentText.GetComponent<Text>().color = Color.white;
		}
		else //le cours de l'action chûte mettre en rouge
		{
			percentText.GetComponent<Text>().text = newPercent.ToString();
			arrowGreen.SetActive(false);
			arrowRed.SetActive(true);
			arrowWhite.SetActive(false);
			percentText.GetComponent<Text>().color = Color.red;
		}
		yield return null;
	}
}
