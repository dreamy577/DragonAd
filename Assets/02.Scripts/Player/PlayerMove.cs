using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region * ����

    private float speed = 10f; //�÷��̾��� �ʱ� �ӵ��� =10
    Transform tr; // �÷��̾� Ʈ������ ���� ���� 


    Vector3 moveDirection = Vector3.zero;

    float turnSpeed = 4.0f; //ȸ�� ���ǵ�
  
    Animator anim; //�ִϸ��̼� ������Ʈ 

    private CharacterController controller; //�÷��̾� ��Ʈ�ѷ� 
    private Vector3 moveDir; //�÷��̾� ����
    private float grav = 40.0f; //�÷��̾� �߷�
    public float jumpPower = 16f; //�÷��̾��� ������

    private float x = 0.0f;
    private float y = 0.0f;

    #endregion

    /*--------------------------------------------------------------------*/

    #region * Unity �Լ� 

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
            //�⺻ ���� 
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            moveDirection = new Vector3(x, 0, z) * speed;

            if (Input.GetButton("Jump")) //����
            {
                moveDirection.y = jumpPower * 3;
                anim.SetBool("isJump", true);
            }

        }
        moveDirection.y -= grav * Time.deltaTime; //���� �� �߷� �����Ͽ� ����
        controller.Move(moveDirection * Time.deltaTime);

        //float r = Input.GetAxis("Mouse X"); //�÷��̾� ȸ�� 
        //transform.Rotate(Vector3.up * turnSpeed * r);
        //Debug.Log("�¿�ȸ��");

        //float t = Input.GetAxis("Mouse Y");
        //transform.Rotate(Vector3.up * turnSpeed * t);
        //Debug.Log("����ȸ��");

    }


 
}
