using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject cameraMain;
    public GameObject cameraCube;
    public GameObject cameraSphere;

    public GameObject canvasMain;
    public GameObject canvasCube;
    public GameObject canvasSphere;

    public void CameraMainOpen(){
        cameraMain.SetActive(true);
        cameraCube.SetActive(false);
        cameraSphere.SetActive(false);

        canvasMain.SetActive(true);
        canvasCube.SetActive(false);
        canvasSphere.SetActive(false);
    }
    public void CameraCubeOpen(){
        cameraCube.SetActive(true);
        cameraMain.SetActive(false);
        cameraSphere.SetActive(false);
        
        canvasMain.SetActive(false);
        canvasCube.SetActive(true);
        canvasSphere.SetActive(false);
    }
    public void CameraSphereOpen(){ 
        cameraSphere.SetActive(true);
        cameraMain.SetActive(false);
        cameraCube.SetActive(false);
        
        canvasMain.SetActive(false);
        canvasCube.SetActive(false);
        canvasSphere.SetActive(true);
    }
}
