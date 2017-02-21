using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class getPrices : MonoBehaviour {

	public static getPrices singleton;

	void Start ()
	{
		if (singleton != null)
		{
			Destroy(singleton);
		}
		singleton = this as getPrices;
	}

	//Cette requête permets de récupérer les datas des 30 derniers jours concernant une société ou une marchandise précisé en paramètre
	public IEnumerator getPricesDays (string ticker)
	{
		UnityWebRequest www = UnityWebRequest.Get(ConfigAPI.apiGoogleBasePath + ConfigAPI.getPrice + ConfigAPI.paramCompany + ticker);
		yield return www.Send();
		print(www.url);
		print(www.downloadHandler.text);

		//attente test verif si donnée en json avant de parser :
		//companyList = JsonUtility.FromJson<CompanyListM>(www.downloadHandler.text);
	}

}
