using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float smoothFactor = 1f;
    private Vector3 cameraOffset;
    [SerializeField]
    private TopDownMovement tdmScript;
    [SerializeField]
    private RoofReveal rrScript;
    private Vector3 Big;
    private Vector3 Medium;
    private Vector3 Small;
    [SerializeField]
    private string roofTag;
    Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        tdmScript = GameObject.Find("TopDownPlayer").GetComponent<TopDownMovement>();
        rrScript = GameObject.Find("Roofs").GetComponentInChildren<RoofReveal>();
        // Saves the current camera position in the editor
        cameraOffset = transform.position - player.transform.position;
        Medium = cameraOffset;
        Big = cameraOffset + new Vector3(0f, 2.7f, -1.5f);
        Small = cameraOffset - new Vector3(0f, 2.65f, 1.35f);
        newPosition = player.transform.position + Medium;
    }

    void Update()
    {
        if(tdmScript.GetRaycastedRoof())
        {
            roofTag = tdmScript.GetRaycastedRoof().tag;
        }
        else
        {
            roofTag = "Untagged";
        }
    }

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        switch (roofTag)
        {
            // Adjust position as per room size
            case "Roof,Big":
                // Calculate new position of the camera as per player's position
                newPosition = player.transform.position + Big;
                break;
            case "Roof,Medium":
                // Calculate new position of the camera as per player's position
                newPosition = player.transform.position + Medium;
                break;
            case "Roof,Small":
                // Calculate new position of the camera as per player's position
                newPosition = player.transform.position + Small;
                break;
            default:
                newPosition = player.transform.position + Medium;
                break;
        }
        //Debug.Log("CAMERA LOCAL: " + this.transform.localPosition);

        // Set camera's position
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
    }

    void FixedUpdate()
    {
        //*************************************STILL UNDER TESTING**********************************************************************
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, player.transform.position - this.transform.position, out hit))
        {
            Debug.Log("GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG"+hit.collider.gameObject);
            if(hit.collider.tag == "SeeThrough")
            {
                Renderer rend;
                rend = hit.collider.GetComponent<Renderer>();
                Color tempColor = rend.material.color;
                tempColor.a = 0.8f;
                rend.material.color = tempColor;
            }
        }
        //******************************************************************************************************************************
    }
}
