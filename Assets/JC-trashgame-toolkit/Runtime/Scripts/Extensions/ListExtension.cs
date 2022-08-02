using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class ListExtension
{
    /// <summary>
    /// Draw a random element from a list
    /// </summary>
    /// <returns></returns>
    public static T Random<T>(this List<T> list)
    {
        return RandomN<T>(list, 1)[0];
    }

    /// <summary>
    /// Draw N random elements from a list, with repeat
    /// </summary>
    /// <param name="list"></param>
    /// <param name="itemsToDraw">draw how many items? (N)</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> RandomN<T>(this List<T> list, int itemsToDraw, bool repeat = false)
    {
        // The copy of the original list, we will remove the element we pick out
        List<T> sourceList = new List<T>(list);
        // The result list
        List<T> targetList = new List<T>();

        while (targetList.Count < itemsToDraw && sourceList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, sourceList.Count);
            targetList.Add(sourceList[index]);
            if(!repeat)
                sourceList.RemoveAt(index);
        }

        return targetList;
    }

    /// <summary>
    /// Shuffle a list directly
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = UnityEngine.Random.Range(0, n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}