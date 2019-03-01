using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour {

    private Animator Player_Animor; 
    [System.Serializable]
    public class MoveSetting
    {
        public float ForwarSpeed = 5f;
        public float BackSpeed = 3f;
        public float HorizonSpeed = 4f;
        public float RunValue = 2f;
        public float JumpForce = 50f;
    }
    [System.Serializable]
    public class MouseLook
    {
        public float XSensitive = 2f;
        public float YSensitive = 2f;
    }
    public MoveSetting moveSetting;
    public MouseLook CameraSet;
    private Quaternion m_chaQutation;
    private Quaternion m_CamQutation;
    private CapsuleCollider player;
    private Vector3 curGroundNormal;
    private bool m_isground;
    private float currentSpeed;
    private Camera m_camera;
    private Transform m_camTran;
    private Transform m_chaTran;
    private Rigidbody m_chaRig;
    private bool m_jump = false;
    public AnimationCurve SlopeCurve;
    private int count = 0;
    void Start () {
        Player_Animor = GameObject.Find("Anim").GetComponent<Animator>();
        player = this.GetComponent<CapsuleCollider>();
        m_chaRig = GetComponent<Rigidbody>();
        m_camera = Camera.main;
        m_camTran = m_camera.transform;
        m_chaTran = transform;
        m_CamQutation = m_camTran.rotation;
        m_chaQutation = m_chaTran.rotation;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private float mousespeed = 5f;
    float RotationY = 0f;
    float RotationX = 0f;
    public float minmouseY = -45f;
    public float maxmouseY = 45f;
    
    void Update () {
        Debug.Log(count);
        
        Cursor.visible = false;
        RotationX += m_camTran.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mousespeed;
        RotationY -= Input.GetAxis("Mouse Y") * mousespeed;
        RotationY = Mathf.Clamp(RotationY, minmouseY, maxmouseY);
        this.transform.eulerAngles = new Vector3(0, RotationX, 0);
        m_camTran.transform.eulerAngles = new Vector3(RotationY, RotationX, 0);
        if (Input.GetKey(KeyCode.Space))
        {
            m_jump = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Player_Animor.SetBool("EndWalking", true);
            Player_Animor.SetBool("StartWalking", false);
            Player_Animor.SetBool("Walking", false);
            count = 0;

        }
        if (Input.GetKey(KeyCode.W))
        {
            if (count > 120)
            {
                Debug.Log("in");
                Player_Animor.SetBool("StartWalking", false);
                Player_Animor.SetBool("Walking", true);
                return;
            }
            count++;
            Player_Animor.SetBool("StartWalking", true);
        }
    }

    Quaternion ClampRotation(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1;
        /*给定一个欧拉旋转(X, Y, Z)（即分别绕x轴、y轴和z轴旋转X、Y、Z度），则对应的四元数为x = sin(Y/2)sin(Z/2)cos(X/2)+cos(Y/2)cos(Z/2)sin(X/2)y = sin(Y/2)cos(Z/2)cos(X/2)+cos(Y/2)sin(Z/2)sin(X/2)z = cos(Y/2)sin(Z/2)cos(X/2)-sin(Y/2)cos(Z/2)sin(X/2)w = cos(Y/2)cos(Z/2)cos(X/2)-sin(Y/2)sin(Z/2)sin(X/2)         */
        //得到推导公式[欧拉角x=2*Aan(q.x)]      
        float angle = 2 * Mathf.Rad2Deg * Mathf.Atan(q.x);
        //限制速度     
        angle = Mathf.Clamp(angle,-90f,90f);
        //反推出q的新x的值     
        q.x = Mathf.Tan(Mathf.Deg2Rad * (angle / 2));
        return q;
 
    }
    private void FixedUpdate()
    {
        Moving();
    }
    void Moving()
    {
        checkGround();
        Vector2 input = GetInput();
        CalculateSpeed(input);
        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && m_isground)
        {
            
            Vector3 desireMove = m_camTran.forward * input.y + m_camTran.right * input.x;
            desireMove = Vector3.ProjectOnPlane(desireMove, curGroundNormal).normalized;
            desireMove.x = desireMove.x * currentSpeed;
            desireMove.y = 0;
            desireMove.z = desireMove.z * currentSpeed;
            if (m_chaRig.velocity.sqrMagnitude < currentSpeed * currentSpeed)
            {
                m_chaRig.AddForce(desireMove * SlopeValue(), ForceMode.Impulse);
            }
        }
        if (m_isground)
        {
            m_chaRig.drag = 5f;
            if (m_jump)
            {
                jumpUp();
            }
        }
        m_jump = false;
    }
    void jumpUp()
    {
        m_chaRig.drag = 0;
        m_chaRig.velocity = new Vector3(m_chaRig.velocity.x, 0f, m_chaRig.velocity.z);
        m_chaRig.AddForce(new Vector3(0, moveSetting.JumpForce, 0), ForceMode.Impulse);
    }
    float SlopeValue()
    {
        float angle = Vector3.Angle(curGroundNormal, Vector3.up);
        float value = SlopeCurve.Evaluate(angle);
        return value;
    }
    void CalculateSpeed(Vector2 _input)
    {
        currentSpeed = moveSetting.ForwarSpeed;
        if (Mathf.Abs(_input.x) > float.Epsilon)
        {
            currentSpeed = moveSetting.HorizonSpeed;
        }
        else if (_input.y < 0)
        {
            currentSpeed = moveSetting.BackSpeed;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = moveSetting.RunValue;
        }
    }
    Vector2 GetInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        return input;
    }
    void checkGround()
    {
        RaycastHit hit;
        if(Physics.SphereCast(player.transform.position,player.radius,Vector3.down,out hit, ((player.height / 2 - player.radius) + 1f))){
            curGroundNormal = hit.normal; //hit.normal 表示所碰到平面的法线 需要到时候debug一下
            m_isground = true;
        }
        else
        {
            curGroundNormal = Vector3.up;
            m_isground = false;
        }
    }
}
