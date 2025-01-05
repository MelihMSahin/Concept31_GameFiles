using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPathfinding : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Transform player;

    private Vector2[] directions = { Vector2.right, Vector2.down, Vector2.left, Vector2.right, new Vector2(1,1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1)};
<<<<<<< Updated upstream
    [SerializeField]  private Vector2 moveDir;
=======
    [SerializeField] protected Vector2 moveDir;

>>>>>>> Stashed changes
    public float moveSpeed;
    public float maxWait;

    private bool isAtLastPlayerPosition = true;
    private Vector3 lastPlayerPosition;

	private void Awake()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Start is called before the first frame update
	void Start()
    {
        moveDir = directions[Random.Range(0, directions.Length)];
        StartCoroutine(RandomDir());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        float speed = Random.Range(0.5f, moveSpeed);
		if (Physics.Raycast(transform.position, player.position - transform.position, out hit, Mathf.Infinity))
		{
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.yellow);
                lastPlayerPosition = player.position;
                isAtLastPlayerPosition = false;
                rigidbody.velocity = new Vector3(Normalise(player.position.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(player.position.z, transform.position.z) * speed);
            }
			else if (!isAtLastPlayerPosition)
			{
                rigidbody.velocity = new Vector3(Normalise(lastPlayerPosition.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(lastPlayerPosition.z, transform.position.z) * speed);
				if (lastPlayerPosition.magnitude - transform.position.magnitude < 3)
				{
                    isAtLastPlayerPosition = true;
				}
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.white);
                rigidbody.velocity = new Vector3(moveDir.x * speed, rigidbody.velocity.y, moveDir.y * speed);
            }
        }
        //rigidbody.MovePosition(new Vector3(moveDir.x * moveSpeed, transform.position.y, moveDir.y * moveSpeed) + transform.position);
        
    }

<<<<<<< Updated upstream
    private float Normalise(float a, float b)
=======
    protected virtual void PlayerHit(float speed)
	{
        lastPlayerPosition = player.position;
        isAtLastPlayerPosition = false;
        rigidbody.velocity = new Vector3(Normalise(player.position.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(player.position.z, transform.position.z) * speed);
    }

    protected virtual void LastSeenPos(float speed)
	{
        rigidbody.velocity = new Vector3(Normalise(lastPlayerPosition.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(lastPlayerPosition.z, transform.position.z) * speed);
        if (lastPlayerPosition.magnitude - transform.position.magnitude < 3)
        {
            isAtLastPlayerPosition = true;
        }
    }

    private void Pathfinding()
	{
        RaycastHit hit;

        float speed = Random.Range(0.5f, moveSpeed);
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.yellow);
                lastPlayerPosition = player.position;
                isAtLastPlayerPosition = false;
                rigidbody.velocity = new Vector3(Normalise(player.position.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(player.position.z, transform.position.z) * speed);
            }
            else if (!isAtLastPlayerPosition)
            {
                rigidbody.velocity = new Vector3(Normalise(lastPlayerPosition.x, transform.position.x) * speed, rigidbody.velocity.y, Normalise(lastPlayerPosition.z, transform.position.z) * speed);
                if (lastPlayerPosition.magnitude - transform.position.magnitude < 3)
                {
                    isAtLastPlayerPosition = true;
                }
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(player.position - transform.position) * hit.distance, Color.white);
                rigidbody.velocity = new Vector3(moveDir.x * speed, rigidbody.velocity.y, moveDir.y * speed);
            }
        }
    }


    protected float Normalise(float a, float b)
>>>>>>> Stashed changes
	{
        float numerator = a - b;
        float denomenator = Mathf.Abs(numerator);
		if (denomenator == 0)
		{
            return 0;
		}
        return numerator / denomenator;
	}

    IEnumerator RandomDir()
	{
        yield return new WaitForSecondsRealtime(Random.Range(0.5f, maxWait));
        moveDir = directions[Random.Range(0, directions.Length)];
        StartCoroutine(RandomDir());
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
        {
            //SceneManager.LoadScene("Combat");
        }
        else if (other.CompareTag("Floor"))
        {
            
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
