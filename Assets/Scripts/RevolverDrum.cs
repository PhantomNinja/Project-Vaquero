using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RevolverDrum : MonoBehaviour
{
    struct chamber
    {
        bool isLoaded;
    }


    [Header("Revolver Attributes")]
    [SerializeField] int drumCapacity;
    [SerializeField] float chamberDistanceFromDrumCenter;
    List<chamber> loadedChambers;
    public bool chamberAligned;
    float degreesBetweenChambers;

    private void Start()
    {
        chamberAligned = true;
        degreesBetweenChambers = drumCapacity / 360;
        //for (int i = 0; i < drumCapacity; i++)
        //{
        //    loadedChambers.Add(new chamber());
        //}
    }

    public IEnumerator rotateCylinder( float lengthOfRotation)
    {
        float rotationAmount;
        rotationAmount = 360 / drumCapacity; // angular rotation between each chamber
        chamberAligned = false;
        float rotationStep = drumCapacity;  
        rotationStep = rotationAmount / rotationStep;
        for (float rotation = 0; rotation < rotationAmount; rotation += rotationStep)
        {
            Vector3 eulerRotation = new Vector3(0,0,rotationStep);
            transform.rotation = transform.rotation * Quaternion.Euler(eulerRotation);
            yield return new WaitForSeconds(lengthOfRotation / rotationStep);
        }
        chamberAligned = true;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);
    }

    void loadChamber(int chamber)
    {
        
    }
}
