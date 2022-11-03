using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int Speed;
    private Rigidbody2D Rigid;
    private Vector3 Dir;

    private void Start()
    {
        Rigid = GetComponent<Rigidbody2D>();
        Dir = Vector3.zero;
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
        Dir.x = Input.GetAxis("Horizontal");    //left, right
        Dir.y = Input.GetAxis("Vertical");      //up, down
    }

    private void Move()
    {
        Rigid.MovePosition((Vector3)Rigid.position + (Dir * Speed * Time.fixedDeltaTime));
    }

}
