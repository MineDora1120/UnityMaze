using UnityEngine;

public class GridGroup : MonoBehaviour
{
    [Header("�̹��� �׷�")]
    [SerializeField] private ImageGrid[] _imageGrids;

    private void Start()
    {
        _imageGrids = GetComponentsInChildren<ImageGrid>(true);
    }

    public void SetImage(int index, ImageStatus status)
    {
        _imageGrids[index].SetImage(status);
    }
}