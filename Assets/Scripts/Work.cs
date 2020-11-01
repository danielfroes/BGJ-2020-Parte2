using UnityEngine;

[CreateAssetMenu]
public class Work: ScriptableObject
{
    public enum workType
    {
        CleanConveyor,
        SeparateA,
        SeparateB,
        SeparateC

    }

    public workType jobType;
    public GarbageItemType referTo;

}