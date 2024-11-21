using UnityEngine;

public class ParticleSizeController : MonoBehaviour
{
    public ParticleSystem particleEffect;  // 파티클 시스템 연결
    public float startSize = 0f;           // 기본 크기 값 설정

    void Start()
    {
        if (particleEffect == null)
        {
            particleEffect = GetComponent<ParticleSystem>();
        }

        // Start에서 한 번만 초기 크기를 설정
        SetStartSize(startSize);
    }

    // Start Size 값을 설정하는 메서드
    public void SetStartSize(float size)
    {
        var main = particleEffect.main;
        main.startSize = size;
    }

    void Update()
    {
        // Inspector에서 startSize 값을 변경할 때마다 실시간 반영
        SetStartSize(startSize);
    }
}