using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOrHide : MonoBehaviour {

    private SimpleTagalong Tagalong;
    public bool show = true;
	// Use this for initialization
	void Start () {
        Tagalong = GetComponent<SimpleTagalong>();
        if (show)
            iTween.MoveFrom(gameObject, iTween.Hash('y', 5f));
        else
            Hide();
    }
	
	// Update is called once per frame
	void Update () {
	   	
	}

    public void Show()
    {
        Tagalong.enabled = true;
        iTween.MoveTo(gameObject, iTween.Hash('y', 0f));
        show = true;
    }

    public void Hide()
    {
        Tagalong.enabled = false;
        iTween.MoveTo(gameObject, iTween.Hash('y', 5f));
        show = false;
    }

    public void Switch()
    {
        if (show)
            Hide();
        else
            Show();
    }


}
