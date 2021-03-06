using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMovement : MonoBehaviour
{
    class MoveCube
    {
        public float moveX;
        public float moveZ;
        public void LerpMove(Vector3 target,float lerpPct){
            moveX = (Mathf.Lerp(moveX,target.x,lerpPct)) ;
            moveZ = (Mathf.Lerp(moveZ,target.z,lerpPct)) ;
        }
        public void updateMove(CharacterController controller){
            Vector3 move = new Vector3(moveX,0,moveZ);
            controller.Move(move);
        }
    }
    
    MoveCube moveCube = new MoveCube();

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
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
       int speed = (int)speedSlider.value;

       if(Input.GetKey(KeyCode.LeftShift) && !isJump){
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

       if(!isJump){
           float x = Input.GetAxis("Horizontal");
           float z = Input.GetAxis("Vertical");
           Vector3 move = transform.right*x*speed*Time.deltaTime + transform.forward*z*speed*Time.deltaTime;
           var rotationLerpValue = 0.106f * Mathf.Pow(moveLerpSlider.value,-0.961f);
           moveCube.LerpMove(move,rotationLerpValue);
       }
       
       moveCube.updateMove(controller);
       
       velocity.y += gravity*gravityFactorSlider.value*Time.deltaTime;
       controller.Move(velocity*Time.deltaTime);
    }
}
