using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private static List<Interaction> _interactions;
    private void Update()
    {
        if (_interactions.Count <= 0)
        {
            return;
        }
        GetMainInteraction().ApplyEffectMain();
        foreach (var interaction in _interactions)
        {
            interaction.ApplyEffectOther();
        }
        _interactions.Clear();
    }

    private static Interaction GetMainInteraction()
    {
        _interactions.Sort(new PriorityComparer());
        return _interactions[0];
    }

    public static void AddInteraction(Interaction interaction)
    {
        _interactions.Add(interaction);
    }
}

public class PriorityComparer : IComparer<Interaction>
{
    public int Compare(Interaction x, Interaction y)
    {
        if (x == null && y == null)
            return 0;
        if (x == null)
            return 1;
        if (y == null)
            return -1;
        return y.Priority.CompareTo(x.Priority);
    }
}