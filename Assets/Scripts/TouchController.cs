using UnityEngine;
using UnityEngine.EventSystems;

public enum Direction
{
    Up, Down, Left, Right, None
}

public class TouchController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    Vector2 origin, direction;

    int touch_ID;

    bool touched;

    private void Awake()
    {
        touched = false;
        direction = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData data)
    {
        if(touch_ID == data.pointerId)
        {
            direction = Vector2.zero;
            touched = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!touched)
        {
            touched = true;
            origin = data.position;
            touch_ID = data.pointerId;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if(touch_ID == data.pointerId)
        {
            Vector2 current_pos = data.position;
            Vector2 raw_direction = current_pos - origin;
            direction = raw_direction.normalized;
        }
    }

    public Direction get_direction()
    {
        float pos_x = Mathf.Abs(direction.x);
        float pos_y = Mathf.Abs(direction.y);

        Direction d_direction;
        if (pos_x > pos_y)
        {
            d_direction = (direction.x > 0) ? Direction.Right : Direction.Left;
        }
        else if(pos_x == 0 && pos_y == 0)
        {
            d_direction = Direction.None;
        }
        else
        {
            d_direction = (direction.y > 0) ? Direction.Up : Direction.Down;
        }
        return d_direction;
    }
}
