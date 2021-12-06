using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    
    
    [SerializeField] 
    bool isPlayerUnit;

    public Pikamon Pikamon {get; set;}

    private Image image;
    private Vector3 originalPos;
    private Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(Pikamon pikamon)
    {
        Pikamon = pikamon;
        if (isPlayerUnit)
            image.sprite = Pikamon.Base.BackSprite;
        else
            image.sprite = Pikamon.Base.FrontSprite;

        transform.localScale = new Vector3(1, 1, 1);
        image.color = originalColor;
        PlayerEnterAnimation();
    }

    public void PlayerEnterAnimation()
    {
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(+500f,originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x,1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f,0.25f));
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f,0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray,0.1f));
        sequence.Append(image.DOColor(originalColor,0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f,0.5f));
    }

    public IEnumerator PlayCaptureAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0, 0.5f));
        //sequence.Join(transform.DOMoveY(originalPos.y - 1f, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(0.3f, 0.3f, 1f), 0.5f));
        yield return sequence.WaitForCompletion();
    }
    
    public IEnumerator PlayBreakOutAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(1, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
        yield return sequence.WaitForCompletion();
    }
}
