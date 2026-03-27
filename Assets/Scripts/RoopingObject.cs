using UnityEngine;

public class RoopingObject : MonoBehaviour
{
    [SerializeField]
    private float width;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -width)
        {
            transform.position += new Vector3(width * 2f, 0, 0);
        }
    }
}
