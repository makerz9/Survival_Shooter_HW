using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string MoveAxisX = "Horizontal";
    public static readonly string MoveAxisY = "Vertical";

    public static readonly string FireAxis = "Fire1";
    public static readonly string ReloadAxis = "Reload";

    public float MoveX { get; private set;  }
    public float MoveY { get; private set;  }
    public bool Fire { get; private set;  }
    public bool Reload { get; private set;  }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnDisable()
    {
        MoveX = 0f;
        MoveY = 0f;
        Fire = false;
        Reload = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveX = Input.GetAxis(MoveAxisX);
        MoveY = Input.GetAxis(MoveAxisY);
        Fire = Input.GetButton(FireAxis);
        Fire = Input.GetButtonDown(FireAxis);
    }
}
