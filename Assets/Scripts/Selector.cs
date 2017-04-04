using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;

public class Selector : MonoBehaviour, IInputHandler
{
    public GameObject selected_object;
	public float distance_selected;
    public Quaternion lockrot;
    public float max_dist = 10f;
    private bool raycast;
    private RaycastHit hit;
    public AudioClip sound_click;

    // Use this for initialization
    void Start () {
        selected_object = null;
        this.gameObject.AddComponent<AudioSource>();
        this.GetComponent<AudioSource>().clip = sound_click;
    }

    // Update is called once per frame
    void Update()
    {
        raycast = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
            out hit, max_dist);
        if (raycast)
        {
            hit.transform.gameObject.SendMessage("Raycast_Receiver", hit, SendMessageOptions.DontRequireReceiver);
        }
        if (selected_object != null)
        {
            selected_object.transform.LookAt(2 * transform.position - Camera.main.transform.position);
            selected_object.transform.localRotation = Quaternion.identity;
            /*if (raycast && hit.transform.gameObject.tag != "Graph")
                selected_object.transform.position = hit.transform.position;
            else
                selected_object.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance_selected;*/
            //selected_object.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance_selected;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            this.GetComponent<AudioSource>().Play();
        }
    }

    public void enable_disable(GameObject o)
    {
        if (selected_object == null && o != null)
        {
            selected_object = o;
            selected_object.transform.parent = Camera.main.transform;
            distance_selected = Vector3.Distance(transform.position, Camera.main.transform.position);
			lockrot = selected_object.transform.rotation;
            selected_object.GetComponent<SharePosition>().receive_data = false;
            selected_object.GetComponent<BoxCollider>().enabled = false;

        }
        else if (selected_object != null)
        {
            selected_object.transform.parent = null;
            selected_object.GetComponent<BoxCollider>().enabled = true;
            SyncSpawnedObject sync = selected_object.GetComponent<DefaultSyncModelAccessor>().SyncModel
                as SyncSpawnedObject;
            sync.Position.Value = selected_object.transform.position;
            sync.Rotation.Value = selected_object.transform.rotation;
            sync.Scale.Value = selected_object.transform.localScale;
            selected_object.GetComponent<SharePosition>().receive_data = true;
            selected_object = null;
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        enable_disable(null);
        Debug.Log("Deselect");

    }

    public void OnInputDown(InputEventData eventData)
    {
        /*if (raycast)
        {
            this.GetComponent<AudioSource>().Play();
            if (selected_object == null && hit.transform.gameObject.tag == "Graph")
            {
                selected_object = hit.transform.gameObject;
                selected_object.transform.parent = Camera.main.transform;
                distance_selected = Vector3.Distance(transform.position, Camera.main.transform.position);
                lockrot = selected_object.transform.rotation;
                selected_object.GetComponent<SharePosition>().receive_data = false;
                selected_object.GetComponent<BoxCollider>().enabled = false;

            }
            else if(selected_object != null)
            {
                selected_object.transform.parent = null;
                selected_object.GetComponent<BoxCollider>().enabled = true;
                SyncSpawnedObject sync = selected_object.GetComponent<DefaultSyncModelAccessor>().SyncModel
                    as SyncSpawnedObject;
                sync.Position.Value = selected_object.transform.position;
                sync.Rotation.Value = selected_object.transform.rotation;
                sync.Scale.Value = selected_object.transform.localScale;
                selected_object.GetComponent<SharePosition>().receive_data = true;
                selected_object = null;
            }
        }*/  
    }  
}
