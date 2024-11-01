using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition; // ������� �� �����
    private GameObject triangle; // ������ �� ����������� ������ ������

    public void SetTriangle(GameObject newTriangle)
    {
        triangle = newTriangle;
    }

    public GameObject GetTriangle()
    {
        return triangle;
    }

    public bool HasTriangle()
    {
        return triangle != null;
    }

    public void ClearTriangle()
    {
        if (triangle != null)
        {
            Destroy(triangle);
            triangle = null;
        }
    }
}
