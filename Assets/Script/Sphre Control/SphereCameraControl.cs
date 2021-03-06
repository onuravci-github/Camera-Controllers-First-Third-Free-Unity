using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereCameraControl : MonoBehaviour
{
    //Rotate Update for Camera 
    class RotateCamera
    {
        public float rotateX;
        public float rotateY;
        public void LerpCamera(Vector2 target,float lerpPct,int cameraVerticalLimit){
            rotateX = Mathf.Clamp((Mathf.Lerp(rotateX,target.x,lerpPct) ),-cameraVerticalLimit,cameraVerticalLimit) ;
            rotateY = (Mathf.Lerp(rotateY,target.y,lerpPct) ) ;
        }
        public void updateRotationCamera(Transform cam){
            cam.localEulerAngles = new Vector3(rotateX,rotateY,0);
        }
    }

    public static Transform rotate;
    RotateCamera rotateCamera= new RotateCamera();
        
    [Range(30,90)]public int cameraVerticalLimit = 70;
    public AnimationCurve mouseSensCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    
    public Vector2 targetRotation;

    public Slider rotationLerpSlider;
    public Slider rotateSensitivitySlider;
    public Toggle freeCameraToggle;

    // Start is called before the first frame update
    void Start()
    {
        rotate = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        //İnvisible Cursor when right mouse button  
        if (Input.GetMouseButtonDown(1) && !freeCameraToggle.isOn){
            Cursor.lockState = 
            CursorLockMode.Locked;
        }
        // Unlock and show cursor when right mouse button
        if (Input.GetMouseButtonUp(1) && !freeCameraToggle.isOn){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if(freeCameraToggle.isOn){
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Input Key for Mouse 1 Rotate Control && Free Rotation Control
        if(Input.GetKeyDown(KeyCode.F2)){
            freeCameraToggle.isOn = !freeCameraToggle.isOn;
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
        }

        float x = Input.GetAxis("Horizontal");
        // Mouse 1 click rotation || Free rotation
        if (Input.GetMouseButton(1) || freeCameraToggle.isOn || x != 0)
        {
            Vector2 mousePosition = new Vector2(Input.GetAxis("Mouse Y")*-1, Input.GetAxis("Mouse X"));
            var sensitivityValue = mouseSensCurve.Evaluate(mousePosition.magnitude);

            if(x == 0){targetRotation.y += (mousePosition.y * rotateSensitivitySlider.value) * sensitivityValue ; }
            targetRotation.y += x*rotateSensitivitySlider.value;
            //Camera Cube Vertical Limit if else
            if(rotateCamera.rotateX < cameraVerticalLimit && rotateCamera.rotateX > -cameraVerticalLimit){
                targetRotation.x += (mousePosition.x * rotateSensitivitySlider.value) * sensitivityValue ;
            }
            else{
                targetRotation.x -= Mathf.Sign(targetRotation.x);
            }
        }
        // Excell graph exponential function
        var rotationLerpValue = 0.106f * Mathf.Pow(rotationLerpSlider.value,-0.961f);

        //Update Rotation Cube and Camera
        rotateCamera.LerpCamera(targetRotation,rotationLerpValue,cameraVerticalLimit);
        rotateCamera.updateRotationCamera(this.transform);
    }
}
