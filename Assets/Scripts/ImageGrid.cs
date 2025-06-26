using System.Collections.Generic;
using UnityEngine;

public class ImageGrid : MonoBehaviour
{
    private SpriteRenderer _spr;

    public ImageStatus _status;

    [Header("이미지 설정")]
    public Color[] colors;

    private void Start()
    {
        TryGetComponent(out _spr);
    }

    public void SetImage(ImageStatus index)
    {
        if (_spr != null)
        {
            _spr.color = colors[(int)index];
            _status = index;
        }
    }
}