using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIControllerOculus : MonoBehaviour
{
    public Camera uiCam;
    public UIController staticController;

    public float activeFocusTimer = 0.0f;
    public float focusTime = 1.5f;

    public EventSystem events;

    // Use this for initialization
    void Start()
    {
        //events = transform.FindChild ("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.VR.VRDevice.isPresent)
        {
            // Rotate dynamically fixed UI
            staticController.panelHUD.transform.localRotation = new Quaternion(UnityEngine.VR.InputTracking.GetLocalRotation(0).x * -0.8f,
                UnityEngine.VR.InputTracking.GetLocalRotation(0).y * -1.2f, 0.0f, 1.0f);

            if (!staticController.uiLocked)
            {
                // Raytrace pointer location
                Ray ray = uiCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit[] hits = Physics.RaycastAll(ray);
                bool focused = false;

                foreach (RaycastHit hit in hits)
                {
                    GameObject obj = hit.collider.gameObject;
                    Button bt = obj.transform.parent.gameObject.GetComponent<Button>();
                    if (bt)
                    {
                        events.SetSelectedGameObject(bt.gameObject);
                        focused = true;
                    }
                }
                activeFocusTimer -= Time.deltaTime;
                if (focused == false)
                {
                    activeFocusTimer = focusTime;
                    events.SetSelectedGameObject(null);
                }
                if (activeFocusTimer <= 0.0f)
                {
                    events.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
                    // Simulate lock
                    activeFocusTimer = 150.0f;
                }
            }
        }
    }
}
