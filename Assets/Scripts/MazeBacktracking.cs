using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImageStatus
{
    Road = 0,
    Wall = 1
}

public class MazeBacktracking : MonoBehaviour
{
    public List<List<ImageStatus>> mazes = new List<List<ImageStatus>>();

    [Header("미로 크기 (홀수로 설정)")]
    public int maxWidth = 21;
    public int maxHeight = 21;

    [Header("이미지 그룹")]
    [SerializeField] private List<GridGroup> gridGroup;

    private bool[,] visited;

    private void Start()
    {
        StartCoroutine(GenerateMaze());
    }

    private IEnumerator GenerateMaze()
    {
        mazes.Clear();
        visited = new bool[maxHeight, maxWidth];

        for (int y = 0; y < maxHeight; y++)
        {
            List<ImageStatus> row = new List<ImageStatus>();
            for (int x = 0; x < maxWidth; x++)
            {
                row.Add(ImageStatus.Wall);
            }
            mazes.Add(row);
        }

        int startX = Random.Range(0, maxWidth / 2) * 2 + 1;
        int startY = Random.Range(0, maxHeight / 2) * 2 + 1;

        yield return StartCoroutine(Backtrack(startX, startY));
    }

    private IEnumerator Backtrack(int x, int y)
    {
        visited[y, x] = true;
        mazes[y][x] = ImageStatus.Road;
        gridGroup[y].SetColor(x, ImageStatus.Road);

        yield return new WaitForSeconds(0.25f);

        var directions = new List<(int dx, int dy)>
        {
            (-2, 0), (2, 0), (0, -2), (0, 2)
        };

        // 방향 섞기
        for (int i = 0; i < directions.Count; i++)
        {
            int rnd = Random.Range(i, directions.Count);
            (directions[i], directions[rnd]) = (directions[rnd], directions[i]);
        }

        foreach (var (dx, dy) in directions)
        {
            int nx = x + dx;
            int ny = y + dy;

            if (nx > 0 && nx < maxWidth && ny > 0 && ny < maxHeight && !visited[ny, nx])
            {
                int wallX = x + dx / 2;
                int wallY = y + dy / 2;

                mazes[wallY][wallX] = ImageStatus.Road;
                gridGroup[wallY].SetColor(wallX, ImageStatus.Road);

                yield return new WaitForSeconds(0.25f);

                ShowMaze();

                yield return StartCoroutine(Backtrack(nx, ny));
            }
        }
    }

    private void ShowMaze()
    {
        for (int y = 0; y < maxHeight; y++)
        {
            for (int x = 0; x < maxWidth; x++)
            {
                gridGroup[y].SetColor(x, mazes[y][x]);
            }
        }
    }

}
