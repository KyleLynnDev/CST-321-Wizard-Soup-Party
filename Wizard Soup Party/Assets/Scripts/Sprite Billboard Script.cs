using System;
using Unity.Cinemachine;
using UnityEngine;


public class SpriteBillboardScript : MonoBehaviour
{
    public Transform billboardTarget; 
    //public CinemachineCamera cam;
    private SpriteRenderer SR;
    
    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (billboardTarget == null) return; // Exit if no target is assigned
        // Get the position of the target behind the camera
        Vector3 targetPosition = billboardTarget.position;
        
        //Vector3 targetPosition =  cam.transform.position;
        targetPosition.z = targetPosition.z - 0.1f;
        
        Vector3 direction = (transform.position - targetPosition).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            transform.forward = direction.normalized;
        }
        
        
        //transform.rotation = Quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y, 0f);
    }
}
