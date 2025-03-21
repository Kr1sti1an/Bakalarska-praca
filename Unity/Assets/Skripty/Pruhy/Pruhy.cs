using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pruhy
{  //statická trieda, môžme ju len volať priamo prostredníctvom Pruhy.
    static float[] pruhyAuta = { -0.3f, 0.3f };   //pruhy kde sa môžu AI autá alebo mince spawnovať
    public static float[] PruhyAuta => pruhyAuta;   //public property na čitanie ktorá vracia pruhyAuta
}
