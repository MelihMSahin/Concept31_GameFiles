using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPathfinder : EnemyPathfinding
{
	protected override void FixedUpdate()
	{
        RaycastHit hit;

        float speed = Random.Range(0.5f, moveSpeed);
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.yellow);
                speed *= 2;
                rigidbody.velocity = new Vector3(moveDir.x * speed, rigidbody.velocity.y, moveDir.y * speed);
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.white);
                rigidbody.velocity = new Vector3(moveDir.x * speed, rigidbody.velocity.y, moveDir.y * speed);
            }
        }
    }
}
