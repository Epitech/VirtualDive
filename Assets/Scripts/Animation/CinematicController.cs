using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CinematicController : MonoBehaviour
{
    public delegate void CinematicCompletionCallback();

    public CinematicCompletionCallback completionClb;
    public GameObject targetObj;
    public Animation targetObjAnim;
    public AnimationClip activeAnim;
    public bool isRunning;

    void Start()
    {

    }

    void Update()
    {
        if (isRunning)
        {
            if (targetObjAnim.isPlaying == false)
            {
                isRunning = false;
                completionClb();
            }
        }
    }

    public void RunAnimation(GameObject obj, AnimationClip anm, CinematicCompletionCallback clb)
    {
        completionClb = clb;
        targetObj = obj;
        activeAnim = anm;
        targetObjAnim = obj.GetComponent<Animation>();
        targetObjAnim.Play(anm.name);
        isRunning = true;
    }
}

