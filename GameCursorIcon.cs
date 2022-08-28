using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursorIcon : MonoBehaviour
{
    //Cursor Icon Variables:
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        hotSpot = new Vector2(cursorTexture.height / 2, cursorTexture.width / 2);

    }

    //Cursor Icon Functions:
    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
