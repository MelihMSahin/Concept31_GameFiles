using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPathfinding : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Transform player;

    private Vector2[] directions = { Vector2.right, Vector2.down, Vector2.left, Vector2.right, new Vector2(1,1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1)};
    [SerializeField]  protected Vector2 moveDir;
    public float moveSpeed;
    public float maxWait;

    private bool isAtLastPlayerPosition = true;
    private Vector3 lastPlayerPosition;

	private void Awake()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform; //Get player location
	}

	// Start is called before the first frame update
	void Start()
    {
        moveDir = directions[Random.Range(0, directions.Length)]; //Pick an initial random direction
        StartCoroutine(RandomDir()); //Change direction of movement randomly
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        RaycastHit hit;

        float speed = Random.Range(0.5f, moveSpeed);
		if (Physics.Raycast(transform.position, player.position - transform.position, out hit, Mathf.Infinity)) //Is player visible
		{
            if (hit.collider.gameObject.CompareTag("Player")) //If player is visible, chase it
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.yellow);
                PlayerHit(speed);
            }
			else if (!isAtLastPlayerPosition) //If player was visible, get close to where you last saw it
			{
                LastSeenPos(speed);
            }
            else //Keep moving in the same direction
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.white);
                rigidbody.velocity = new Vector3(moveDir.x * speed, rigidbody.velocity.y, moveDir.y * speed);
            }
        }
        //rigidbody.MovePosition(new Vector3(moveDir.x * moveSpeed, transform.position.y, moveDir.y * moveSpeed) + transform.position);
        
    }

    protected virtual void PlayerHit(float speed) //Go to player
	{
        lastPlayerPosition = player.position;
        isAtLastPlayerPosition = false;
        rigidbody.velocity = new Vector3(Normalise(player.position.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(player.position.z, transform.position.z) * speed);
    }

    protected virtual void LastSeenPos(float speed) //Approach the last seen player position
	{
        rigidbody.velocity = new Vector3(Normalise(lastPlayerPosition.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(lastPlayerPosition.z, transform.position.z) * speed);
        if (lastPlayerPosition.magnitude - transform.position.magnitude < 3)
        {
            isAtLastPlayerPosition = true;
        }
    }

    protected float Normalise(float a, float b) //Simple function to normalise and avoid divide by 0 errors
	{
        float numerator = a - b;
        float denomenator = Mathf.Abs(numerator);
		if (denomenator == 0)
		{
            return 0;
		}
        return numerator / denomenator;
	}

    IEnumerator RandomDir() //Pick a random direciton after waiting a random amount of time
	{
        yield return new WaitForSecondsRealtime(Random.Range(0.5f, maxWait));
        moveDir = directions[Random.Range(0, directions.Length)];
        StartCoroutine(RandomDir());
    }

	private void OnTriggerEnter(Collider other) //If hit wall, move in the opposite direction
	{
        if (other.CompareTag("Floor"))
        {
            return;
        }
        else
        {
            /*
            Vector2 temp;
            do
            {
                temp = directions[Random.Range(0, directions.Length)];
            } while (false);//temp == moveDir);
            moveDir = temp;
            */
            moveDir = moveDir * -1;
		}
    }
}
