using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// Pop up Panel
/// You can inherit this class to create your own pop up panel. 
/// Notice in case that you need to use Awake() or Update(), use protected override void, and remember to call base.Awake()
/// </summary>
public class PopUI : MonoBehaviour
{
    [Header("Pop UI Settings")]
    public bool ShowOnAwake = false;
    public float AnimDuration = 0.48763f;    
    [SerializeField, Tooltip("後面的陰影，會自動加上觸碰以關閉的按紐")] Image _bgImage;
    [SerializeField, Tooltip("此 Panel 的 container")] Transform _mainContainer;
    // [SerializeField] protected bool _playSfx = true;
    [SerializeField] protected bool _destroyOnHide = true;
    
    Color bgImage_initColor;
    float startShowingTime; // to block "hide command when show animation is playing"


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        _mainContainer.transform.localScale = Vector3.zero;
        bgImage_initColor = _bgImage.color;
        _bgImage.color = Color.clear;

        // add hide function to bgImage
        if(_bgImage)
        {
            Button hideBtn = _bgImage.GetComponent<Button>();
            if(!hideBtn)
            {
                hideBtn = _bgImage.gameObject.AddComponent<Button>();
                hideBtn.transition = Selectable.Transition.None;
            }
            hideBtn.onClick.AddListener(()=>Hide());
        }        

        // gameObject.SetActive(false);

        if(ShowOnAwake)
            Show();
    }

    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        _mainContainer.transform.DOScale(Vector3.one, AnimDuration).SetEase(Ease.OutBack);
        
        if(_bgImage)
        {
            _bgImage.DOColor(bgImage_initColor, AnimDuration);
        }

        // lower music vol
        // if(MusicControllerJc.HasInstance)
        // {
        //     MusicControllerJc.Instance.masterVolume = 0.6f; 
        //     InputManager.Instance.ChangeState(InputManager.State.NoControl);
        //     if(_playSfx)
        //         MusicControllerJc.Instance.PlaySfx(MusicControllerJc.SfxType.Confirm);
        // }

        startShowingTime = Time.time;
    }

    public virtual void Hide()
    {
        // block "hide command when show animation is playing"
        if(Time.time - startShowingTime < AnimDuration)
        {
            Debug.Log("[PopUI] 還在跑彈出動畫，太快了");
            _mainContainer.DOKill();
        }

         _mainContainer.transform.DOScale(Vector3.zero, AnimDuration/2f).SetEase(Ease.InCubic)
         .onComplete += ()=>{
            // Destroy on hide
            if(_destroyOnHide)
                Destroy(gameObject);
            gameObject.SetActive(false);
        };
        
        if(_bgImage)
        {
            _bgImage.DOColor(Color.clear, AnimDuration/2f);
        }

        // music and input
        // if(MusicControllerJc.HasInstance)
        // {
        //     MusicControllerJc.Instance.masterVolume = 1f;
        //     InputManager.Instance.ChangeState(InputManager.State.CharacterControl);
        //     if(_playSfx)
        //         MusicControllerJc.Instance.PlaySfx(MusicControllerJc.SfxType.Cancel);
        // }
    }
}