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
		{"TADAWUL:1120", "Al Rajhi Banking"},
		{"NASDAQ:AAPL", "Apple Inc"},
		{"GLE:EPA", "Société Générale"},
		{"NASDAQ:MSFT", "Microsoft Corp"},
		{"NASDAQ:GOOG", "Google"},
		{"NASDAQ:AMZN", "Amazone"},
		{"EPA:AM", "Dassault Aviation SA"},
		{"INDEXEURO:PX1", "CAC 40"},
		{"EPA:AIR", "Airbus Group"},
		{"EPA:HO", "Thales SA"},
		{"TADAWUL:2010", "Saudi Basic Ind."}
	};
	public static Dictionary<string, double> PriceList = new Dictionary<string, double>()
	{
		{"TADAWUL:1120", 63.63},
		{"NASDAQ:AAPL", 146.84},
		{"GLE:EPA", 46.38},
		{"NASDAQ:MSFT", 64.21},
		{"NASDAQ:GOOG", 830.46},
		{"NASDAQ:AMZN", 851.20},
		{"EPA:AM", 1127.90},
		{"INDEXEURO:PX1", 4961.47},
		{"EPA:AIR", 68.85},
		{"EPA:HO", 89.01},
		{"TADAWUL:2010", 95.50}
	};
	public static Dictionary<string, double> PercentList = new Dictionary<string, double>()
	{
		{"TADAWUL:1120", 0.40},
		{"NASDAQ:AAPL", 1.05},
		{"GLE:EPA", -2.38},
		{"NASDAQ:MSFT", -1.11},
		{"NASDAQ:GOOG", -2.11},
		{"NASDAQ:AMZN", 0.21},
		{"EPA:AM", 0.94},
		{"INDEXEURO:PX1", -0.82},
		{"EPA:AIR", -1.09},
		{"EPA:HO", 0.0},
		{"TADAWUL:2010", -1.04}
	};
	//public static string[] TickersList = { "AAPL", "GLE", "MSFT", "GOOG", "AMZN", "YHOO", "INDEXEURO:PX1" };
	//public static string[] CompanyNameList = { "Apple Inc", "Société Générale", "Microsoft Corp", "Google", "Amazone", "Yahoo", "CAC 40" };
	//public static string paramDateStart = "&startdate=";
	//public static string paramDateEnd = "&enddate=";
	//public static string paramFormatJson = "&format=json";

}