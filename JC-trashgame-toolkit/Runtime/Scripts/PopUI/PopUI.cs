using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Pop up Panel
/// You can inherit this class to create your own pop up panel. 
/// Notice in case that you need to use Awake() or Update(), use protected override void, and remember to call base.Awake()
/// </summary>
public class PopUI : MonoBehaviour
{
    [Header("Pop UI Settings")]

    [SerializeField, Tooltip("")] 
    protected bool _showOnAwake = false;

    [SerializeField, Tooltip("動畫長度")] 
    protected float _animDuration = 0.48763f;  

    [SerializeField, Tooltip("Ignore timescale?")]
    protected bool _ignoreTimescale = false;

    [SerializeField, Tooltip("(Optional) 後面的陰影，會自動加上觸碰以關閉的按紐")]
    protected Image _bgImage;

    [SerializeField, Tooltip("此 Panel 的 container")] 
    protected Transform _mainContainer;

    // [SerializeField] 
    // protected bool _playSfx = true;
    
    [SerializeField, Tooltip("Destroy when Hide() is called")] 
    protected bool _destroyOnHide = true;

    [SerializeField, Tooltip("Hide when esc is hit")] 
    protected bool _hideOnEscClicked = true;

    /* -------------------------------------------------------------------------- */
    // Events
    protected UnityAction OnShowAnimFinished;

    /* -------------------------------------------------------------------------- */
    // Runtime params
    Color bgImage_initColor;
    bool isShowAnimFinished; // to block "hide command when show animation is playing"

    /* -------------------------------------------------------------------------- */


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        _mainContainer.transform.localScale = Vector3.zero;        

        // add hide function to bgImage
        if(_bgImage)
        {
            bgImage_initColor = _bgImage.color;
            _bgImage.color = Color.clear;
            _bgImage.raycastTarget = true;

            Button hideBtn = _bgImage.GetComponent<Button>();
            if(!hideBtn)
            {
                hideBtn = _bgImage.gameObject.AddComponent<Button>();
                hideBtn.transition = Selectable.Transition.None;
            }
            hideBtn.onClick.AddListener(()=>Hide());
        }        

        // gameObject.SetActive(false);
        if(_showOnAwake)
            Show();

        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while(true)
        {
            if(_hideOnEscClicked && Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }

            yield return null;
        }
    }


    public virtual void Show()
    {
        gameObject.SetActive(true);
        isShowAnimFinished = false;
        _mainContainer.transform
            .DOScale(Vector3.one, _animDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(_ignoreTimescale)
            .OnComplete(()=>
            {
                isShowAnimFinished = true;
                if(OnShowAnimFinished != null)
                    OnShowAnimFinished();
            });
        
        if(_bgImage)
        {
            _bgImage.DOColor(bgImage_initColor, _animDuration);
        }

        // lower music vol
        // if(MusicControllerJc.HasInstance)
        // {
        //     MusicControllerJc.Instance.masterVolume = 0.6f; 
        //     InputManager.Instance.ChangeState(InputManager.State.NoControl);
        //     if(_playSfx)
        //         MusicControllerJc.Instance.PlaySfx(MusicControllerJc.SfxType.Confirm);
        // }

    }

    public virtual void Hide()
    {
        // block "hide command when show animation is playing"
        if(!isShowAnimFinished)
        {
            Debug.Log("[PopUI] Too fast to hide (Still playing show animation)");
            _mainContainer.DOKill();
            _bgImage.DOKill();
            isShowAnimFinished = true;
        }

         _mainContainer.transform
            .DOScale(Vector3.zero, _animDuration/2f)
            .SetEase(Ease.InCubic)
            .SetUpdate(_ignoreTimescale)
            .onComplete += ()=>{
                // Destroy on hide
                if(_destroyOnHide)
                    Destroy(gameObject);
                gameObject.SetActive(false);
            };
        
        if(_bgImage)
        {
            _bgImage.DOColor(Color.clear, _animDuration/2f);
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