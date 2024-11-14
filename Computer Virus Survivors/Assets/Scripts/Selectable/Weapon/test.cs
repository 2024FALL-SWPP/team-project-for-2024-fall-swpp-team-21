using UnityEngine;

public class test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key was pressed.");
            GameObject lightning = PoolManager.instance.GetObject(PoolType.Proj_ChainLightning, transform.position + new Vector3(0, 4, 0), transform.rotation);
            lightning.GetComponent<P_ChainLightning>().Initialize(1, 1, 6, 4, 3);
        }
    }
}
