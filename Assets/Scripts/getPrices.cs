using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class getPrices : MonoBehaviour {
	
	public static getPrices singleton;
	public static string[][] dataPrices; //données requete API des 25 derniers jours

	void Start ()
	{
		if (singleton != null)
		{
			Destroy(singleton);
		}
		singleton = this as getPrices;
	}

	public IEnumerator getPricesDays (string ticker)
	{
		print("in getprice : " + ticker);

		UnityWebRequest www = UnityWebRequest.Get(ConfigAPI.apiGoogleBasePath + ConfigAPI.getPrice + ConfigAPI.paramCompany + ticker);
		yield return www.Send();
		print("URL" + www.url);
		print("resultat brut requete 25 derniers jours : " + www.downloadHandler.text);
		dataPrices = parseRequestAllPrices(www.downloadHandler.text);
	}

	/*
	 * récupère les données de la requete et les retournent sous forme de tableau, de tableau de string
	 * supprime les 8 premières lignes dont on n'a pas besoin pour ne conserver que les dates et prix
	 * newData[i] contient alors: DATE,CLOSE,HIGH,LOW,OPEN,VOLUME
	 */
   private string[][] parseRequestAllPrices (string data)
   {
	   string[] stringTmp = data.Split('\n');
	   string[][] newData = new string[stringTmp.Length - 8][];
	   int i = 0; //index pour remplir le nouveau tableau
	   int i2 = 0; //index pour supprimer les lignes inutiles
	   foreach (string substring in stringTmp)
	   {
		   if (i2 < 8)
			   i2++;
		   else
			   newData[i++] = substring.Split(',');
	   }
	   printDataRequestDebug(newData);
	   return (newData);
   }

   private void printDataRequestDebug (string[][] data)
   {
	   foreach (string[] val1 in data)
	   {
		   foreach (string val2 in val1)
		   {
			   print(val2);
		   }
	   }
   }
}
