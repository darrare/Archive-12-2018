using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Modifiers
{
    public static bool IS_STATIC_MODE = true; //Static meaning you will get the same cards no matter what, according to the details in the document
    public static Dictionary<bool, int> CARDS_TO_DRAW = new Dictionary<bool, int> //based on IS_STATIC_MODE. dictates how many cards you draw
    {
        { true, 5 },
        { false, 12 }
    };
}
