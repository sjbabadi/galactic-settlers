using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Adds repeat functionality to our inputs. Allows us to differentiate between quick press and holding down the input.
/// </summary>
class Repeater
{
    /// <summary>
    /// The amount of time to wait between an initial press of an input and the time it will start to repeat.
    /// </summary>
    const float threshold = 0.5f;
    /// <summary>
    /// The speed the input will repeat at.
    /// </summary>
    const float rate = 0.25f;
    float _next;
    /// <summary>
    /// Indicates whether or not the user is continuing to hold the button.
    /// </summary>
    bool _hold;
    string _axis;

    public Repeater(string axisName)
    {
        _axis = axisName;
    }

    public int Update()
    {
        int retValue = 0;
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

        if (value != 0)
        {
            if (Time.time > _next)
            {
                retValue = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }

        return retValue;
    }
}

/// <summary>
/// Handle inputs for the entire game. Communicates the results with other scripts via events.
/// </summary>
public class InputController : MonoBehaviour
{
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");
    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

    void Update()
    {
        int x = _hor.Update();
        int y = _ver.Update();
        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        }

        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }
}