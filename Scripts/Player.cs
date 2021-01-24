using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public float lookSensitivity;           // 마우스 민감도
    public float cameraRoationLimit;   // 카메라 전환 한계
    public float curCameraRotX;         // 현재 카메라 Rotation x의 값
    public Camera playerCamera;       // 카메라

    float hor_Axis;
    float ver_Axis;
    float Mstate = 1f;

    bool Rstate;
    bool Jstate;
    bool Sstate;
    bool Jbutton;
    bool Sbutton;
    bool Rbutton;

    Vector3 move;
    Vector3 run;

    Rigidbody player;

    private void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        InputFunc();
        Movement();
        Jump();
        MoveState();
        CameraRotation();
        CharacterRotation();
    }

    void InputFunc()
    {
        hor_Axis = Input.GetAxisRaw("Horizontal");
        ver_Axis = Input.GetAxisRaw("Vertical");
        Jbutton = Input.GetButtonDown("Jump");

        if (Rbutton)
        {
            if ((Input.GetButtonDown("Sit")  || Input.GetButtonDown("Run")) && !Sbutton && !Jstate)
            {
                Rbutton = !Rbutton;
            }
        }
        else
        {
            if (Input.GetButtonDown("Run") && !Sbutton && !Jstate)
                Rbutton = !Rbutton;
        }

        if (Sbutton)
        {
            if ((Input.GetButtonDown("Sit") || Input.GetButtonDown("Run")) && !Rbutton && !Jstate)
            {
                Sbutton = !Sbutton;
            }
        }
        else
        {
            if (Input.GetButtonDown("Sit") && !Rbutton && !Jstate)
                Sbutton = !Sbutton;
        }
    }

    void Movement()
    {
        Vector3 moveHorizontal = transform.right * hor_Axis;
        Vector3 moveVertical = transform.forward * ver_Axis;
        
        move = (moveHorizontal + moveVertical).normalized * speed;

        player.MovePosition(transform.position + move * Mstate *  Time.deltaTime); 
    }

    void Jump()
    {
        if (Jbutton && !Jstate && !Sbutton)
        {
            player.AddForce(Vector3.up * 10, ForceMode.Impulse);
            Jstate = true;
        }
    }

    void MoveState()
    {
        if (Rbutton && !Sbutton)
        {
            Mstate = 3f;
        }
        else if (!Rbutton && Sbutton)
        {
            Mstate = 0.5f;
        }
        else if (!Rbutton && !Sbutton)
        {
            Mstate = 1f;
        }
        else
        {
            Rbutton = false;
            Sbutton = false;
            Mstate = 1f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Jstate = false;
        }
    }

    void CharacterRotation()
    {
        // 좌우 캐릭터 회전
        float yRoation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRoation, 0f) * lookSensitivity;

        player.MoveRotation(player.rotation * Quaternion.Euler(characterRotationY));
    }
    void CameraRotation()
    {
        // 상하 카메라 회전
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRoationX = xRotation * lookSensitivity;

        curCameraRotX -= cameraRoationX;
        curCameraRotX = Mathf.Clamp(curCameraRotX, -cameraRoationLimit, cameraRoationLimit);

        playerCamera.transform.rotation = playerCamera.transform.rotation * Quaternion.Euler(curCameraRotX, 0f, 0f);
    }
}