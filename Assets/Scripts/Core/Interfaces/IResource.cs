using System;

public interface IResource
{
    int Current {  get; }
    int Max { get; }

    event Action<int, int> OnChanged;
}
