using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPathfinding : EnemyPathfinding
{
    protected override void FixedUpdate()
    {
        RaycastHit hit;

        float speed = Random.Range(0.5f, moveSpeed);
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                speed *= -2;
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.yellow);
                rigidbody.velocity = new Vector3(Normalise(player.position.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(player.position.z, transform.position.z) * speed);
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.white);
                rigidbody.velocity = new Vector3(moveDir.x * speed, rigidbody.velocity.y, moveDir.y * speed);
            }
        }
        //rigidbody.MovePosition(new Vector3(moveDir.x * moveSpeed, transform.position.y, moveDir.y * moveSpeed) + transform.position);

    }
}
