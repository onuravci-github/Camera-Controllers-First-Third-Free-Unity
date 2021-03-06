using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    class CameraState
    {
        public Vector3 eulerAngels;
        public Vector3 position;

        //Rotation
        public void RotationLerp(Vector2 target,float lerpPct){
            eulerAngels.x = (Mathf.Lerp(eulerAngels.x,target.x ,lerpPct) ) ;
            eulerAngels.y = (Mathf.Lerp(eulerAngels.y,target.y ,lerpPct)  ) ;
        }
        public void updateRotation(Transform t){
            t.eulerAngles = new Vector3(eulerAngels.x,eulerAngels.y,0); 
        }
        //Position
        public void PositionLerp(Vector3 target,float lerpPct){
            position.x = (Mathf.Lerp(position.x,target.x ,lerpPct)) ;
            position.y = (Mathf.Lerp(position.y,target.y ,lerpPct)) ;
            position.z = (Mathf.Lerp(position.z,target.z ,lerpPct)) ;
        }
        public void updatePosition(Transform t){
            t.position = new Vector3(position.x,position.y,position.z); 
        }

    }

    CameraState cameraState = new CameraState();

    //Rotation
    Vector2 nowMousePosition;
    Vector2 targetRotation;
    [Range(0.001f, 1f)]public float rotationLerpTime = 0.01f;
    public AnimationCurve mouseSensCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    //Position
    Vector3 nowPosition;
    [Range(0, 50)]public float speed;
    [Range(0.001f, 1f)]public float positionLerpTime = 0.2f;


    public Slider[] sliders;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = this.transform.eulerAngles;
        nowPosition = transform.position;
        cameraState.position = transform.position;
        cameraState.eulerAngels = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        SliderUpdate();

        if(Input.GetKey(KeyCode.LeftShift)){
            speed = speed*2;
        }

        if (Input.GetMouseButton(1)){
            nowMousePosition = new Vector2(Input.GetAxis("Mouse Y")*-1, Input.GetAxis("Mouse X"));
            var sensitivityValue = mouseSensCurve.Evaluate(nowMousePosition.magnitude);
            targetRotation.x += nowMousePosition.x * sensitivityValue ;
            targetRotation.y += nowMousePosition.y * sensitivityValue ;
        }

        // Excell graph exponential function
        var rotationLerpValue = 0.106f * Mathf.Pow(rotationLerpTime,-0.961f);
        var positionLerpValue = 0.106f * Mathf.Pow(positionLerpTime,-0.961f);

        cameraState.RotationLerp(targetRotation,rotationLerpValue);
        cameraState.updateRotation(transform);

        nowPosition += MovementInput()*speed*Time.deltaTime;
        cameraState.PositionLerp(nowPosition,positionLerpValue);
        cameraState.updatePosition(transform);
    }
    

    public Vector3 MovementInput(){
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += Vector3.up;
        }
        return Quaternion.Euler(targetRotation.x, targetRotation.y, 0f) * direction;
    }

    public void SliderUpdate(){
        rotationLerpTime = sliders[0].value;
        positionLerpTime = sliders[1].value;
        speed = sliders[2].value*5;
    }
}
