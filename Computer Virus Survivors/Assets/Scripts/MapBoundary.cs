using UnityEngine;

public class MapBoundary : Singleton<MapBoundary>
{
    [SerializeField] private GameObject[] walls; // LU, LD, RU, RD
    [SerializeField] private Transform[] borderPoints; // L, R

    private Vector2[] border2DPoints; // L, U, R, D
    private float rightUpDistance;
    private float rightDownDistance;

    public override void Initialize()
    {

    }

    private void Start()
    {
        SetBoundary();
    }

    private void SetBoundary()
    {
        Vector2 leftPoint = new Vector2(borderPoints[0].position.x, borderPoints[0].position.z);
        Vector2 rightPoint = new Vector2(borderPoints[1].position.x, borderPoints[1].position.z);
        Vector2 upPoint = leftPoint + ((rightPoint.x - leftPoint.x) / 2 + (rightPoint.y - leftPoint.y) / 2) * new Vector2(1, 1);
        Vector2 downPoint = leftPoint + ((rightPoint.x - leftPoint.x) / 2 - (rightPoint.y - leftPoint.y) / 2) * new Vector2(1, -1);

        border2DPoints = new Vector2[] { leftPoint, upPoint, rightPoint, downPoint };
        rightUpDistance = Vector2.Distance(leftPoint, upPoint);
        rightDownDistance = Vector2.Distance(leftPoint, downPoint);
    }

    public Vector2 GetRandomPoint()
    {
        float ru = Random.Range(0, rightUpDistance);
        float rd = Random.Range(0, rightDownDistance);
        return border2DPoints[0] + ru * new Vector2(1, 1).normalized + rd * new Vector2(1, -1).normalized;
    }

    public bool PointInBoundary(Vector2 point)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2 a = border2DPoints[i];
            Vector2 b = border2DPoints[(i + 1) % 4];

            if (Cross(b - a, point - a) >= 0)
            {
                return false;
            }
        }

        return true;
    }

    private float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }
}