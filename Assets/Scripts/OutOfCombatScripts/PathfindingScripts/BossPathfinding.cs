using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPathfinding : EnemyPathfinding
{
    protected override void FixedUpdate()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Maze")) //Stops movement if not in maze scene
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            RaycastHit hit;

            float speed = Random.Range(0.5f, moveSpeed);
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit, Mathf.Infinity)) // Find the player
            {
                if (hit.collider.gameObject.CompareTag("Player")) //Instead of going towards the player, go away from it
                {
                    speed *= -2;
                    //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.yellow);
                    rigidbody.velocity = new Vector3(Normalise(player.position.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(player.position.z, transform.position.z) * speed);
                }
                else //If player is out of sight, go random
                {
                    //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.white);
                    rigidbody.velocity = new Vector3(moveDir.x * speed, rigidbody.velocity.y, moveDir.y * speed);
                }
            }
            //rigidbody.MovePosition(new Vector3(moveDir.x * moveSpeed, transform.position.y, moveDir.y * moveSpeed) + transform.position);
        }
    }
}
