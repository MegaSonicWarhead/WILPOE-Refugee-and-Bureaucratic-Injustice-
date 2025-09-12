using UnityEngine;

public class TriggerDebugger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER: " + other.name + " | Tag: " + other.tag);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("TRIGGER STAY: " + other.name + " | Tag: " + other.tag);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("TRIGGER EXIT: " + other.name + " | Tag: " + other.tag);
    }
}
