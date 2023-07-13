using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Element : Block
{
    public int orderElement;
    public int damage;

    public void Start()
    {
        DOTween.Sequence()
            .Append(transform.DOScale(0.1f, 0.01f))
            .Append(transform.DOScale(1.35f, 0.75f))
            .AppendInterval(0.25f)
            .Append(transform.DOScale(1, 0.75f))
            .AppendCallback(SpawnEnd)
            .SetLink(gameObject);
    }

    private void SpawnEnd()
    {

    }

    public override void Destroy()
    {
        gameObject.layer = 0;
        var seqence = DOTween.Sequence();
        seqence.Insert(0, this.transform.DOScale(0, 0.25f)).SetLink(gameObject);

        seqence.OnComplete(() =>
        {
            base.Destroy();
        });
    }
}
