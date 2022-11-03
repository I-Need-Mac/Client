using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int speed;
    private Rigidbody2D rigid;
    private Vector3 dir;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        dir = Vector3.zero;
    }

    /*
     *키보드 입력이랑 움직이는 부분은 안정성을 위해 분리시킴
     *Update -> 키보드 input
     *FixedUpdate -> 움직임 구현
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
        dir.x = Input.GetAxis("Horizontal");    //left, right
        dir.y = Input.GetAxis("Vertical");      //up, down
    }

    private void Move()
    {
        rigid.MovePosition((Vector3)rigid.position + (dir * speed * Time.fixedDeltaTime));
    }

}
