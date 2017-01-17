using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Location {
    public int x;
    public int y;

    public Location(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
