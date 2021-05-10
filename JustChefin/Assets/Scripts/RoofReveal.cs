using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofReveal : MonoBehaviour
{
    private GameObject player;
    Renderer rend;
    [SerializeField]
    private TopDownMovement tdmScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Iris");
        tdmScript = GameObject.Find("Iris").GetComponent<TopDownMovement>();
        rend = gameObject.GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("Transparent/Diffuse");
        Color tempColor = rend.material.color;
        tempColor.a = 0.93f;
        rend.material.color = tempColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.DrawRay(pointToRaycastFrom, player.transform.position - pointToRaycastFrom, Color.green);
        // Check only if roof renderer exists
        if(rend.enabled)
        {
            if (tdmScript.GetRaycastedRoof())
            {
                // Disable renderer
                tdmScript.GetRaycastedRoof().GetComponent<Renderer>().enabled = false;
            }   
        }
    }
}
