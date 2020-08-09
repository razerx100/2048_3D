using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject cube;

    [SerializeField]
    Material[] materials;

    private void Start()
    {
        spawn_cubes();
    }

    void spawn_cubes()
    {
        for (int i = 0, z = 10; i < 3; i++, z -= 10)
        {
            GameObject cube1 = Instantiate(cube, new Vector3(0, 0, z), Quaternion.identity);
            cube1.transform.Rotate(new Vector3(0, -90, 0));
            cube1.GetComponent<MeshRenderer>().material = materials[Random.Range(0, 11)];
        }
    }
}
