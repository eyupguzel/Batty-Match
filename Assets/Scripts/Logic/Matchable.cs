using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Matchable : MonoBehaviour
{
    private Cursor cursor;
    private SpriteRenderer _renderer;
    private int type;
    public int Type
    {
        get
        {
            return type;
        }
    }
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        cursor = Cursor.Instance;
    }
    public void SetType(int type, Sprite sprite)
    {
        this.type = type;
        _renderer.sprite = sprite;
    }

    private void OnMouseDown()
    {
        cursor.FirstSelect(this);
    }
    private void OnMouseUp()
    {
        cursor.SecondSelect(null);
    }
    private void OnMouseEnter()
    {
        cursor.SecondSelect(this);
    }
}
