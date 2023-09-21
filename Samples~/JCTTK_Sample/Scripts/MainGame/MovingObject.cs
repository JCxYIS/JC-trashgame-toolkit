using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JC.TrashGameToolkit.Sample
{
    public class MovingObject : MonoBehaviour
    {

        // Update is called once per frame
        protected virtual void Update()
        {
            if(transform.position.y < -1)
            {
                KillSelf();
            }
        }

        protected virtual void KillSelf()
        {
            FindObjectOfType<Player>().KillAlly(gameObject);
        }
    }
}
