using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSkills : MonoBehaviour
{
    public Transform other; //���� ��ġ 
    public float closeDistance = 4.0f; //�Ÿ� ����


    void Update()
    {
        pickAtk();
    }

    public void pickAtk() //������_����Ʈ �߻� �� ���� �Ÿ� ���� �� ��� ��� 
    {
        if (other)
        {
            Vector3 offset = other.position - transform.position;
            float sqrLen = offset.sqrMagnitude;

            if (sqrLen < closeDistance * closeDistance)
            {
                GameObject.Destroy(other);
            }
        }
    }

}
