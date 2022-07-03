using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
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
                // is this not our current interactable?
                // if so, set it as our current interactable
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[E]</b> {0}", curInteractable.GetInteractPrompt());
    }

    // called when we press the "E" button - managed by the Input System
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // did we press down this frame and are we hovering over an interactable?
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}
