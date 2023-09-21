using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace JC.TrashGameToolkit.Sample
{
    public class Wall : MonoBehaviour
    {
        [Header("Settings")]
        public string behavior = "+3";

        [Header("Bindings")]
        [SerializeField] Image gradient;
        [SerializeField] Text text;
        [HideInInspector] public StageController stageController;
        [HideInInspector] public int _stageNo = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {        
            text.text = behavior;
        }

        public void Set(int stageNo, string behavior)
        {
            this._stageNo = stageNo;;
            this.behavior = behavior;
        }

        public void OnTrigger(bool isGoodForPlayer)
        {
            gradient.DOColor(isGoodForPlayer ? Color.green : Color.red, 0.25f).SetLoops(2, LoopType.Yoyo);
            text.transform.DOJump(text.transform.position, 0.8f, 1, 0.5f);
            stageController.CreateNextStage(_stageNo);
        }
    }
}
