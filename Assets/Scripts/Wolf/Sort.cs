using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : MonoBehaviour, IComparer<CapsuleCollider2D>
{
    public int Compare(CapsuleCollider2D x, CapsuleCollider2D y)
    {
        return (new CaseInsensitiveComparer()).Compare(x.name, y.name);
    }
}
