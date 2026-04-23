using Unity.Mathematics;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [Header("Score variables")]
    [SerializeField] AnimationCurve scoreCurve;
    [SerializeField] float maxScoreValue;
    Vector3 faceCenter;

    [Header("Debug Values")]
    public bool debugGizmos;
    Vector3 lastPointHit;

    private void Update()
    {
        if (debugGizmos)
        {
            Vector3 forwardNormal = transform.forward;
            forwardNormal = Vector3.ClampMagnitude(forwardNormal, transform.localScale.y / 2);
            faceCenter = transform.position + forwardNormal; // this is calculated everytime since the target moves
            Debug.DrawLine(lastPointHit, faceCenter);
        }
    }
    int calculateScore(Vector3 position)
    {
        lastPointHit = position;
        Vector3 forwardNormal = transform.forward;
        forwardNormal = Vector3.ClampMagnitude(forwardNormal, transform.localScale.y / 2);
        faceCenter = transform.position + forwardNormal; // this is calculated everytime since the target moves
        Debug.DrawLine(faceCenter, position);
        float distanceFromCenter = Vector3.Distance(position, faceCenter);
        float percentage = (0.6f - distanceFromCenter) / 0.6f;
        float accuracy = scoreCurve.Evaluate(percentage);
        float score = accuracy * maxScoreValue;
        int finalScore = (int)math.round(score);
        Debug.Log("Score " + finalScore + ", Accuracy " + accuracy);
        return finalScore;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION");
        if (!(collision.gameObject.GetComponent<Bullet>() == null))
        {
            ContactPoint contact = collision.GetContact(0);
            int score = calculateScore(contact.point);
            ShootingGalleryManager.Instance.removeTargetFromGallery(gameObject, score);
            Destroy(collision.gameObject);
        }
    }


}
