using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing.SyncModel;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace HoloToolkit.Sharing.Spawning
{
	[SyncDataClass]
    public class SyncWhiteboard : SyncSpawnedObject //Pour le partage des donnees sur le network
    {
        [SyncData]
        public SyncString data;

    }
}

public class DrawPixel : MonoBehaviour {
	public static Color ColorToDraw;
	public static Color ColorToErase = Color.white;

	public Sprite[] Letters;
	public GameObject PencilSprite;
	public Sprite[] CursorSprites;

	private Texture2D WhiteBoardTexture;
	private Color[] WhiteBoardTabColors;

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
	private Texture2D CursorTexture;
    public SyncWhiteboard sync;
    public byte[] greydata;
    public string actual_data;
	public PrefabSpawnManager SpawnManager;

	// Use this for initialization
	private void Start () {
		PencilMode = false;
		RubberMode = false;
		KeyboardMode = false;
		OnTape = false;
		OnDraw = false;
		ColorToDraw = Color.black;
		ColorToDrawPrevious = ColorToDraw;
		SizePencil = 2;
        Pencil = transform.FindChild("pencil").gameObject;
		PencilSprite.GetComponent<SpriteRenderer>().sprite = CursorSprites[0];
		WhiteBoardTexture = GetComponent<Renderer>().material.mainTexture as Texture2D;
		WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
		SpawnManager = GameObject.Find("Sharing").GetComponent<PrefabSpawnManager>();
		sync = transform.parent.GetComponent<DefaultSyncModelAccessor>().SyncModel as SyncWhiteboard;
        greydata = new byte[WhiteBoardTabColors.Length / 8];
		if (WhiteBoardTexture == null)
			throw new System.Exception("no texture for the Whiteboard!");
		else
			CleanWhiteBoard();
		StartCoroutine("SharingWhiteboard");
	}

	public void Destroy_on_Network ()
	{
		SpawnManager.Delete(sync);
		Destroy(this.transform.parent);
	}

	public void send_data()
    {
        int o = 0;
		for (int i = 0; i < WhiteBoardTabColors.Length; i += 8)
        {
            greydata[o] = (byte)0;
            for (int j = 0; j < 8; j++)
                greydata[o] |= (byte)((WhiteBoardTabColors[i + j] == Color.white ? (byte)0 : (byte)1) << j);
            o++;
        }
        sync.data.Value = System.Convert.ToBase64String(greydata);
        actual_data = sync.data.Value;
    }

 

