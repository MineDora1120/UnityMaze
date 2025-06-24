using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ImageStatus
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 3,
    Down = 4,
}

public class MazeBacktracking : MonoBehaviour
{
    public List<List<ImageStatus>> mazes = new List<List<ImageStatus>>();

    private int nowWidth = 0;
    private int nowHeight = 0;

    [Header("길이")]
    public int maxWidth = 5;

    public int maxHeight = 5;

    [Header("이미지 그룹")]
    [SerializeField] private List<GridGroup> gridGroup;

    // 셀 방문 여부를 저장하는 배열
    private bool[,] visited;

    /// <summary>
    /// 시작 시 미로 생성
    /// </summary>
    private void Start()
    {
        GenerateMaze();
    }

    /// <summary>
    /// 미로를 초기화하고 랜덤한 위치에서 백트래킹으로 생성 시작
    /// </summary>
    private void GenerateMaze()
    {
        // 이전 미로 정보 초기화
        mazes.Clear();
        visited = new bool[maxWidth, maxHeight];

        // 모든 셀을 None 상태로 초기화
        for (int y = 0; y < maxHeight; y++)
        {
            List<ImageStatus> row = new List<ImageStatus>();
            for (int x = 0; x < maxWidth; x++)
            {
                row.Add(ImageStatus.None);
            }
            mazes.Add(row);
        }

        // 시작 지점을 무작위로 설정
        nowWidth = Random.Range(0, maxWidth);
        nowHeight = Random.Range(0, maxHeight);

        // 백트래킹 알고리즘 시작
        Backtrack(nowWidth, nowHeight);

        // 생성된 미로를 출력
        ShowMaze();
    }

    private void ShowMaze() //미로 표시
    {
        for (int y = 0; y < maxHeight; y++)
        {
            for (int x = 0; x < maxWidth; x++)
            {
                gridGroup[y].SetImage(x, mazes[y][x]);
            }
        }
    }

    /// <summary>
    /// 백트래킹 방식으로 미로를 생성
    /// </summary>
    /// <param name="x">현재 x 좌표</param>
    /// <param name="y">현재 y 좌표</param>

    private void Backtrack(int x, int y)
    {
        visited[y, x] = true;

        // (dx, dy, 현재방향, 반대방향)
        List<(int dx, int dy, ImageStatus dir, ImageStatus opp)> directions = new List<(int, int, ImageStatus, ImageStatus)>
        {
            (-1,  0, ImageStatus.Left,  ImageStatus.Right),
            ( 1,  0, ImageStatus.Right, ImageStatus.Left),
            ( 0, -1, ImageStatus.Up,    ImageStatus.Down),
            ( 0,  1, ImageStatus.Down,  ImageStatus.Up)
        };

        // 방향 셔플
        for (int i = 0; i < directions.Count; i++)
        {
            int rnd = Random.Range(i, directions.Count);
            (directions[i], directions[rnd]) = (directions[rnd], directions[i]);
        }

        foreach (var (dx, dy, dir, opp) in directions)
        {
            int nx = x + dx;
            int ny = y + dy;

            // 범위 내 & 미방문 셀
            if (nx >= 0 && nx < maxWidth && ny >= 0 && ny < maxHeight && !visited[ny, nx])
            {
                // 현재 셀에서 dir 방향으로 길 뚫기
                mazes[y, x] |= dir;
                // 다음 셀에서 opp 방향으로 길 뚫기
                mazes[ny, nx] |= opp;

                Backtrack(nx, ny);
            }
        }
    }
}