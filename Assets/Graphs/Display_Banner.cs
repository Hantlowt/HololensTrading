using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display_Banner : MonoBehaviour {
    public string text;
    public int nbr_characters_to_show;
    public int pos_in_text;
    public float time_to_update;
    private float time;
    private TextMesh text_mesh;
	// Use this for initialization
	void Start () {
        pos_in_text = 0;
        text_mesh = GetComponent<TextMesh>();
	}

	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= time_to_update)
        {
            text_mesh.text = "";
            for (int i = 0; i < nbr_characters_to_show; i++)
            {
                text_mesh.text += text[(pos_in_text + i) % text.Length];
            }
            //text_mesh.text = text.Substring(pos_in_text, nbr_characters_to_show);
            if (pos_in_text >= text.Length)
                pos_in_text = 0;
            else
                pos_in_text++;
            time = 0.0f;
        }

     }
}
