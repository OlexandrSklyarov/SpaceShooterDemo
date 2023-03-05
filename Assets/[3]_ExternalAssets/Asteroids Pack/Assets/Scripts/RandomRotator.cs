using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble = 0.1f;

    void Start()
    {
        transform.rotation = Quaternion.LookRotation(Random.insideUnitSphere * tumble);
    }
}