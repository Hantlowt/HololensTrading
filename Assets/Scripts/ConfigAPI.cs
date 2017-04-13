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
		{"EPA:CAP", "Cap Gemini SA"},
		{"NASDAQ:AAPL", "Apple Inc"},
		{"EPA:GLE", "Société Générale"},
		{"NASDAQ:MSFT", "Microsoft Corp"},
		{"NASDAQ:GOOG", "Google"},
		{"NASDAQ:AMZN", "Amazone"},
		{"EPA:AM", "Dassault Aviation SA"},
		{"INDEXEURO:PX1", "CAC 40"},
		{"EPA:AIR", "Airbus Group"},
		{"EPA:HO", "Thales SA"},
		{"EPA:ORA", "Orange SA."}
	};
	public static Dictionary<string, double> PriceList = new Dictionary<string, double>()
	{
		{"EPA:CAP", 86.26},
		{"NASDAQ:AAPL", 141.80},
		{"EPA:GLE", 43.31},
		{"NASDAQ:MSFT", 65.23},
		{"NASDAQ:GOOG", 830.46},
		{"NASDAQ:AMZN", 851.20},
		{"EPA:AM", 1127.90},
		{"INDEXEURO:PX1", 5070.79},
		{"EPA:AIR", 71.64},
		{"EPA:HO", 89.01},
		{"EPA:ORA", 14.22}
	};
	public static Dictionary<string, double> PercentList = new Dictionary<string, double>()
	{
		{"EPA:CAP", -1.28},
		{"NASDAQ:AAPL", 0.00},
		{"EPA:GLE", -3.01},
		{"NASDAQ:MSFT", -1.11},
		{"NASDAQ:GOOG", -2.11},
		{"NASDAQ:AMZN", 0.21},
		{"EPA:AM", 0.94},
		{"INDEXEURO:PX1", -0.59},
		{"EPA:AIR", -0.40},
		{"EPA:HO", 0.0},
		{"EPA:ORA", -1.01}
	};
	//public static string[] TickersList = { "AAPL", "GLE", "MSFT", "GOOG", "AMZN", "YHOO", "INDEXEURO:PX1" };
	//public static string[] CompanyNameList = { "Apple Inc", "Société Générale", "Microsoft Corp", "Google", "Amazone", "Yahoo", "CAC 40" };
	//public static string paramDateStart = "&startdate=";
	//public static string paramDateEnd = "&enddate=";
	//public static string paramFormatJson = "&format=json";

}