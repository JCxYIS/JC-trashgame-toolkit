using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JC.TrashGameToolkit.Sample
{
    public class CameraMovement : MonoBehaviour
    {
        Player _player;
        float _zPosDelta;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            _player = FindObjectOfType<Player>();
            _zPosDelta = transform.position.z - _player.transform.position.z;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if(!_player)
                return;
            float newZ = _player.transform.position.z + _zPosDelta;
            // if(newZ < transform.position.z)
            //     newZ = transform.position.z;
            transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        }
    }
}
