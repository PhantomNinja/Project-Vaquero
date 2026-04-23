using UnityEngine;
using UnityEngine.InputSystem;

public class VRRevolver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Revolver Objects")]
    [SerializeField] GameObject drum;
    RevolverDrum _drum;
    [SerializeField] GameObject drumPivot;
    [SerializeField] GameObject projectileSpawnPoint;
    [SerializeField] GameObject projectile;
    [SerializeField] float drumPivotRotationAmount;
    public bool drumInPosition;
    bool canFire;
    [Header("Revolver Attributes")]
    [SerializeField] float muzzleVelocity;

    [Header("Cylinder Attributes")]
    [SerializeField] int drumCapacity;
   

    void Start()
    {
        _drum = drum.GetComponent<RevolverDrum>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void fireProjectile()
    {
        
        if (_drum.chamberAligned)
        {
                
            _drum.StartCoroutine("rotateCylinder", 0.5f);
            GameObject bullet = Instantiate(projectile, projectileSpawnPoint.transform.position,projectileSpawnPoint.transform.rotation);
            bullet.GetComponent<Bullet>().setupProjectile(muzzleVelocity);
        }
    }
   
    void changeCylinderPosition()
    {
        // Rotates Cylinder pivot by cylinderPivotAmount 
        // TODO: Change this to use a hinge joint or something similar locking the cylinder when in line with the barrel and releasing on a button press
        if (!drumInPosition)
        {

            Quaternion angle = Quaternion.AngleAxis(drumPivotRotationAmount, drumPivot.transform.forward);
            drumPivot.transform.rotation = angle;
        }
        else
        {
            drumPivot.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

}
