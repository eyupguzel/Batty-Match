using UnityEngine;

public class Cursor : Singleton<Cursor>
{
    GridManager grid;
    private Matchable[] selected = new Matchable[2];
    private void Start()
    {
        grid = GridManager.Instance;
    }
  
    public void FirstSelect(Matchable toSelect)
    {
        selected[0] = toSelect;

        if (selected[0] == null)
            return;

        //transform.position = toSelect.transform.position;

    }
    public void SecondSelect(Matchable toSelect)
    {
        selected[1] = toSelect;

        if (selected[1] == null)
            return;


        TrySwap();


        FirstSelect(null);
    }
    private void TrySwap()
    {
        if (selected[0] == null || selected[1] == null)
            return;

        StartCoroutine(grid.SwapAnimation(selected[0], selected[1], 0.3f));

        Vector3 temp = selected[0].transform.position;
        selected[0].transform.position = selected[1].transform.position;
        selected[1].transform.position = temp;


        //grid.SwapTiles(selected[0], selected[1]);
        bool hasMatch = grid.CheckMatchAfterSwap(selected[0], selected[1]);

        Debug.Log(hasMatch);

        if (!hasMatch)
        {
            StartCoroutine(grid.SwapAnimation(selected[1], selected[0], 0.3f));

            selected[1].transform.position = selected[0].transform.position;
            selected[0].transform.position = temp;
        }
    }
}
