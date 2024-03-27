using UnityEngine;

public class NPCAnimInMenu : MonoBehaviour
{
    Animator poseAnimator;

    public float poseNumber;

    private void Awake()
    {
        poseAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        poseAnimator.SetFloat("PoseNumber", poseNumber);
    }

}
