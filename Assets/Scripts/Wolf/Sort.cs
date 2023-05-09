using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : MonoBehaviour, IComparer
{
    public int Compare(Object x, Object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(y.name, x.name));
    }
}
