using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace JC.TrashGameToolkit.Sample
{
    public class StageController : MonoBehaviour
    {
        public GameObject _stagePrefab;
        public Vector3 _firstStagePosition;
        public float _stageLength;

        private Queue<GameObject> _stages = new Queue<GameObject>();
        int _stageNo = 0;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            for(int i = 0; i < 5; i++)
            {
                CreateNextStage();
            }
        }

        public void CreateNextStage()
        {
            _stageNo++;

            // Create Stage
            GameObject stage = _stages.Count > 5 ? _stages.Dequeue() : Instantiate(_stagePrefab, transform.position, Quaternion.identity);
            stage.transform.position = _firstStagePosition + Vector3.forward * _stageLength * _stageNo;
            _stages.Enqueue(stage);

            // Set walls
            var walls = stage.GetComponentsInChildren<Wall>();
            foreach(var wall in walls)
            {
                wall.stageController = this;
                wall.behavior = Random.value > 0.5f ? "+"+Random.Range(1, 5) : "-"+Random.Range(1, 5);
            }
        }
    }
}