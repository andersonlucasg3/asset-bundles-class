using UnityEngine;

public static class TransformExt
{
    public static Transform[] FindChildren(this Transform transform, string match)
    {
        using ListPool<Transform> children = ListPool<Transform>.Rent();

        FindChildren(transform, match, children);

        return children.ToArray();
    }

    private static void FindChildren(Transform transform, string match, ListPool<Transform> output)
    {
        for (int index = 0; index < transform.childCount; index++)
        {
            Transform child = transform.GetChild(index);
            if (child.childCount > 0) FindChildren(child, match, output);
            if (child.name.ToLower().Contains(match.ToLower())) output.Add(child);
        }
    }
}
