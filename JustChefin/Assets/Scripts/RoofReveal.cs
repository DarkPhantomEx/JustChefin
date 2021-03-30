using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofReveal : MonoBehaviour
{
    private GameObject player;
    private Vector3 RaycastPointOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("TopDownPlayer");
        // Height of the point to start raycasting from
        RaycastPointOffset = new Vector3(0f, 10f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Updated point to ray cast from as per player's position
        Vector3 pointToRaycastFrom = player.transform.position + RaycastPointOffset;
        //Debug.DrawRay(pointToRaycastFrom, player.transform.position - pointToRaycastFrom, Color.green);
        // Raycast only if roof renderer exists
        if(this.GetComponent<Renderer>().enabled)
        {
            RaycastHit hit;
            if (Physics.Raycast(pointToRaycastFrom, player.transform.position - pointToRaycastFrom, out hit))
            {
                //Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Roof")
                    // Disable renderer
                    hit.collider.GetComponent<Renderer>().enabled = false;
            }   
        }
    }
}
