using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject cube;

    [SerializeField]
    Material[] materials;

    public float speed;

    public TouchController touch_ctrl;

    Rigidbody rb;

    public Boundary boundary;

    bool is_moving;

    private void Awake()
    {
        is_moving = false;
    }

    private void Start()
    {
        spawn_cubes();
    }

    private void FixedUpdate()
    {
        Direction direction = touch_ctrl.get_direction();
        Vector3 veloocity = Vector3.zero;
        if (!is_moving)
        {
            if (direction == Direction.Right)
            {
                veloocity.x = 1f;
                is_moving = true;
            }
            else if (direction == Direction.Left)
            {
                veloocity.x = -1f;
                is_moving = true;
            }
            else if (direction == Direction.Up)
            {
                veloocity.z = 1f;
                is_moving = true;
            }
            else if (direction == Direction.Down)
            {
                veloocity.z = -1f;
                is_moving = true;
            }
            rb.velocity = veloocity * speed;
        }
        else
        {
            if(rb.position.x > boundary.max_x)
            {
                stop_cube(rb, out is_moving);
            }
            else if(rb.position.x < boundary.min_x)
            {
                stop_cube(rb, out is_moving);
            }
            else if(rb.position.z > boundary.max_z)
            {
                stop_cube(rb, out is_moving);
            }
            else if(rb.position.z < boundary.min_z)
            {
                stop_cube(rb, out is_moving);
            }
        }
        rb.position = new Vector3(
                Mathf.Clamp(rb.position.x, boundary.min_x, boundary.max_x),
                0,
                Mathf.Clamp(rb.position.z, boundary.min_z, boundary.max_z)
             );
    }

    void spawn_cubes()
    {
        GameObject cube1 = Instantiate(cube, new Vector3(0, 0, 0), Quaternion.identity);
        cube1.transform.Rotate(new Vector3(0, -90, 0));
        cube1.GetComponent<MeshRenderer>().material = materials[Random.Range(0, 11)];
        rb = cube1.GetComponent<Rigidbody>();
    }

    void stop_cube(Rigidbody rb, out bool is_moving)
    {
        rb.velocity = Vector3.zero;
        is_moving = false;
    }
}
