using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meal
{
    bool mIsPoisoned;

    public Meal()
    {
        mIsPoisoned = false;
    }

    public bool isPoisoned()
    {
        return mIsPoisoned;
    }

    public void setPoisoned(bool poisoned)
    {
        mIsPoisoned = poisoned;
    }
}
