using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPixel : MonoBehaviour {
	public static Texture2D WhiteBoardTexture;
	public static Color[] WhiteBoardTabColors;
	public static Color ColorToDraw;
	public static Color ColorToErase = Color.white;
	

	public Sprite[] Letters;

	private bool PencilMode;
	private bool RubberMode;
	private bool KeyboardMode;
	private bool OnDraw;
	private bool OnTape;
	private Color ColorToDrawPrevious;
	private int SizePencil;
	private int CursorCoordStartXOnTape;
	private Vector2 PreviousPoint;
	private Vector2 CursorCoord;
	private Vector2 VectorNull = new Vector2(0,0);
    private GameObject Pencil;

	// Use this for initialization
	private void Start () {
		PencilMode = false;
		RubberMode = false;
		KeyboardMode = false;
		OnTape = false;
		OnDraw = false;
		ColorToDraw = Color.black;
		SizePencil = 2;
        Pencil = transform.FindChild("pencil").gameObject;
		//initialisation temporaire du curseur, test input keyboard in hololens (le curseur de la souris n'est pas automatiquement pris en compte)
		CursorCoord = new Vector2(25, 200);
		WhiteBoardTexture = GetComponent<Renderer>().material.mainTexture as Texture2D;
		WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
		if (WhiteBoardTexture == null)
			throw new System.Exception("no texture for the Whiteboard!");
		else
			CleanWhiteBoard();
	}

    private Vector2 return_PosPencil()
    {
        RaycastHit hit;
        if (Physics.Raycast(Pencil.transform.position, Vector3.forward, out hit, 100))
            return (new Vector2((int)(hit.textureCoord.x * WhiteBoardTexture.width), (int)(hit.textureCoord.y * WhiteBoardTexture.height)));
        else
            return (VectorNull);
    }

	// Update is called once per frame
	private void Update () {
        Vector3 newPosPencil = new Vector3(Mathf.Clamp(Pencil.transform.localPosition.x + Input.GetAxis("Mouse X") * -1.0f,
            -5.0f, 5.0f), 0.03f, Mathf.Clamp(Pencil.transform.localPosition.z + Input.GetAxis("Mouse Y") * -1.0f, -5.0f, 5.0f));
        //Debug.Log(Mathf.Clamp(Pencil.transform.localPosition.y + Input.GetAxis("Mouse X") * -1.0f, -5.0f, 5.0f));
        Pencil.transform.localPosition = newPosPencil;
        if (OnDraw)//Si on a cliqué sur le whiteboard et que le mode PENCIl ou RUBBER sont activés, on trace un trait avec la fontion de bresenham, tant que le clic n'est pas relaché
		{
            //Vector2 NewPoint = SearchImpact();

            Vector2 NewPoint = return_PosPencil();
            if (NewPoint != VectorNull && PreviousPoint != VectorNull)
				BresenhamLike.DrawLineWithSize(SizePencil, NewPoint, PreviousPoint, WhiteBoardTexture.width, WhiteBoardTabColors, ColorToDraw);
            WhiteBoardTexture.SetPixels(WhiteBoardTabColors);
            WhiteBoardTexture.Apply();
            PreviousPoint = NewPoint;
		}
		else if (OnTape) //Si on a cliqué sur le whiteboard et que le mode clavier est activé on réalise l'action correspondante à la touche
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				StopCoroutine("ActiveCursor");
				OnTape = false;
			}
			else if (Input.GetKeyUp(KeyCode.Space))
			{
				DrawLetter(ConfigKeyboardDraw.LetterCursorBlank);
				CursorCoord.x += (int)Letters[ConfigKeyboardDraw.LetterCursorBlank].rect.width;
			}
			else if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
			{
				StopCoroutine("ActiveCursor");
				DrawLetter(ConfigKeyboardDraw.LetterCursorBlank);
				CursorCoord.x = CursorCoordStartXOnTape;
				CursorCoord.y -= (int)Letters[ConfigKeyboardDraw.LetterCursorBlank].rect.height;
				StartCoroutine("ActiveCursor");
			}
			else if (Input.GetKeyUp(KeyCode.Backspace))
			{
				StopCoroutine("ActiveCursor");
				CursorCoord.x -= (int)Letters[ConfigKeyboardDraw.LetterCursorBlank].rect.width;
				DrawLetter(ConfigKeyboardDraw.LetterCursorBlank);
				StartCoroutine("ActiveCursor");
			}
			else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1))
			{
				bool checkLetters = false;
				foreach (KeyValuePair<KeyCode, int> value in ConfigKeyboardDraw.LetterIndex)
				{
					if (Input.GetKeyDown(value.Key))
					{
						DrawLetter(value.Value);
						CursorCoord.x += (int)Letters[ConfigKeyboardDraw.LetterCursorBlank].rect.width;
						checkLetters = true;
						break;
					}
				}
				if (!checkLetters)
					print("La touche : '" + Input.inputString + "' n'est pas prise en charge.");
			}
		}
	}

	//Récupère la texture de la lettre demandée et la place sur le whiteboard à l'emplacement du curseur
	private void DrawLetter (int index)
	{
		Texture2D letters_tex = Letters[index].texture;
		Color[] letter = letters_tex.GetPixels((int)Letters[index].rect.x, (int)Letters[index].rect.y, (int)Letters[index].rect.width, (int)Letters[index].rect.height);
		WhiteBoardTexture.SetPixels((int)CursorCoord.x, (int)CursorCoord.y, (int)Letters[index].rect.width, (int)Letters[index].rect.height, letter);
		WhiteBoardTexture.Apply();
	}

	//Lance un rayon pour trouver les coordonnées du curseur sur la texture
	private Vector2 SearchImpact()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100))
		{
			int x = (int)(hit.textureCoord.x * WhiteBoardTexture.width);
			int y = (int)(hit.textureCoord.y * WhiteBoardTexture.height);
			return (new Vector2(x, y));
		}
		return (VectorNull);
	}

	// Pour afficher un curseur qui clignote et indiquer l'activation de la frappe à l'utilisateur :)
	private IEnumerator ActiveCursor ()
	{
		while (true)
		{
			DrawLetter(ConfigKeyboardDraw.LetterCursorBlank);
			yield return new WaitForSeconds(0.3f);
			DrawLetter(ConfigKeyboardDraw.LetterCursorIndex);
			yield return new WaitForSeconds(0.3f);
			DrawLetter(ConfigKeyboardDraw.LetterCursorBlank);
			yield return null;
		}
	}

	//activer le mode frappe du clavier et désactiver le stylo / gomme
	public void ActiveKeyboardMode ()
	{
		if (!KeyboardMode)
		{
			WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
			KeyboardMode = true;
			OnDraw = false;
			PencilMode = false;
			RubberMode = false;
		}
		else
		{
			StopCoroutine("ActiveCursor");
			KeyboardMode = false;
			OnTape = false;
		}
	}

	//active le mode dessin au stylo et désactive la frappe clavier / gomme
	public void ActivePencilMode ()
	{
		if (!PencilMode)
		{
			WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
			PencilMode = true;
			ColorToDraw = ColorToDrawPrevious;
			SizePencil = 2;
			KeyboardMode = false;
			OnTape = false;
			RubberMode = false;
		}
		else
			PencilMode = false;
	}

	//active le mode dessin à la gomme et désactive la frappe clavier / dessin stylo
	public void ActiveRubberMode ()
	{
		if (!RubberMode)
		{
			WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
			RubberMode = true;
			ColorToDrawPrevious = ColorToDraw;
			ColorToDraw = ColorToErase;
			SizePencil = 10;
			OnTape = false;
			KeyboardMode = false;
			PencilMode = false;
		}
		else
			RubberMode = false;
	}

	//nettoie tout le whiteboard = mode éponge
	public void CleanWhiteBoard ()
	{
		for (int i = 0; i < WhiteBoardTabColors.Length; i++)
		{
			WhiteBoardTabColors[i] = ColorToErase;
		}
		WhiteBoardTexture.SetPixels(WhiteBoardTabColors);
		WhiteBoardTexture.Apply();
	}

	// Au clic de la souris, récupére la position du cursor sur la texture et configure les paramètre pour les différents modes
	public void OnMouseDown ()
	{
		if (PencilMode || RubberMode)
		{
            //PreviousPoint = SearchImpact();
            PreviousPoint = return_PosPencil();
			OnDraw = true;
		}
		else if (KeyboardMode)
		{
			StopCoroutine("ActiveCursor");
			DrawLetter(ConfigKeyboardDraw.LetterCursorBlank);
			CursorCoord = return_PosPencil();
			CursorCoordStartXOnTape = (int)CursorCoord.x;
			StartCoroutine("ActiveCursor");
			OnTape = true;
		}
	}

	public void OnMouseUp ()
	{
		if (PencilMode || RubberMode)
		{
			OnDraw = false;
		}
	}
}
