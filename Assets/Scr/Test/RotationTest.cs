using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public Vector2 velocity;
    public Transform wall;

    [Space(20)]
    public Vector3 direction;
    public float angle;

    [Space(20)]
    public float a;
    public float b;
    public Vector3 u;
    public Vector3 w;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = transform.position - wall.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        wall.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnDrawGizmos()
    {
        angle = Mathf.Atan2(this.velocity.y, this.velocity.x) * Mathf.Rad2Deg - 90;
        direction = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Vector3.zero, direction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, velocity);


        a = Vector3.Dot(this.velocity, wall.up);
        b = Vector3.Dot(wall.up, wall.up);

        u = wall.up * a / b;
        w = (Vector3) velocity - u;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, w - u);
    }
}
