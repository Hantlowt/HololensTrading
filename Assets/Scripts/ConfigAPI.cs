using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Memo doc API GOOGLE FINANCE :
 *		-  : valeur à l'ouverture, à la fermeture, la plus basse et la plus haute dans la journée et volume total des transactions pour ce jour.
 *		- finance/info?
 *		- finance/historical?
 *		- finance/getprices?
 *		- Sociétés: AAPL-Apple Inc, GLE-Société Générale, MSFT-Microsoft Corporation, GOOG-google, AMZN-Amazone, YHOO-Yahoo, INDEXEURO:PX1-CAC 40.
 *		- Définition des champs de la réponse JSON :  
 *			t	Ticker
 *			e	Exchange
 *			l	Last Price
 *			ltt	Last Trade Time
 *			l	Price
 *			lt	Last Trade Time Formatted
 *			lt_dts	Last Trade Date/Time
 *			c	Change
 *			cp	Change Percentage
 *			el	After Hours Last Price
 *			elt	After Hours Last Trade Time Formatted
 *			div	Dividend
 *			yld	Dividend Yield
 */

public class ConfigAPI : MonoBehaviour {

	public static string apiGoogleBasePath = "http://finance.google.com/finance/";
	public static string getPrice = "getprices?";
	public static string getLastPrice = "info?";
	public static string getFinance = "finance?";
	public static string paramCompany = "q=";
	public static Dictionary<string, string> CompanyList = new Dictionary<string, string>()
	{
		{"TADAWUL:1120", "Al Rajhi Banking & Invstmnt" },
		{"NASDAQ:AAPL", "Apple Inc" },
		{"GLE:EPA", "Société Générale" },
		{"NASDAQ:MSFT", "Microsoft Corp" },
		{"NASDAQ:GOOG", "Google" },
		{"NASDAQ:AMZN", "Amazone" },
		{"EPA:AM", "Dassault Aviation SA" },
		{"INDEXEURO:PX1", "CAC 40" },
		{"EPA:AIR", "Airbus Group" },
		{"EPA:HO", "Thales SA" },
		{"TADAWUL:2010", "Saudi Basic Ind." },
	};
	//public static string[] TickersList = { "AAPL", "GLE", "MSFT", "GOOG", "AMZN", "YHOO", "INDEXEURO:PX1" };
	//public static string[] CompanyNameList = { "Apple Inc", "Société Générale", "Microsoft Corp", "Google", "Amazone", "Yahoo", "CAC 40" };
	//public static string paramDateStart = "&startdate=";
	//public static string paramDateEnd = "&enddate=";
	//public static string paramFormatJson = "&format=json";

}