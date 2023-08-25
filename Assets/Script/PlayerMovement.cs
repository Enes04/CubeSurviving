using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float dastCoolDown , currentDashTime;
    public Skills currentSkill;
    private CharacterController controller;
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    public float playerSpeed = 2.0f;
    private float jumpHeight = 5.0f;
    private float gravityValue = -29.81f;
    private GameObject character;
    public LayerMask layerMask;
    private Camera camera;
    private GameManager _gameManager;
    private WaitSkillTime currentSkillTimeCoolDown;
    private bool dash;
    private void Awake()
    {
      
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        character = transform.GetChild(0).gameObject;
        controller = gameObject.GetComponent<CharacterController>();
        _gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < _gameManager.waitSkillTimes.Length; i++)
        {
            if (currentSkill == _gameManager.waitSkillTimes[i].mySkill)
                currentSkillTimeCoolDown = _gameManager.waitSkillTimes[i];
        }
    }

    public void MovementPlayer()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y =gravityValue * Time.deltaTime;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * (Time.deltaTime * playerSpeed));

        if (move != Vector3.zero)
        {
            //gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void RotatePlayer()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            //character.transform.LookAt(hit.point);

            var lookPos = hit.point - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, rotation, Time.deltaTime * 10);
        }
    }
 
    void Update()
    {
        if (!dash)
        {
            MovementPlayer();
            RotatePlayer();
        }
       
        currentDashTime += Time.deltaTime;
        if (currentDashTime > dastCoolDown)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                currentSkillTimeCoolDown.StartCoolDown(dastCoolDown);
                currentDashTime = 0;
                DashPlayer();
                dash = true;
            }
        }
    }

    public void DashPlayer()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.DOMove(transform.position+(move * 50), .5f).SetEase(Ease.Linear).OnComplete((() => dash = false));
    }
}