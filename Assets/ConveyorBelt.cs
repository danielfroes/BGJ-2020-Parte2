using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float Speed;

    // Start is called before the first frame update
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("GarbagePile"))
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * Speed;
        }
    }
}
