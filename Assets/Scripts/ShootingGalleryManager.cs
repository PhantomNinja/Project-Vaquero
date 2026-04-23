using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootingGalleryManager : MonoBehaviour
{
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

    [Header("Target Spawning")]
    [SerializeField] Transform TargetStache;
    [SerializeField] GameObject target;
    // have a point where the target navigates towards
    // targetBehavior influences math used to navigate
    enum targetBehavior {
        linear,
        sin,
    }
    struct Target
    {
        public GameObject Object;
        public targetBehavior behavior;
        public Vector3 destination;
        public int ID;
        public int lane;
    }
    List<Target> activeGalleryItems = new List<Target>();
    List<Target> disabledGalleryItems = new List<Target>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    

    void gameLogic()
    {
        
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

    public void addTargetToGallery()
    {
        Target newTarget = new Target();
        newTarget.Object = Instantiate(target, TargetStache);
        newTarget.ID = newTarget.Object.GetHashCode();
        newTarget.Object.SetActive(false);
        
    }
    public void removeTargetFromGallery(GameObject target, int score) 
    {
        addScore(score);
        Target thisTarget = returnTargetByGameObject(target);
    }

    void addScore(int score)
    {
        combo++;
        Multiplier = checkMultiplier();
        gameScore = score * Multiplier;
    }
    int checkMultiplier()
    {
        int multiplierValue;
        multiplierValue = Mathf.RoundToInt(MultiplierCurve.Evaluate(combo));
        return multiplierValue;
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
}
