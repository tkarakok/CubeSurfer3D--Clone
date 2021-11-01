using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingCube : MonoBehaviour
{

    public void AddCube(bool add)
    {
        if (add)
        {
            LevelController.Current.ChangeScore(1);
            int cubeCount = PlayerController.Current.cubes.Count;
            transform.localPosition = new Vector3(transform.localPosition.x, -.1f * (cubeCount - 1) - 0.25f, transform.localPosition.z);
            transform.localScale = new Vector3(.5f, transform.localScale.y, .5f);
        }
        else
        {
            PlayerController.Current.DestroyCube(this);
        }
            
        
        
    }
}
