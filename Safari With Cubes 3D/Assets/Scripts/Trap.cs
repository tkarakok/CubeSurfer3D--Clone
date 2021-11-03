using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int unit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dead")
        {
            PlayerController.Current.Die();
        }
    }
}
