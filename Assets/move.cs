﻿using UnityEngine;
using UnityEngine.EventSystems;

public class move : EventTrigger
{
    HandsTrackingManager input;
    Selector s;

    public void Start()
    {
        Debug.Log("Start button");
        input = GameObject.Find("InputManager").GetComponent<HandsTrackingManager>();
        s = GameObject.Find("Cursor").GetComponent<Selector>();
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        transform.parent.parent.position += input.velocity;
    }

    public override void OnPointerDown(PointerEventData data)
    {
        s.enable_disable(this.transform.parent.parent.gameObject);
        Debug.Log("OnPointerDown called.");
    }

    public override void OnPointerUp(PointerEventData data)
    {
        s.enable_disable(null);
        Debug.Log("OnPointerUp called.");
    }

}