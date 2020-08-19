using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float speed;

    public TouchController touch_ctrl;


    List<MoveCube> cubes_destination;

    public Boundary boundary;

    CubeManager cb_mngr;

    public Direction last_dir;

    bool cubes_moving;

    private void Awake()
    {
        cubes_moving = false;
    }

    private void Start()
    {
        cb_mngr = GetComponent<CubeManager>();
        cubes_destination = new List<MoveCube>();
    }

    private void FixedUpdate()
    {
        last_dir = touch_ctrl.get_direction();
        cb_mngr.last_direction = last_dir;
        control_cubes();
    }

    void control_cubes()
    {
        if (last_dir != Direction.None && !cubes_moving)
        {
            cubes_destination = cb_mngr.get_destinations();
            cubes_moving = true;
        }
        for(int i = 0; i < cubes_destination.Count; i++)
        {
            cubes_destination[i].rb.transform.position = Vector3.MoveTowards(cubes_destination[i].rb.transform.position, cubes_destination[i].destination, Time.fixedDeltaTime * speed);
        }
        if (cubes_moving)
        {
            cubes_moving = false;
            foreach(MoveCube mcb in cubes_destination)
            {
                if(!Mathf.Approximately(mcb.rb.velocity.x, 0.0f) || !Mathf.Approximately(mcb.rb.velocity.z, 0.0f))
                {
                    cubes_moving = true;
                    break;
                }
            }
        }
        foreach (MoveCube cb in cubes_destination)
        {
            cb.rb.position = new Vector3(
                    Mathf.Clamp(cb.rb.position.x, boundary.min_x, boundary.max_x),
                    0,
                    Mathf.Clamp(cb.rb.position.z, boundary.min_z, boundary.max_z)
                 );
        }
    }
}
