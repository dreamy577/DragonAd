using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMoveUtil :MonoBehaviour
{
    //player move_(캐릭터 컨트롤러, 이동할 위치, 이동 속도, 회전 속도) 인자값 

    public static float MoveFrame(CharacterController characterController,Transform target,float moveSpeed,float turnSpeed)
    {
        Transform t = characterController.transform;
        Vector3 dir = target.position - t.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
        Vector3 targetPos = t.position + dirXZ;
        Vector3 framePos = Vector3.MoveTowards(t.position, targetPos, moveSpeed * Time.deltaTime);
        characterController.Move(framePos - t.position + Physics.gravity);
        RotateToDir(t, target, turnSpeed);
        return
        Vector3.Distance(framePos, targetPos);
    }

    //플레이어 회전_(플레이어 위치, 회전 각도, 회전 속도) 인자값을 MoveFrame에서 불러오기
    public static void RotateToDir(Transform self,Transform target,float turnSpeed)
    {
        Vector3 dir = target.position - self.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);


        if(dirXZ==Vector3.zero) //회전 방향이 더 이상 필요 없는 경우 
        {
            return;  
        }
        //회전
        self.rotation = Quaternion.RotateTowards(self.rotation, Quaternion.LookRotation(dirXZ), turnSpeed * Time.deltaTime);
    }

    public static void RotateToDirBurst(Transform self,Transform target) //긴급 회전 (피격 등)
    {
        Vector3 dir=target.position - self.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

        if(dirXZ==Vector3.zero) //회전 방향이 더 이상 필요 없는 경우 
        {
            return;
        }

        //회전
        self.rotation = Quaternion.LookRotation(dirXZ);
    }

}
