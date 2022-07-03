using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour, IInteractable
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera cam;

    void Start ()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // true every "checkRate" seconds
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // create a ray from the center of our screen pointing in the direction we're looking
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            // did we hit something?
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {

            }
            else
            {

            }
        }

    }
}

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}
