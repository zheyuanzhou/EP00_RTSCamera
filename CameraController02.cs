using System;
using UnityEngine;

public class CameraController02 : MonoBehaviour
{
    private float panSpeed;
    [SerializeField] private float moveTime;//缓冲时间，用于之后的Vector3.Lerp和Quaternion.Lerp方法/函数
    [SerializeField] private float normalSpeed, fastSpeed;

    private Vector3 newPos;
    private Quaternion newRotation;
    [SerializeField] private float rotationAmount;//旋转的程度

    private Transform cameraTrans;//子物体嘛～主相机Trans，要改YZ数值
    [SerializeField] private Vector3 zoomAmount;//要改YZ数值，设置zoomAmount结构体中YZ的数值
    private Vector3 newZoom;

    private Vector3 dragStartPos, dragCurrentPos;//鼠标拖拽的起始点，和鼠标拖拽的当前位置
    private Vector3 rotateStart, rotateCurrent;//鼠标初始位置和当前位置，用来计算相机旋转角度

    private void Start()
    {
        newPos = transform.position;
        newRotation = transform.rotation;

        cameraTrans = transform.GetChild(0);
        newZoom = cameraTrans.localPosition;
    }

    private void Update()
    {
        HandleMovementInput();//通过键盘控制相机
        HandleMouseInput();//通过鼠标控制相机
    }

    private void HandleMouseInput()
    {
        if(Input.GetMouseButtonDown(1))//鼠标按下一瞬间！
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if(plane.Raycast(ray, out distance))//out输出参数，一般方法返回一个数值，out则返回return和out数值，2个结果
            {
                dragStartPos = ray.GetPoint(distance);
            }
        }

        if (Input.GetMouseButton(1))//鼠标按着（当前）
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))//out输出参数，一般方法返回一个数值，out则返回return和out数值，2个结果
            {
                dragCurrentPos = ray.GetPoint(distance);

                Vector3 difference = dragStartPos - dragCurrentPos;//大家可以试试反过来写，效果雷同只是方向相反
                newPos = transform.position + difference;
            }
        }

        newZoom += Input.mouseScrollDelta.y * zoomAmount;

        if (Input.GetMouseButtonDown(2))
            rotateStart = Input.mousePosition;
        if(Input.GetMouseButton(2))
        {
            rotateCurrent = Input.mousePosition;
            Vector3 difference = rotateStart - rotateCurrent;

            rotateStart = rotateCurrent;//赋值最新的鼠标位置
            newRotation *= Quaternion.Euler(Vector3.up * -difference.x / 20);//水平方向触发旋转
            //newRotation *= Quaternion.Euler(Vector3.up * -difference.y / 20);//垂直方向
        }
    }

    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            panSpeed = fastSpeed;
        else
            panSpeed = normalSpeed;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            newPos += transform.forward * panSpeed * Time.deltaTime;//相机平移向上
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            newPos -= transform.forward * panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            newPos += transform.right * panSpeed * Time.deltaTime;//相机平移向右
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            newPos -= transform.right * panSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);//Q：逆时针
        if (Input.GetKey(KeyCode.E))
            newRotation *= Quaternion.Euler(Vector3.down * rotationAmount);//(0,-1,0)顺时针

        if (Input.GetKey(KeyCode.R))
            newZoom += zoomAmount;//放大功能：Y越来越小，Z越来越大
        if (Input.GetKey(KeyCode.F))
            newZoom -= zoomAmount;//缩小：Y越来越大，Z越来越小

        //transform.position = newPos;//AxisRaw / Axis
        //Lerp方法：当前位置，目标位置，最大距离：速度 * 时间 =>从当前位置，到目标位置，需要多少时间到达
        transform.position = Vector3.Lerp(transform.position, newPos, moveTime * Time.deltaTime);

        //transform.rotation = newRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, moveTime * Time.deltaTime);

        cameraTrans.localPosition = Vector3.Lerp(cameraTrans.localPosition, newZoom, moveTime * Time.deltaTime);
    }
}
