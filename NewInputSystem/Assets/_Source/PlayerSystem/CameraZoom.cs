using UnityEngine;

public class CameraZoom
{
    private Camera _camera;

    public CameraZoom()
    {
        _camera = Camera.main;
    }

    public void Zoom(float zoomDirection, float zoomSpeed)
    {
        _camera.fieldOfView -= zoomDirection * zoomSpeed * Time.deltaTime;
    }
}