    private IEnumerator SharingWhiteboard()
    {
        while (true)
        {
            if (!OnDraw && !OnTape)
            {
                if (sync.data.Value != "")
				{
					WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
					//send_data();
					byte[] a = System.Convert.FromBase64String(sync.data.Value);
                    int o = 0;
                    for (int i = 0; i < WhiteBoardTabColors.Length; i += 8)
                    {

                        for (int j = 0; j < 8; j++)
                        {
                            WhiteBoardTabColors[i + j] = (byte)((a[o] >> j) & 0x1) == 0x0 ? Color.white : Color.black;
                            if ((i + j) % 3000 == 0)
                                yield return null;
                        }
                        o++;
                    }
                    WhiteBoardTexture.SetPixels(WhiteBoardTabColors);
                    WhiteBoardTexture.Apply();
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
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
		Vector3 newPosPencil = new Vector3(Mathf.Clamp(Pencil.transform.localPosition.x + Input.GetAxis("Mouse X") * -0.1f,
            -0.5f, 0.5f), Mathf.Clamp(Pencil.transform.localPosition.y + Input.GetAxis("Mouse Y") * -0.1f, -0.5f, 0.5f), -0.8f);
        Pencil.transform.localPosition = newPosPencil;

		if (OnDraw)//Si on a cliqué sur le whiteboard et que le mode PENCIl ou RUBBER sont activés, on trace un trait avec la fontion de bresenham, tant que le clic n'est pas relaché
		{
			WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
			Vector2 NewPoint = return_PosPencil();
            if (NewPoint != VectorNull && PreviousPoint != VectorNull)
				BresenhamLike.DrawLineWithSize(SizePencil, NewPoint, PreviousPoint, WhiteBoardTexture.width, WhiteBoardTabColors, ColorToDraw);
			WhiteBoardTexture.SetPixels(WhiteBoardTabColors);
            WhiteBoardTexture.Apply();
            PreviousPoint = NewPoint;
		}
		else if (OnTape) //Si on a cliqué sur le whiteboard et que le mode clavier est activé on réalise l'action correspondante à la touche
		{
			WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
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
			KeyboardMode = true;
			WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
			PencilSprite.GetComponent<SpriteRenderer>().sprite = CursorSprites[1];
			OnDraw = false;
			PencilMode = false;
			RubberMode = false;
		}
		else
		{
			KeyboardMode = false;
			StopCoroutine("ActiveCursor");
			OnTape = false;
			PencilSprite.GetComponent<SpriteRenderer>().sprite = CursorSprites[0];
		}
	}

	//active le mode dessin au stylo et désactive la frappe clavier / gomme
	public void ActivePencilMode ()
	{
		if (!PencilMode)
		{
			StopCoroutine("ActiveCursor");
			PencilMode = true;
			WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
			PencilSprite.GetComponent<SpriteRenderer>().sprite = CursorSprites[2];
			ColorToDraw = ColorToDrawPrevious;
			PencilSprite.GetComponent<SpriteRenderer>().color = ColorToDraw;
			SizePencil = 3;
			KeyboardMode = false;
			OnTape = false;
			RubberMode = false;
            send_data();
		}
		else
		{
			PencilMode = false;
			PencilSprite.GetComponent<SpriteRenderer>().sprite = CursorSprites[0];
		}
	}

	//active le mode dessin à la gomme et désactive la frappe clavier / dessin stylo
	public void ActiveRubberMode ()
	{
		if (!RubberMode)
		{
			StopCoroutine("ActiveCursor");
			RubberMode = true;
			WhiteBoardTabColors = WhiteBoardTexture.GetPixels();
			PencilSprite.GetComponent<SpriteRenderer>().sprite = CursorSprites[3];
			if (ColorToDraw != ColorToErase)
			{
				ColorToDrawPrevious = ColorToDraw;
				ColorToDraw = ColorToErase;
			}
			SizePencil = 14;
			OnTape = false;
			KeyboardMode = false;
			PencilMode = false;
            send_data();
		}
		else
		{
			RubberMode = false;
			PencilSprite.GetComponent<SpriteRenderer>().sprite = CursorSprites[0];
		}
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
        send_data();
	}
	
	// Au clic de la souris, récupére la position du cursor sur la texture et configure les paramètre pour les différents modes
	public void OnMouseDown ()
	{
		if (PencilMode || RubberMode)
		{
            PreviousPoint = return_PosPencil();
			OnDraw = true;
		}
		else if (KeyboardMode)
		{
			StopCoroutine("ActiveCursor");
			DrawLetter(ConfigKeyboardDraw.LetterCursorBlank);
			CursorCoord = return_PosPencil();
			CursorCoord.y -= 1;
			CursorCoordStartXOnTape = (int)CursorCoord.x;
			StartCoroutine("ActiveCursor");
			OnTape = true;
		}
	}

	public void OnMouseUp ()
	{
        send_data();
        if (PencilMode || RubberMode)
		{
			OnDraw = false;
		}
	}

    public void SaveData()
    {
        using (FileStream fs = new FileStream(Application.persistentDataPath + "/boards.save", FileMode.Append, FileAccess.Write))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            send_data();
            sw.WriteLine(System.Convert.ToBase64String(greydata));
        }
    }

    public void LoadData(Text TextureLabel)
    {
        int id = System.Convert.ToInt32(TextureLabel.text.Replace("Board", ""));
        if (File.Exists(Application.persistentDataPath + "/boards.save"))
        {
            string[] all_file = File.ReadAllLines(Application.persistentDataPath + "/boards.save");
            sync.data.Value = all_file[id];
        }
    }
}
