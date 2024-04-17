using System.Collections;
using UnityEngine;

public class StatisticsMenuNPC : MonoBehaviour
{
    bool npcRotate;
    public bool allowForMove;
    bool workPermit;

    Animator anim;
    Rigidbody rb;
    SettingsScript settingsScript;
    Vector3 startPosition;

    public AudioSource[] footSteps;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        settingsScript = FindAnyObjectByType<SettingsScript>();

        startPosition = transform.position;
    }

    private void Update()
    {
        if (workPermit)
        {
            if (allowForMove)
            {
                if (npcRotate)
                    rb.velocity = new Vector3(0f, 0f, 1f);
                else
                    rb.velocity = new Vector3(0, 0f, -1f);
            }
        }

        foreach (AudioSource step in footSteps)
        {
            step.volume = settingsScript.GetSFXVolume();
        } 
    }

    IEnumerator ChangeRotation()
    {
        while (workPermit)
        {
            yield return new WaitForSeconds(15f);
            npcRotate = !npcRotate;

            if (npcRotate)
            {
                anim.SetBool("WalkingBack", true);
                anim.SetBool("WalkingFwd", false);
            }
            else
            {
                anim.SetBool("WalkingBack", false);
                anim.SetBool("WalkingFwd", true);
            }

            allowForMove = true;
        }
        
    }

    public void NPCRot()
    {
        if (npcRotate)
            transform.Rotate(0f, 180f, 0f);
        else
            transform.Rotate(0f, -180f, 0f);
    }

    public void StartRot()
    {
        workPermit = true;
        StartCoroutine(ChangeRotation());
    }

    public void ResetAll()
    {
        StopCoroutine(ChangeRotation());

        anim.SetBool("WalkingBack", false);
        anim.SetBool("WalkingFwd", false);
        anim.SetTrigger("ClosingMenu");

        rb.velocity = Vector3.zero;

        workPermit = false;
        allowForMove = false;
        npcRotate = false;

        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(0f,180f,0f);
    }

    public void FootStepRight()
    {
        footSteps[0].Play();
    }

    public void FootStepLeft()
    {
        footSteps[1].Play();
    }
}
