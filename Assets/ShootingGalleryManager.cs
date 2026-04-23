using NUnit.Framework;
using System.Collections.Generic;
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
    List<GameObject> galleryItems;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLive)
        {
            gameTimer += Time.deltaTime;
            if (gameTimer > gameLength)
            {
                // end game
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
    }

    public void addTargetToGallery(GameObject target)
    {
        galleryItems.Add(target);   
    }
    public void removeTargetFromGallery(GameObject target, int score) 
    {
        addScore(score);
        galleryItems.Remove(target);
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
}
