using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int moveSpeed;

    private Rigidbody2D playerRigidbody;
    private Vector3 playerDirection;

    public PlayerData playerData { get; private set; } = new PlayerData();

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerDirection = Vector3.zero;
    }

    /*
     *키보드 입력이랑 움직이는 부분은 안정성을 위해 분리시킴
     *Update -> 키보드 input
     *FixedUpdate -> movement
     */
    private void Update()
    {
        KeyDir();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void KeyDir()
    {
        //left, right
        playerDirection.x = Input.GetAxis("Horizontal");
        //up, down
        playerDirection.y = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        playerRigidbody.MovePosition((Vector3)playerRigidbody.position + (playerDirection * moveSpeed * Time.fixedDeltaTime));
    }

}
