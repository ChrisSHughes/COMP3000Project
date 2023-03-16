using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    /// <summary>
    /// Standard, damage speed and target for projectile.
    /// StartPos and EndPos are the start and end of the projectiles path retrospectively. 
    /// control point is how hight the projectile will go before dipping back down.
    /// duration is how long it takes to get from point a to point b. The shorter the time, the faster the bullet.
    /// </summary>
    public int Damage;
    public float speed;
    public Transform target;

    Vector3 startPosition;
    Vector3 midPoint;
    Vector3 controlPoint;  
    Vector3 endPosition;

    GameObject midpoint;

    float currentDuration;
    float duration = 1.0f;

    void Start()
    {
        startPosition = this.transform.position;
        midPoint = new Vector3((transform.position.x + target.position.x) / 2, (transform.position.y + target.position.y) / 2, (transform.position.z + target.position.z) / 2);
        controlPoint = new Vector3(midPoint.x, midPoint.y + 0.5f, midPoint.z);
        endPosition = target.position;

        midpoint = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), midPoint, Quaternion.identity);
        midpoint.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

    }


    void Update()
    {
        midPoint = new Vector3((transform.position.x + target.position.x) / 2, (transform.position.y + target.position.y) / 2, (transform.position.z + target.position.z) / 2);
        midpoint.transform.position = midPoint;
        endPosition = new Vector3(target.position.x, target.position.y + 0.2f, target.position.z);
        this.transform.position = CalculateBezierPoint(currentDuration / duration, startPosition, endPosition, controlPoint);

        transform.rotation = Quaternion.LookRotation(Vector3.forward);

        currentDuration += Time.deltaTime;
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 startPosition, Vector3 endPosition, Vector3 controlPoint)
    {
        float u = 1 - t;
        float uu = u * u;

        Vector3 point = uu * startPosition;
        point += 2 * u * t * controlPoint;
        point += t * t * endPosition;

        return point;
    }
}
