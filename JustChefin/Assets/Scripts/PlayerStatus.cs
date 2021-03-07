using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //LIVES
    [SerializeField]
    int strikes;
    [SerializeField]
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        strikes = 3;
    }


    public int getStrike()
    {
        return strikes;
    }

    public void LoseLife()
    {
        strikes--;
        if(strikes <= 0)
        {
            isDead = true;
        }
    }

    public bool isAlive()
    {
        return !isDead;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
