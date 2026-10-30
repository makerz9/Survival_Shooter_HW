using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static readonly int moveX = Animator.StringToHash("MoveX"); //!@# 1
    public static readonly int moveY = Animator.StringToHash("MoveY"); //!@# 1
    public float moveSpeed = 5f;

    public float rotateSpeed = 180f; //마우스 회전

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>(); //애니메이터 등록
        playerInput = GetComponent<PlayerInput>(); // 인풋 스크립트 등록
        playerRigidbody = GetComponent<Rigidbody>(); // 물리엔진 등록

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"X: {playerInput.MoveX}, Y: {playerInput.MoveY}");

        playerAnimator.SetFloat(moveY, playerInput.MoveY); //!@# 2
        playerAnimator.SetFloat(moveX, playerInput.MoveX); //!@# 2
    }

    private void FixedUpdate()
    {
        //float h = playerInput.MoveX; //좌우
        //float v = playerInput.MoveY; //상하

        //Vector3 moveDirection = new Vector3(h, 0, h);

        //moveDirection *= moveSpeed;

        Vector3 deltaY = playerInput.MoveY * Vector3.forward * moveSpeed * Time.deltaTime; //!@# 3
        playerRigidbody.MovePosition(playerRigidbody.position + deltaY);

        Vector3 deltaX = playerInput.MoveX * Vector3.right * moveSpeed * Time.deltaTime; //!@# 3
        playerRigidbody.MovePosition(playerRigidbody.position + deltaX);
    }


}
