using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int id;
    public int width = 1;
    public int height = 1;

    public int health = 1;
    public int attack = 1;

    public Sprite itemIcon;
}
