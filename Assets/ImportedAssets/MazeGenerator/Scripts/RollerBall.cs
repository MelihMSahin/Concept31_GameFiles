using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//<summary>
//Ball movement controlls and simple third-person-style camera
//</summary>
public class RollerBall : MonoBehaviour {

	public GameObject ViewCamera = null;
	public AudioClip JumpSound = null;
	public AudioClip HitSound = null;
	public AudioClip CoinSound = null;

	private Rigidbody mRigidBody = null;
	private AudioSource mAudioSource = null;
	private bool mFloorTouched = false;

	[Space]
	[SerializeField]
	private float toatlCameraOffsetMultiplier;
	[SerializeField]
	private float cameraOffsetUpMultiplier;

	[Space]
	[Header("Movement")]
	public PlayerMoveInput playerMove;
	public float movementSpeed = 5f;
	private InputAction move;
	private Vector2 moveDir;


	void Awake()
	{
		playerMove = new PlayerMoveInput();
	}

	void Start () {
		mRigidBody = GetComponent<Rigidbody> ();
		mAudioSource = GetComponent<AudioSource> ();
	}

	private void OnEnable()
	{
		move = playerMove.Player.Move;
		move.Enable();
	}

	private void OnDisable()
	{
		move.Disable();
	}

	void Update()
	{
		moveDir = move.ReadValue<Vector2>();
	}

	void FixedUpdate () {
		if (mRigidBody != null) {
			mRigidBody.velocity = new Vector3(moveDir.x * movementSpeed, mRigidBody.velocity.y, moveDir.y * movementSpeed);
			/*
			if (Input.GetButton ("Horizontal")) {
				mRigidBody.AddTorque(Vector3.back * Input.GetAxis("Horizontal")*10);
			}
			if (Input.GetButton ("Vertical")) {
				mRigidBody.AddTorque(Vector3.right * Input.GetAxis("Vertical")*10);
			}
			if (Input.GetButtonDown("Jump")) {
				if(mAudioSource != null && JumpSound != null){
					mAudioSource.PlayOneShot(JumpSound);
				}
				mRigidBody.AddForce(Vector3.up*200);
			}
			*/
		}

		
		if (ViewCamera != null) {
			Vector3 direction = (Vector3.up * cameraOffsetUpMultiplier + Vector3.back) * toatlCameraOffsetMultiplier;
			RaycastHit hit;
			Debug.DrawLine(transform.position,transform.position+direction,Color.red);
			if(Physics.Linecast(transform.position,transform.position+direction,out hit)){
				ViewCamera.transform.position = hit.point;
			}else{
				ViewCamera.transform.position = transform.position+direction;
			}
			ViewCamera.transform.LookAt(transform.position);
		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = true;
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.y > .5f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		} else {
			if (mAudioSource != null && HitSound != null && coll.relativeVelocity.magnitude > 2f) {
				mAudioSource.PlayOneShot (HitSound, coll.relativeVelocity.magnitude);
			}
		}
	}

	void OnCollisionExit(Collision coll){
		if (coll.gameObject.tag.Equals ("Floor")) {
			mFloorTouched = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Enemy"))
		{
			SceneManager.LoadScene("Combat");
		}

		/*
		if (other.gameObject.tag.Equals ("Coin")) {
			if(mAudioSource != null && CoinSound != null){
				mAudioSource.PlayOneShot(CoinSound);
			}
			Destroy(other.gameObject);
		}*/
	}
}
