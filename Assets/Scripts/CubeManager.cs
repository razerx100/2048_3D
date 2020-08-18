using System.Collections.Generic;
using UnityEngine;

public class MoveCube
{
    public Rigidbody rb;
    public Vector3 destination;
}

public class CubeManager : MonoBehaviour
{
    class Cube
    {
        public GameObject cube;
        public int point;
    }
    class Location
    {
        public Vector2 location;
        public Cube cube;
        public Location(Vector2 loc)
        {
            location = loc;
        }
    }
    Location[] all_locations;

    public Direction last_direction;

    public Material[] mat_prefabs;
    Dictionary<int, Material> materials;

    private void Awake()
    {
        materials = new Dictionary<int, Material>();
    }

    private void Start()
    {
        organize_materials();
        assign_positions();
        spawn_cube();
        spawn_cube(4);
    }

    void assign_positions()
    {
        all_locations = new Location[32] {
            new Location(new Vector2(cube_pos.x1, cube_pos.z8)), new Location(new Vector2(cube_pos.x2, cube_pos.z8)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z8)), new Location(new Vector2(cube_pos.x3, cube_pos.z8)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z7)), new Location(new Vector2(cube_pos.x2, cube_pos.z7)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z7)), new Location(new Vector2(cube_pos.x3, cube_pos.z7)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z6)), new Location(new Vector2(cube_pos.x2, cube_pos.z6)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z6)), new Location(new Vector2(cube_pos.x3, cube_pos.z6)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z5)), new Location(new Vector2(cube_pos.x2, cube_pos.z5)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z5)), new Location(new Vector2(cube_pos.x3, cube_pos.z5)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z4)), new Location(new Vector2(cube_pos.x2, cube_pos.z4)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z4)), new Location(new Vector2(cube_pos.x3, cube_pos.z4)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z3)), new Location(new Vector2(cube_pos.x2, cube_pos.z3)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z3)), new Location(new Vector2(cube_pos.x3, cube_pos.z3)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z2)), new Location(new Vector2(cube_pos.x2, cube_pos.z2)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z2)), new Location(new Vector2(cube_pos.x3, cube_pos.z2)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z1)), new Location(new Vector2(cube_pos.x2, cube_pos.z1)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z1)), new Location(new Vector2(cube_pos.x3, cube_pos.z1))
        };
    }

    void organize_materials()
    {
        for(int i = 0, j = 2; i < 11; i++, j *= 2)
        {
            materials[j] = mat_prefabs[i];
        }
    }

    void spawn_cube(int number = 2)
    {
        GameObject cube = CubePool.shared_instance.get_pooled_object();
        cube.SetActive(true);
        int random_position = get_random_position();
        Vector2 loc = all_locations[random_position].location;
        cube.transform.position = new Vector3(loc.x, 0.0f, loc.y);
        Cube cb = new Cube();
        cb.cube = cube;
        set_materials(cb, number);
        all_locations[random_position].cube = cb;
    }

    void set_materials(Cube cb, int number)
    {
        cb.cube.GetComponent<MeshRenderer>().material = materials[number];
        cb.point = number;
    }

    int get_random_position()
    {
        int num;
        while (true)
        {
            num = Random.Range(0, 32);
            if (all_locations[num].cube == null)
            {
                break;
            }
        }
        return num;
    }

    public List<MoveCube> get_destinations()
    {
        List<MoveCube> destinations = new List<MoveCube>();
        if(last_direction == Direction.Down)
        {
            for(int i = 0; i < 4; i++)
            {
                List<Cube> column_cubes = new List<Cube>();
                for(int j = 31 - i; j >= 0; j -= 4)
                {
                    if(all_locations[j].cube != null)
                    {
                        column_cubes.Add(all_locations[j].cube);
                        all_locations[j].cube = null;
                    }
                }
                for(int j = 0, k = 31 - i; j < column_cubes.Count; j++, k -= 4)
                {
                    MoveCube m_cb = new MoveCube();
                    m_cb.rb = column_cubes[j].cube.GetComponent<Rigidbody>();
                    m_cb.destination = new Vector3(all_locations[k].location.x, 0.0f, all_locations[k].location.y);
                    all_locations[k].cube = column_cubes[j];
                    destinations.Add(m_cb);
                }
            }
        }
        return destinations;
    }
}
