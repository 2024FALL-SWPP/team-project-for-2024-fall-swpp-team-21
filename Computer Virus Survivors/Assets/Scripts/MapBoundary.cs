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

    public void SetBoundary()
    {
        Vector2 leftPoint = new Vector2(borderPoints[0].position.x, borderPoints[0].position.z);
        Vector2 rightPoint = new Vector2(borderPoints[1].position.x, borderPoints[1].position.z);
        Vector2 upPoint = leftPoint + ((rightPoint.x - leftPoint.x) / 2 + (rightPoint.y - leftPoint.y) / 2) * new Vector2(1, 1);
        Vector2 downPoint = leftPoint + ((rightPoint.x - leftPoint.x) / 2 - (rightPoint.y - leftPoint.y) / 2) * new Vector2(1, -1);

        border2DPoints = new Vector2[] { leftPoint, upPoint, rightPoint, downPoint };
        rightUpDistance = Vector2.Distance(leftPoint, upPoint);
        rightDownDistance = Vector2.Distance(leftPoint, downPoint);

        SetTransform(walls[0], leftPoint, upPoint);
        SetTransform(walls[1], leftPoint, downPoint);
        SetTransform(walls[2], rightPoint, upPoint);
        SetTransform(walls[3], rightPoint, downPoint);
    }

    private void SetTransform(GameObject wall, Vector2 point1, Vector2 point2)
    {
        wall.transform.position = new Vector3((point1.x + point2.x) / 2, wall.transform.position.y, (point1.y + point2.y) / 2);
        wall.transform.localScale = new Vector3(Vector2.Distance(point1, point2), wall.transform.localScale.y, wall.transform.localScale.z);
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


#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(MapBoundary))]
public class MapBoundaryEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set Boundary"))
        {
            MapBoundary mapBoundary = target as MapBoundary;
            mapBoundary.SetBoundary();
            Debug.Log("Successfully set boundary");
        }
    }
}
#endif
