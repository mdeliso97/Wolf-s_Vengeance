using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : MonoBehaviour, IComparer<CircleCollider2D>
{
    public int Compare(CircleCollider2D x, CircleCollider2D y)
    {
        return (new CaseInsensitiveComparer()).Compare(x.name, y.name);
    }
}
