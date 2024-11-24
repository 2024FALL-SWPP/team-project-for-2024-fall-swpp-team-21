using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : VirusBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private float aimToBeamPeriod;
    [SerializeField] private float beamToAimPeriod;
    [SerializeField] private GameObject beamPf;
    [SerializeField] private float beamArriveTime; // time to reach the player
    [SerializeField] private int beamDamage;
    [SerializeField] private float upSpeed;
    [SerializeField] private float targetYOffset;
    private LayerMask layerMask;
    private LineRenderer lineRenderer;

    protected override void Start()
    {
        base.Start();
        // Add a LineRenderer component to the GameObject
        layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Terrain");
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(GoUp());
    }

    protected override void Die()
    {
        PoolManager.instance.ReturnObject(virusData.poolType, gameObject);
    }

    // 밑에서 지상으로 올라옴
    private IEnumerator GoUp()
    {
        while (true)
        {
            float newY = Mathf.Min(transform.position.y + Time.deltaTime * upSpeed, 0);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            if (newY >= 0)
            {
                break;
            }
            yield return null;
        }
        StartCoroutine(Shoot());
    }


    private IEnumerator Shoot()
    {
        while (true)
        {
            // 조준
            float elapsedTime = 0f;
            Vector3 targetPos = player.transform.position + new Vector3(0, targetYOffset, 0);
            lineRenderer.enabled = true;
            while (elapsedTime < aimToBeamPeriod)
            {
                // 일시정지 시에도 조준선이 깜빡거리는 현상 방지
                if (Time.deltaTime > 0)
                {
                    // muzzle에서 player 방향으로 조준선 그리기
                    targetPos = player.transform.position + new Vector3(0, targetYOffset, 0);
                    lineRenderer.SetPosition(0, muzzle.position);
                    lineRenderer.SetPosition(1, targetPos);

                    // 조준선 깜빡거리게 함
                    lineRenderer.enabled = !lineRenderer.enabled;
                    elapsedTime += Time.deltaTime;
                }
                yield return null;
            }

            // 빔 발사
            lineRenderer.enabled = false;
            float beamSpeed = Vector3.Distance(muzzle.position, targetPos) / beamArriveTime;
            GameObject beam = PoolManager.instance.GetObject(PoolType.VProj_Beam, muzzle.position, Quaternion.identity);
            beam.GetComponent<VP_Beam>().Initialize(targetPos, beamSpeed, beamDamage);

            yield return new WaitForSeconds(beamToAimPeriod);
        }
    }
}
