using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    public class DissolveChilds : MonoBehaviour
    {
        // Start is called before the first frame update
        private List<Material> materials = new List<Material>();

        private void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }

            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_NoiseSeed", Random.value);
            }
        }

        private void Reset()
        {
            Start();
            SetValue(0);
        }

        // Update is called once per frame
        private void Update()
        {

            var value = Mathf.PingPong(Time.time * 0.5f, 1f);
            SetValue(value);
        }

        //IEnumerator enumerator()
        //{

        //    //float value =         while (true)
        //    //{
        //    //    Mathf.PingPong(value, 1f);
        //    //    value += Time.deltaTime;
        //    //    SetValue(value);
        //    //    yield return new WaitForEndOfFrame();
        //    //}
        //}

        public void SetValue(float value)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve", value);
            }
        }
    }
}
