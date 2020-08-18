using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<bool> is_moving;

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
        is_moving = new List<bool>();
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
        control_cube();
    }

    void control_cube()
    {
        if (last_dir != Direction.None && !cubes_moving)
        {
            cubes_destination = cb_mngr.get_destinations();
            is_moving = new List<bool>();
            foreach (MoveCube m_cb in cubes_destination)
            {
                is_moving.Add(true);
            }
            cubes_moving = true;
        }
        for(int i = 0; i < cubes_destination.Count; i++)
        {
            Debug.Log(cubes_destination[i].destination);
            cubes_destination[i].rb.transform.position = Vector3.MoveTowards(cubes_destination[i].rb.transform.position, cubes_destination[i].destination, Time.fixedDeltaTime * speed);
            if(cubes_destination[i].rb.transform.position == cubes_destination[i].destination)
            {
                is_moving[i] = false;
            }
        }
        if (cubes_moving)
        {
            bool cbs_moving = true;
            foreach (bool cb in is_moving)
            {
                if (cb)
                {
                    cbs_moving = false;
                }
            }
            cubes_moving = cbs_moving;
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
