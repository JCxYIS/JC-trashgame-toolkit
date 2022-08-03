using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JC.TrashGameToolkit.Sample
{
    // ripped code from my another project (freshweb20
    public class PlayerController : MonoBehaviour
    {
        private const float PLAYER_SPEED =  20.0f;
        private const float RUN_SPEED_MULTUPLE = 1.75f; // RUN SPED=7
        private const float DASH_SPEED_MULTIPLE = 4.0f;
        private const float DASH_LASTING_TIME = 0.3f;

        private Rigidbody rgbd;
        private Vector3 inputVelocity;
        private bool groundedPlayer;
        private float dash_lastingTime = 0f;

        public static float SpeedBoost = 1f; // basis  1

        void Start()
        {
            rgbd = GetComponent<Rigidbody>();        
        }

        void Update()
        {       
            // xz movement
            inputVelocity = Vector3.zero;
            inputVelocity.x = Input.GetAxis("Horizontal") * 0.5f;

            float axisz = Input.GetAxis("Vertical");
            inputVelocity.z = 1f + (axisz > 0 ? axisz * 0.7f : axisz * 0.2f);

            if (inputVelocity != Vector3.zero)
                transform.forward = inputVelocity;

            // if(Input.GetButton("Run"))
            //     inputVelocity *= RUN_SPEED_MULTUPLE;     
            // if(Input.GetButtonDown("Dash"))
            // {
            //     inputVelocity *= DASH_SPEED_MULTIPLE;
            //     dash_lastingTime = DASH_LASTING_TIME;
            // }
        }
        void FixedUpdate()
        {
            // 
            // if(MainGameManager.isPaused)
            //     return;


            // is dashing
            dash_lastingTime -= Time.fixedDeltaTime;
            if(dash_lastingTime > 0)
                inputVelocity *= DASH_SPEED_MULTIPLE;
            
            inputVelocity *= SpeedBoost;

            // rgbd velocity: only y
            float vy = rgbd.velocity.y;
            rgbd.velocity = Vector3.up * vy;
            

            rgbd.MovePosition(rgbd.position + inputVelocity * PLAYER_SPEED * Time.fixedDeltaTime);
        }
    }
}
