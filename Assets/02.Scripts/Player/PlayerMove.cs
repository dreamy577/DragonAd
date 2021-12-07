using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region * 변수

    private float speed = 10f; //플레이어의 초기 속도값 =10
    Transform tr; // 플레이어 트랜스폼 제어 변수 


    Vector3 moveDirection = Vector3.zero;

    float turnSpeed = 4.0f; //회전 스피드
  
    Animator anim; //애니메이션 컴포넌트 

    private CharacterController controller; //플레이어 컨트롤러 
    private Vector3 moveDir; //플레이어 벡터
    private float grav = 40.0f; //플레이어 중력
    public float jumpPower = 16f; //플레이어의 점프력

    private float x = 0.0f;
    private float y = 0.0f;

    #endregion

    /*--------------------------------------------------------------------*/

    #region * Unity 함수 

    void Start()
    {
        tr = this.transform;
        controller = GetComponent<CharacterController>();
        anim = this.GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        characterCtrl();

        x += turnSpeed * Input.GetAxis("Mouse X");
        y += turnSpeed * Input.GetAxis("Mouse Y");

        x = Mathf.Clamp(x, -180f, 180f);
        y = Mathf.Clamp(y, 0, 15f);

        transform.eulerAngles = new Vector3(-y, x, 0.0f);
    }


    #endregion

    /*--------------------------------------------------------------------*/

    public void characterCtrl()
    {
        if (controller.isGrounded)
        {
            //기본 조작 
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            moveDirection = new Vector3(x, 0, z) * speed;

            if (Input.GetButton("Jump")) //점프
            {
                moveDirection.y = jumpPower * 3;
                anim.SetBool("isJump", true);
            }

        }
        moveDirection.y -= grav * Time.deltaTime; //점프 후 중력 적용하여 착지
        controller.Move(moveDirection * Time.deltaTime);

        //float r = Input.GetAxis("Mouse X"); //플레이어 회전 
        //transform.Rotate(Vector3.up * turnSpeed * r);
        //Debug.Log("좌우회전");

        //float t = Input.GetAxis("Mouse Y");
        //transform.Rotate(Vector3.up * turnSpeed * t);
        //Debug.Log("상하회전");

    }


 
}
