using UnityEngine;
using UnityEngine.EventSystems;

public class Resize : EventTrigger
{
    HandsTrackingManager input;
    Selector s;
    GraphLine GL = null;
    GraphBar GB = null;

    public void Start()
    {
        Debug.Log("Start button");
        input = GameObject.Find("InputManager").GetComponent<HandsTrackingManager>();
        s = GameObject.Find("Cursor").GetComponent<Selector>();
        if (transform.parent.parent.FindChild("GraphLine").GetComponent<GraphLine>() != null)
            GL = transform.parent.parent.FindChild("GraphLine").GetComponent<GraphLine>();
        if (transform.parent.parent.FindChild("GraphBar") != null)
            GB = transform.parent.parent.FindChild("GraphBar").GetComponent<GraphBar>();
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        //transform.parent.parent.localScale += new Vector3(input.velocity.x / 50.0f, input.velocity.y / 50.0f, 1.0f);
        if (GL != null)
        {
            GL.height += input.velocity.y / 10.0f;
            GL.width += input.velocity.x / 10.0f;
            //GL.Restart();
        }
        if (GB != null)
        {
            GB.height += input.velocity.y / 10.0f;
            GB.width += input.velocity.x / 10.0f;
            //GL.Restart();
        }
    }

    public override void OnPointerDown(PointerEventData data)
    {
        if (GL != null)
            GL.online = false;
        if (GB != null)
            GB.online = false;
        //s.enable_disable(this.transform.parent.parent.gameObject);
        Debug.Log("OnPointerDown called.");
    }

    public override void OnPointerUp(PointerEventData data)
    {
        //s.enable_disable(null);
        
        if (GL != null)
        {
            GL.sync.Height.Value = GL.height;
            GL.sync.Width.Value = GL.width;
            GL.online = true;
        }
        if (GB != null)
        {
            GB.sync.Height.Value = GB.height;
            GB.sync.Width.Value = GB.width;
            GB.online = true;
        }
        Debug.Log("OnPointerUp called.");
    }

}