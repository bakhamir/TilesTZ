using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject[] trianglePrefabs;
    public GameObject buttonPrefab; // ѕрефаб кнопки
    private Tile[,] gridArray = new Tile[3, 3];
    private Camera mainCamera;
    private int maxTriangles = 5;
    private int currentTriangleCount = 0;

    void Start()
    {
        mainCamera = Camera.main;
        CreateGrid();
        CenterCamera();
        PopulateRandomTriangles();
        CreateButtons(); // —оздаЄм кнопки дл€ ввода треугольников
    }

    void CreateGrid()
    {
        float tileSize = 1.1f;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
                GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity);
                tileObj.transform.parent = transform;

                Tile tile = tileObj.GetComponent<Tile>();
                tile.gridPosition = new Vector2Int(x, y);
                gridArray[x, y] = tile;
            }
        }
    }

    void CenterCamera()
    {
        Vector3 centerPosition = new Vector3(1, 1, -10);
        mainCamera.transform.position = centerPosition;
        mainCamera.orthographicSize = 2.5f;
    }

    void PopulateRandomTriangles()
    {
        foreach (var tile in gridArray)
        {
            if (currentTriangleCount >= maxTriangles) break;

            if (Random.value < 0.5f && !tile.HasTriangle())
            {
                AddRandomTriangleToTile(tile);
            }
        }
    }

void AddRandomTriangleToTile(Tile tile)
{
    if (tile.HasTriangle())
    {
        Debug.Log($"Tile at {tile.gridPosition} already has a triangle.");
        return;
    }

    int triangleType = Random.Range(0, trianglePrefabs.Length);
    GameObject triangle = Instantiate(trianglePrefabs[triangleType], tile.transform.position, Quaternion.identity);
    triangle.transform.parent = tile.transform;
    tile.SetTriangle(triangle);
    currentTriangleCount++;
    Debug.Log($"Triangle added to tile at position: {tile.gridPosition}");
}


    void CreateButtons()
    {
        // —оздаЄм кнопки дл€ каждой €чейки (кроме центральной)
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (x == 1 && y == 1) continue; // ѕропускаем центральную €чейку

                Vector3 buttonPosition = gridArray[x, y].transform.position + new Vector3(0f, 0.5f, 0.5f);
                GameObject buttonObj = Instantiate(buttonPrefab, buttonPosition, Quaternion.identity, transform);

                Button button = buttonObj.GetComponent<Button>();
                int tempX = x, tempY = y;
                button.onClick.AddListener(() => OnAddTriangleButtonClicked(tempX, tempY));
            }
        }
    }

    void OnAddTriangleButtonClicked(int x, int y)
    {
        Debug.Log($"Button clicked for tile at position: {x}, {y}");

        Tile targetTile = gridArray[x, y];
        if (!targetTile.HasTriangle())
        {
            AddRandomTriangleToTile(targetTile);
        }
        else
        {
            ShiftTriangle(targetTile);
        }
    }

    void ShiftTriangle(Tile tile)
    {
        Tile emptyNeighbor = FindEmptyNeighbor(tile);
        if (emptyNeighbor != null)
        {
            emptyNeighbor.SetTriangle(tile.GetTriangle()); // ѕеремещаем треугольник в пустую €чейку
            tile.ClearTriangle(); // ќчищаем текущую €чейку
        }
    }

    Tile FindEmptyNeighbor(Tile tile)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var direction in directions)
        {
            Vector2Int neighborPosition = tile.gridPosition + direction;

            if (IsWithinBounds(neighborPosition) && !gridArray[neighborPosition.x, neighborPosition.y].HasTriangle())
            {
                return gridArray[neighborPosition.x, neighborPosition.y];
            }
        }

        return null;
    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < 3 && position.y >= 0 && position.y < 3;
    }
}
