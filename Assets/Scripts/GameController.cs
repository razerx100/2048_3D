using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float speed;

    public TouchController touch_ctrl;

    public GameObject restart_button, quit_button, pause_button;

    public Text game_over_text;

    List<MoveCube> cubes_destination;

    public Boundary boundary;

    CubeManager cb_mngr;

    public Direction last_dir;

    bool cubes_moving, game_running;

    private void Awake()
    {
        cubes_moving = false;
        game_running = true;
        game_over_text.text = "";
    }

    private void Start()
    {
        cb_mngr = GetComponent<CubeManager>();
        cubes_destination = new List<MoveCube>();
        restart_button.SetActive(false);
        quit_button.SetActive(false);
    }

    private void FixedUpdate()
    {
        last_dir = touch_ctrl.get_direction();
        control_cubes();
    }

    void control_cubes()
    {
        if (last_dir != Direction.None && !cubes_moving && game_running)
        {
            cb_mngr.last_direction = last_dir;
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
                if(!Mathf.Approximately(mcb.rb.position.x, mcb.destination.x) || !Mathf.Approximately(mcb.rb.position.z, mcb.destination.z))
                {
                    cubes_moving = true;
                    break;
                }
            }
            if (!cubes_moving)
            {
                if (cb_mngr.cube_number() < 32)
                {
                    if (cb_mngr.has_moved(cubes_destination))
                    {
                        cb_mngr.spawn_cube();
                    }
                }
                else
                {
                    if (!cb_mngr.is_moveable()) {
                        game_over_text.text = "Game over!";
                        restart_button.SetActive(true);
                        quit_button.SetActive(true);
                        pause_button.SetActive(false);
                        game_running = false;
                    }
                }
                cubes_destination = new List<MoveCube>();
                cb_mngr.merge_cubes();
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

    public void quit_game()
    {
        Application.Quit();
    }

    public void restart_game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void toggle_game_running()
    {
        quit_button.SetActive(game_running);
        restart_button.SetActive(game_running);
        game_running = !game_running;
    }
}
