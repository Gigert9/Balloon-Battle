using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursor : MonoBehaviour
{
    //Cursor Icon Variables:
    public Texture2D menuCursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        hotSpot = new Vector2(menuCursorTexture.height / 2, menuCursorTexture.width / 2);
    }

    //Cursor Icon Functions:
    void OnMouseEnter()
    {
        Cursor.SetCursor(menuCursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
