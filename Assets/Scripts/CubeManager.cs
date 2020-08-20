using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    class Cube
    {
        public GameObject cube;
        public int point;
    }
    class CubeLocation
    {
        public Cube cube;
        public int offset;
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

    int total_cube, score;

    public Text score_text;

    private void Awake()
    {
        materials = new Dictionary<int, Material>();
        score = 0;
    }

    private void Start()
    {
        organize_materials();
        assign_positions();
        spawn_cube();
        spawn_cube();
        score_text.text = "Score : " + score;
    }

    void assign_positions()
    {
        all_locations = new Location[32] {
            new Location(new Vector2(cube_pos.x1, cube_pos.z8)), new Location(new Vector2(cube_pos.x2, cube_pos.z8)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z8)), new Location(new Vector2(cube_pos.x4, cube_pos.z8)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z7)), new Location(new Vector2(cube_pos.x2, cube_pos.z7)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z7)), new Location(new Vector2(cube_pos.x4, cube_pos.z7)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z6)), new Location(new Vector2(cube_pos.x2, cube_pos.z6)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z6)), new Location(new Vector2(cube_pos.x4, cube_pos.z6)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z5)), new Location(new Vector2(cube_pos.x2, cube_pos.z5)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z5)), new Location(new Vector2(cube_pos.x4, cube_pos.z5)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z4)), new Location(new Vector2(cube_pos.x2, cube_pos.z4)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z4)), new Location(new Vector2(cube_pos.x4, cube_pos.z4)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z3)), new Location(new Vector2(cube_pos.x2, cube_pos.z3)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z3)), new Location(new Vector2(cube_pos.x4, cube_pos.z3)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z2)), new Location(new Vector2(cube_pos.x2, cube_pos.z2)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z2)), new Location(new Vector2(cube_pos.x4, cube_pos.z2)),
            new Location(new Vector2(cube_pos.x1, cube_pos.z1)), new Location(new Vector2(cube_pos.x2, cube_pos.z1)),
            new Location(new Vector2(cube_pos.x3, cube_pos.z1)), new Location(new Vector2(cube_pos.x4, cube_pos.z1))
        };
    }

    void organize_materials()
    {
        for(int i = 0, j = 2; i < 11; i++, j *= 2)
        {
            materials[j] = mat_prefabs[i];
        }
    }

    public void spawn_cube(int number = 2)
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
        total_cube++;
    }

    void set_materials(Cube cb, int number)
    {
        cb.cube.GetComponent<MeshRenderer>().material = materials[number];
        cb.point = number;
    }

    void add_score(int scoree)
    {
        score += scoree;
        score_text.text = "Score : " + score;
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
        List<CubeLocation> all_cubes = new List<CubeLocation>();
        if (last_direction == Direction.Down)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 31 - i; j >= 0; j -= 4)
                {
                    find_all_cubes(i, j, all_cubes);
                }
            }
            foreach (CubeLocation cbl in all_cubes)
            {
                for (int i = 31 - cbl.offset; i >= 0; i -= 4)
                {
                    if (all_locations[i].cube == null)
                    {
                        add_destinations(cbl, i, destinations);
                        break;
                    }
                }
            }
        }
        else if (last_direction == Direction.Up)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0 + i; j < 32; j += 4)
                {
                    find_all_cubes(i, j, all_cubes);
                }
            }
            foreach (CubeLocation cbl in all_cubes)
            {
                for (int i = 0 + cbl.offset; i < 32; i += 4)
                {
                    if (all_locations[i].cube == null)
                    {
                        add_destinations(cbl, i, destinations);
                        break;
                    }
                }
            }
        }
        else if (last_direction == Direction.Left)
        {
            for (int i = 0; i < 32; i += 4)
            {
                for (int j = 0 + i; j < 4 + i; j++)
                {
                    find_all_cubes(i, j, all_cubes);
                }
            }
            foreach (CubeLocation cbl in all_cubes)
            {
                for (int i = 0 + cbl.offset; i < 4 + cbl.offset; i++)
                {
                    if (all_locations[i].cube == null)
                    {
                        add_destinations(cbl, i, destinations);
                        break;
                    }
                }
            }
        }
        else if (last_direction == Direction.Right)
        {
            for (int i = 0; i < 32; i += 4)
            {
                for (int j = 3 + i; j >= 0 + i; j--)
                {
                    find_all_cubes(i, j, all_cubes);
                }
            }
            foreach (CubeLocation cbl in all_cubes)
            {
                for (int i = 3 + cbl.offset; i >= 0 + cbl.offset; i--)
                {
                    if (all_locations[i].cube == null)
                    {
                        add_destinations(cbl, i, destinations);
                        break;
                    }
                }
            }
        }
        return destinations;
    }

    void find_all_cubes(int i, int j, List<CubeLocation> all_cubes)
    {
        if (all_locations[j].cube != null)
        {
            CubeLocation cbl = new CubeLocation();
            cbl.cube = all_locations[j].cube;
            cbl.offset = i;
            all_cubes.Add(cbl);
            all_locations[j].cube = null;
        }
    }

    void add_destinations(CubeLocation cbl, int i, List<MoveCube> destinations)
    {
        all_locations[i].cube = cbl.cube;
        MoveCube mcb = new MoveCube();
        mcb.rb = cbl.cube.cube.GetComponent<Rigidbody>();
        mcb.destination = new Vector3(all_locations[i].location.x, 0.0f, all_locations[i].location.y);
        destinations.Add(mcb);
    }

    public int cube_number()
    {
        return total_cube;
    }

    public void merge_cubes()
    {
        if (last_direction == Direction.Down)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 31 - i; j >= 4; j -= 4)
                {
                    if(all_locations[j].cube != null && all_locations[j - 4].cube != null)
                    {
                        if(all_locations[j].cube.point == all_locations[j - 4].cube.point)
                        {
                            set_materials(all_locations[j].cube, all_locations[j].cube.point * 2);
                            add_score(all_locations[j].cube.point);
                            all_locations[j - 4].cube.cube.SetActive(false);
                            all_locations[j - 4].cube = null;
                            total_cube--;
                        }
                    }
                }
            }
        }
        else if (last_direction == Direction.Up)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0 + i; j < 28; j += 4)
                {
                    if(all_locations[j].cube != null && all_locations[j + 4].cube != null)
                    {
                        if(all_locations[j].cube.point == all_locations[j + 4].cube.point)
                        {
                            set_materials(all_locations[j].cube, all_locations[j].cube.point * 2);
                            add_score(all_locations[j].cube.point);
                            all_locations[j + 4].cube.cube.SetActive(false);
                            all_locations[j + 4].cube = null;
                            total_cube--;
                        }
                    }
                }
            }
        }
        else if (last_direction == Direction.Left)
        {
            for (int i = 0; i < 32; i += 4)
            {
                for (int j = 0 + i; j < 3 + i; j++)
                {
                    if(all_locations[j].cube != null && all_locations[j + 1].cube != null)
                    {
                        if(all_locations[j].cube.point == all_locations[j + 1].cube.point)
                        {
                            set_materials(all_locations[j].cube, all_locations[j].cube.point * 2);
                            add_score(all_locations[j].cube.point);
                            all_locations[j + 1].cube.cube.SetActive(false);
                            all_locations[j + 1].cube = null;
                            total_cube--;
                        }
                    }
                }
            }
        }
        else if (last_direction == Direction.Right)
        {
            for (int i = 0; i < 32; i += 4)
            {
                for (int j = 3 + i; j >= 1 + i; j--)
                {
                    if(all_locations[j].cube != null && all_locations[j - 1].cube != null)
                    {
                        if(all_locations[j].cube.point == all_locations[j - 1].cube.point)
                        {
                            set_materials(all_locations[j].cube, all_locations[j].cube.point * 2);
                            add_score(all_locations[j].cube.point);
                            all_locations[j - 1].cube.cube.SetActive(false);
                            all_locations[j - 1].cube = null;
                            total_cube--;
                        }
                    }
                }
            }
        }
    }
}
