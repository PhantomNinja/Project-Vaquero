using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
using Random = UnityEngine.Random;
using Color = UnityEngine.Color;
using UnityEngine.Rendering;
public class ShootingGalleryManager : MonoBehaviour
{
    public static ShootingGalleryManager Instance;

    [Header("Gallery Attributes")]
    public bool gameLive;
    [SerializeField] float gameLength;
    [SerializeField] int maxTargets;
    [SerializeField] AnimationCurve MultiplierCurve;
    [SerializeField] int Multiplier;
    [SerializeField] int combo;
    public int gameScore;
    public int HighestScore;
    float gameTimer;
    int maxTarget = 12;
    int activeTargets;
    [Header("Target Spawning")]
    [SerializeField] Transform TargetStache;
    [SerializeField] GameObject target;
    [SerializeField] GameObject LeftSpawnVolume;
    [SerializeField] GameObject RightSpawnVolume;    // have a point where the target navigates towards
    // targetBehavior influences math used to navigate
    enum targetBehavior {
        idle,
        linear,
        sin,
        circular,
    }
    struct Target
    {
        public GameObject Object;
        public Rigidbody rb;
        public targetBehavior behavior;
        public Vector3 destination;
        public int ID;
        public int lane;
    }
    List<Target> activeGalleryItems = new List<Target>();
    List<Target> disabledGalleryItems = new List<Target>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    void Start()
    {
        gameStart();
    }

    // Update is called once per frame
    void Update()
    {

        if (gameLive)
        {
            gameTimer += Time.deltaTime;
            if (gameTimer > gameLength)
            {

            }

            gameLogic();


        }
    }
    void spawnObjectEdgeOfBound()
    {
        Vector3 spawnPoint;
        Vector3 direction;
        int randomInt = Random.Range(0,2);
        switch (randomInt) // can't rotate gallery without breaking spawning
        {
            case 0: // left
                spawnPoint = randomVector3InBounds(LeftSpawnVolume.GetComponent<Collider>().bounds);
                direction = -transform.right;
                
                activateTarget(spawnPoint,direction);
                break;
            case 1: // right
                spawnPoint = randomVector3InBounds(RightSpawnVolume.GetComponent<Collider>().bounds);
                direction = transform.right;
                activateTarget(spawnPoint, direction);
                break;

        }
    }
    void gameLogic()
    {
     if (activeTargets < maxTargets)
        {
            spawnObjectEdgeOfBound();
        }   
    }
    public void gameStart()
    {
        gameLive = true;
        
        if (disabledGalleryItems.Count <= 0)
        {
            for (int i = 0; i < maxTargets; i++)
            {
                addTargetToGallery();
            }
        }
    }
    void activateTarget(Vector3 spawnPoint, Vector3 direction)
    {
        Target thisTarget = disabledGalleryItems[0];
        thisTarget.behavior = randomBehavior();
        thisTarget.Object.SetActive(true);
        thisTarget.rb.MovePosition(spawnPoint);
        activeGalleryItems.Add(thisTarget);
        disabledGalleryItems.Remove(thisTarget);
        activeTargets++;
    }
    void addTargetToGallery()
    {
        Target newTarget = new Target();
        newTarget.Object = Instantiate(target, TargetStache);
        newTarget.rb = newTarget.Object.GetComponent<Rigidbody>();
        newTarget.ID = newTarget.Object.GetHashCode();
        newTarget.behavior = targetBehavior.idle;
        newTarget.Object.SetActive(false);
        disabledGalleryItems.Add(newTarget);
    }
    public void removeTargetFromGallery(GameObject target, int score) 
    {
        addScore(score);
        Target thisTarget = returnTargetByGameObject(target);
        activeGalleryItems.Remove(thisTarget);
        activeTargets--;
    }

    void addScore(int score)
    {
        combo++;
        Multiplier = checkMultiplier();
        gameScore = score * Multiplier;
    }

    void resetCombo()
    {
        combo = 0;
        Multiplier = checkMultiplier();
    }


    Target returnTargetByGameObject(GameObject target)
    {
        Target thisTarget;
        int targetHash = target.GetHashCode();
        for (int i = 0; i < activeGalleryItems.Count; i++)
        {
            if (targetHash == activeGalleryItems[i].ID)
            {
                thisTarget = activeGalleryItems[i];
                return thisTarget;
            }
        }
        thisTarget = new Target();
        Debug.Log("Returned empty Target");
        return thisTarget;
    }
    targetBehavior randomBehavior()
    {
        int randomInt = Random.Range(0, sizeof(targetBehavior));
        targetBehavior randomBehavior;
        randomBehavior = (targetBehavior)randomInt;
        return randomBehavior;
    }
    int checkMultiplier()
    {
        int multiplierValue;
        multiplierValue = Mathf.RoundToInt(MultiplierCurve.Evaluate(combo));
        return multiplierValue;
    }

    Vector3 randomVector3InBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
