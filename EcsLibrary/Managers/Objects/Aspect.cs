using System.Collections.Generic;
using System.Text;

namespace EcsLibrary.Managers.Objects;

public struct Aspect
{
    private readonly List<ulong> _aspectBitMaps = new() { 0 };

    public bool IsEmpty()
    {
        return _aspectBitMaps.Count == 1 && _aspectBitMaps[0] == 0;
    }

    public Aspect()
    {
    }

    public void Add(int typeId)
    {
        int index = typeId / 64;
        if (index >= _aspectBitMaps.Count)
        {
            _aspectBitMaps.Add(0);
        }

        _aspectBitMaps[index] |= 1ul << typeId;
    }

    public void AddRange(int[] typeIds)
    {
        foreach (var typeId in typeIds)
        {
            Add(typeId);
        }
    }

    public bool Overlaps(Aspect other)
    {
        if (other._aspectBitMaps.Count != _aspectBitMaps.Count)
            return false;

        for (int i = 0; i < _aspectBitMaps.Count; i++)
        {
            var otherBitMap = other._aspectBitMaps[i];
            var bitMap = _aspectBitMaps[i];
            if ((otherBitMap & bitMap) != bitMap)
            {
                return false;
            }
        }

        return true;
    }
}