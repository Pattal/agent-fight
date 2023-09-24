using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static bool TryCast<T>(object obj, out T result)
    {
        if (obj is T)
        {
            result = (T)obj;
            return true;
        }

        result = default;
        return false;
    }

    public static void TryCast<T1, T2>(List<T1> listOfObjectsToCast, List<T2> listOfObjectsAfterCast)
    {
        foreach (T1 manager in listOfObjectsToCast)
        {
            if (TryCast(manager, out T2 updateable)) listOfObjectsAfterCast.Add(updateable);
        }
    }

    public static List<T2> TryCast<T1, T2>(List<T1> listOfObjectsToCast)
    {
        List<T2> result = new();

        foreach (T1 manager in listOfObjectsToCast)
        {
            if (TryCast(manager, out T2 updateable)) result.Add(updateable);
        }

        return result;
    }
}
