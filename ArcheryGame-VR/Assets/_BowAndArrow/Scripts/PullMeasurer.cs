using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PullMeasurer : XRBaseInteractable
{
    // Hidden in Inspector, would need to be serializable, and added to custom editor
    public class PullEvent : UnityEvent<Vector3, float> { }
    public PullEvent Pulled = new PullEvent();

    public Transform start = null;
    public Transform end = null;

    private float pullAmount = 0.0f;
    public float PullAmount => pullAmount;

    private GameObject topParicleGO, bottomParticleGO;
    

    private XRBaseInteractor pullingInteractor = null;

    private ChangeArrow changeArrowScript;
    private int defaultEmissionRate = 2;

    private void Start()
    {
        changeArrowScript = FindObjectOfType<ChangeArrow>();
        topParicleGO = GameObject.Find("TopParticle");
        bottomParticleGO = GameObject.Find("BottomParticle");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs     args)
    {
        base.OnSelectEntered(args);

        // Set interactor for measurement
        pullingInteractor = args.interactor;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Clear interactor, and reset pull amount for animation
        pullingInteractor = null;

        // Reset everything
        SetPullValues(start.position, 0.0f);
    }

    [Obsolete]
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected)
        {
            // Update pull values while the measurer is grabbed
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
                CheckForPull();
        }
    }

    [Obsolete]
    private void CheckForPull()
    {
        // Use the interactor's position to calculate amount
        Vector3 interactorPosition = pullingInteractor.transform.position;

        // Figure out the new pull value, and it's position in space
        float newPullAmount = CalculatePull(interactorPosition);
        Vector3 newPullPosition = CalculatePosition(newPullAmount);

        // Check if we need to send out event
        SetPullValues(newPullPosition, newPullAmount);

        SetParticleValue(newPullAmount);
    }

    [Obsolete]
    private void SetParticleValue(float newPullAmount)
    {
        if (changeArrowScript.haveAnyChange)
        {
            for (int i = 0; i < 5; i++)
            {
                int x = i * 2;
                if(changeArrowScript.weponNumber == i)
                {
                    changeArrowScript.bowParticals[x+1].SetActive(true);
                    changeArrowScript.bowParticals[x+1].GetComponent<ParticleSystem>().Play();
                    changeArrowScript.bowParticals[x].SetActive(true);
                    changeArrowScript.bowParticals[x].GetComponent<ParticleSystem>().Play();
                    //Debug.Log("açik : " + i);
                }
                else
                {
                    changeArrowScript.bowParticals[x].SetActive(false);
                    changeArrowScript.bowParticals[x+1].SetActive(false);
                    //Debug.Log("kapalı : " + i);
                }
            }
            changeArrowScript.haveAnyChange = false;
        }
        ChangeBowParticle((int)(newPullAmount * 20));
        
        /*
        topParicleGO.GetComponent<ParticleSystem>().emissionRate = (int)(newPullAmount * 20);
        bottomParticleGO.GetComponent<ParticleSystem>().emissionRate = (int)(newPullAmount * 20);
        topParicleGO.transform.rotation = Quaternion.Euler(((newPullAmount * 45)+90), 0, 0);
        bottomParticleGO.transform.rotation = Quaternion.Euler((270-(newPullAmount * 45)), 0, 0);
        */

        /*
        Debug.Log("emission" + topParicleGO.GetComponent<ParticleSystem>().emissionRate);
        Debug.Log("emission2" + bottomParticleGO.GetComponent<ParticleSystem>().emissionRate);
        Debug.Log("emission222" + bottomParticleGO.GetComponent<ParticleSystem>().emission);
        Debug.Log("topParicleGO_R" + topParicleGO.transform.rotation);
        Debug.Log("bottomParticleGO_R" + bottomParticleGO.transform.rotation);
        Debug.Log("newPullAmount" + newPullAmount);
        */
    }
    [Obsolete]
    public void ChangeBowParticle(int emissionRate)
    {
        Debug.Log("ChangeBowParticle : ");
        switch (changeArrowScript.weponNumber)
        {
            case 0:
                changeArrowScript.bowParticals[0].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                changeArrowScript.bowParticals[1].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                break;
            case 1:
                changeArrowScript.bowParticals[2].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                changeArrowScript.bowParticals[3].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                break;
            case 2:
                changeArrowScript.bowParticals[4].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                changeArrowScript.bowParticals[5].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                break;
            case 3:
                changeArrowScript.bowParticals[6].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                changeArrowScript.bowParticals[7].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                break;
            case 4:
                changeArrowScript.bowParticals[8].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                changeArrowScript.bowParticals[9].GetComponent<ParticleSystem>().emissionRate = emissionRate;
                break;
            default:
                Debug.LogWarning("Unexpected wepon number");
                break;
        }
    }
    [Obsolete]
    public void ResetBowParticle()
    {
        ChangeBowParticle(defaultEmissionRate);
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        // Direction, and length
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;

        // Figure out out the pull direction
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();

        // What's the actual distance?
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        pullValue = Mathf.Clamp(pullValue, 0.0f, 1.0f);

        return pullValue;
    }

    private Vector3 CalculatePosition(float amount)
    {
        // Find the actual position of the hand
        return Vector3.Lerp(start.position, end.position, amount);
    }

    private void SetPullValues(Vector3 newPullPosition, float newPullAmount)
    {
        // If it's a new value
        if (newPullAmount != pullAmount)
        {
            pullAmount = newPullAmount;
            Pulled?.Invoke(newPullPosition, newPullAmount);
        }
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        // Only let direct interactors pull the string
        return base.IsSelectableBy(interactor) && IsDirectInteractor(interactor);
    }

    private bool IsDirectInteractor(XRBaseInteractor interactor)
    {
        return interactor is XRDirectInteractor;
    }

    private void OnDrawGizmos()
    {
        // Draw line from start to end point
        if (start && end)
            Gizmos.DrawLine(start.position, end.position);
    }
}
