using System;
using System.Collections;
using UnityEngine;

public static class MonsterScanner
{
    public static GameObject ScanNearestObject(Vector3 position, float radius, LayerMask layerMask)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, layerMask);
        GameObject nearestMonster = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Virus"))
            {
                float distance = Vector3.Distance(position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestMonster = collider.gameObject;
                    nearestDistance = distance;
                }
            }
        }

        return nearestMonster;
    }

    public static GameObject ScanRandomObject(Vector3 position, float radius, LayerMask layerMask)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, layerMask);
        if (colliders.Length == 0)
        {
            return null;
        }

        return colliders[UnityEngine.Random.Range(0, colliders.Length)].gameObject;
    }

    public static IEnumerator SearchEnemyCoroutine(Transform origin, float radius, Action<GameObject> callbackOnEnemyFound, LayerMask layerMask)
    {
        const float searchTick = 0.2f;
        while (true)
        {
            yield return new WaitForSeconds(searchTick);
            GameObject target = MonsterScanner.ScanNearestObject(origin.position, radius, layerMask);
            if (target != null)
            {
                callbackOnEnemyFound(target);
                yield break;
            }
        }
    }

    public static IEnumerator SearchEnemyCoroutine(Transform origin, float radius, Action<GameObject> callbackOnEnemyFound)
    {
        return SearchEnemyCoroutine(origin, radius, callbackOnEnemyFound, LayerMask.GetMask("Virus"));
    }
}
