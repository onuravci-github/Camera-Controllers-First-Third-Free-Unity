﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereMoveControl : MonoBehaviour
{
    class MoveCube
    {
        public float moveX;
        public float moveZ;
        public void LerpMove(Vector3 target,float lerpPct,Transform transform, Vector3 sphereRotation){
            moveX = (Mathf.Lerp(moveX,target.x,lerpPct)) ;
            moveZ = (Mathf.Lerp(moveZ,target.z,lerpPct)) ;
            sphereRotation.x = Mathf.Lerp(transform.localEulerAngles.x,sphereRotation.x,lerpPct);
            sphereRotation.y = Mathf.Lerp(transform.localEulerAngles.y,sphereRotation.z,lerpPct);
        }
        public void updateMove(CharacterController controller,Transform transform, Vector3 sphereRotation){
            Vector3 move = new Vector3(moveX,0,moveZ);
            controller.Move(move);
            transform.localEulerAngles = sphereRotation;
        }
    }
    MoveCube moveCube = new MoveCube();

    public Transform sphereTransform;
    public Vector3 sphereRotation;
    public float sphereRadius;


    private CharacterController controller;
    private Vector3 velocity;
    public float gravity = -9.81f;

    public bool isJump = false;
    public bool doubleJump = true;


    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;
    bool isGround;

    public Slider speedSlider;
    public Slider moveLerpSlider;
    public Slider gravityFactorSlider;
    public Toggle doubleJumpToggle;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        controller = this.GetComponent<CharacterController>();
        sphereRadius = controller.radius*this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
       int speed = (int)speedSlider.value;

       if(Input.GetKey(KeyCode.LeftShift)){
            speed = speed*2;
       }

       isGround = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
       if(isGround && velocity.y <0){
           velocity.y = -2f;
           doubleJump = true;
           isJump = false;
       }
       
       if(Input.GetKeyDown(KeyCode.Space) && isGround){
           velocity.y = 10f;
           isJump = true;
       }
       else if(Input.GetKeyDown(KeyCode.Space) && doubleJump && doubleJumpToggle.isOn){
           velocity.y = 10f;
           doubleJump = false;
       }

       if(Input.GetKeyDown(KeyCode.F3)){
           doubleJumpToggle.isOn = !doubleJumpToggle.isOn;
       }


        if(!isJump || isJump){
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            // PI*r distance => 180 Degrees Rotate
            Vector3 move = SphereCameraControl.rotate.forward*z*speed*Mathf.PI*sphereRadius*Time.deltaTime;
            sphereRotation += new Vector3(z*speed*180/Mathf.PI/sphereRadius*Time.deltaTime,0,0);

            var rotationLerpValue = 0.106f * Mathf.Pow(moveLerpSlider.value,-0.961f);
            moveCube.LerpMove(move,rotationLerpValue,sphereTransform,sphereRotation);
        }
       
       moveCube.updateMove(controller,sphereTransform,sphereRotation);
       
       velocity.y += gravity*gravityFactorSlider.value*Time.deltaTime;
       controller.Move(velocity*Time.deltaTime);
    }
}
