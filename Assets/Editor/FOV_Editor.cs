using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//Editor 클래스는 인스펙터나 윈도우 화면 등을 자유롭게 구성하거나 확장할 수 있도록 하기 위한 클래스.
// 쉽게 말해 사용자가 커스텀한 제작툴 제작에 사용됨

[CustomEditor(typeof(BossFOV))]    //EnemyFOV 스크립트를 보조하는 커스텀 에디터라고 명시해줌

public class FOV_Editor : Editor
{
    private void OnSceneGUI()
    {
        BossFOV fov = (BossFOV)target;    //에디터가 보조할 대상을 지정. EnemyFOV 클래스를 참조함

        //원주 위의 시작점 좌표를 계산 (시야각의 1/2)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        Handles.color = Color.white;    //원주의 색상을 흰색으로 지정

        //외곽선만 존재하는 원을 그려줌
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange); //(원점 좌표, 노멀 벡터, 원의 반지름)

        //부채꼴(시야각) 표현
        Handles.color = new Color(1, 1, 1, 0.2f);

        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.viewAngle, fov.viewRange);
                          //(원점 좌표, 노멀 벡터, 부채꼴 시작 좌표(각도), 부채꼴 각도, 부채꼴 반지름);

        //시야각 라벨링
        Handles.Label(fov.transform.position + fov.transform.forward * 2f, fov.viewAngle.ToString());
        
    }
}
