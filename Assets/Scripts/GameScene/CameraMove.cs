using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //摄像机看向的目标
    public Transform target;
    //摄像机相对目标 的偏移位置
    public Vector3 offsetPos;

    //看向位置的偏移
    public float bodyHeight;

    //移动和选择速度
    public float moveSpeed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target==null)
        {
            return;
        }

        //向角色后面偏移z坐标
        Vector3 targetPos = target.position + target.forward * offsetPos.z;
        //向上偏移
        targetPos += Vector3.up * offsetPos.y;
        //左右偏移
        targetPos += target.right * offsetPos.x;
        //插值运算 让摄像机平滑移动
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed*Time.deltaTime);


        //旋转计算
        Quaternion targetRotation = Quaternion.LookRotation((target.position + Vector3.up*bodyHeight)- transform.position);
        //Slerp旋转让摄像机平滑转动
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


    }

    /// <summary>
    /// 设置摄像机看向的对象
    /// </summary>
    /// <param name="player"></param>
    public void SetTarget(Transform player)
    {
        target = player;
    }
}
