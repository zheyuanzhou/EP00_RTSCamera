using UnityEngine;

public class CameraController01 : MonoBehaviour
{
    private Vector3 moveInput;//接收键盘的输入量
    [SerializeField] private float panSpeed;//相机平移的速度

    [SerializeField] private float scrollSpeed;//鼠标滚动的速度

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        //我们其实动态改变的是Main Camera的Trans组件的Pos
        Vector3 pos = transform.position;

        //moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));//性能？
        moveInput.Set(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Vector2 mousePos = Input.mousePosition;
        if (mousePos.x > Screen.width * 0.9f && mousePos.x < Screen.width)
            moveInput.x = 1;
        if (mousePos.x < Screen.width * 0.1f && mousePos.x > 0)
            moveInput.x = -1;
        if (mousePos.y > Screen.height * 0.9 && mousePos.y < Screen.height)
            moveInput.z = 1;
        if (mousePos.y < Screen.height * 0.1 && mousePos.y > 0)
            moveInput.z = -1;

        //pos += moveInput * panSpeed * Time.deltaTime;//ERROR
        //pos += moveInput.normalized * panSpeed * Time.deltaTime;//匀速运动，归一化/向量化

        pos.x += moveInput.normalized.x * panSpeed * Time.deltaTime;
        pos.y += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;//Y轴滚轮输入量
        pos.z += moveInput.normalized.z * panSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -10, 10);
        pos.y = Mathf.Clamp(pos.y, 5, 30);
        pos.z = Mathf.Clamp(pos.z, -25, 5);//根据自己的地图范围进行调整，可以设置为变量，方便嘛！

        transform.position = pos;
    }
}
