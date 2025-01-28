using UnityEngine;

public class MatchablePool : ObjectPool<Matchable>
{
    public int howManyTypes;
    [SerializeField] private Sprite[] sprites;

    public void RandomizeType(Matchable matchable,int type)
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("Sprites array is empty. Please assign sprites in the inspector.");
            return;
        }

        int random = Random.Range(0, howManyTypes);

        matchable.SetType(type, sprites[type]);
    }
   /* public Matchable GetRandomMatchable()
    {
        Matchable randomMatchable = GetObjectPooled();

        RandomizeType(randomMatchable);

        return randomMatchable;
    }*/
}
