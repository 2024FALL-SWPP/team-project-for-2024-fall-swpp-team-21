using UnityEngine;

public class SFXSequenceTester : MonoBehaviour
{
    [SerializeField] private SFXSequencePreset sfxSequencePreset;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sfxSequencePreset.Stop();
            sfxSequencePreset.Play();
        }
    }
}
