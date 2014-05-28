using System;

[Serializable]
public class DropOutStack<T>
{
    
    private T[] items;
    private int top;
    private int _count;

    public DropOutStack(int capacity)
    {
        items = new T[capacity];
    }

    public void Push(T item)
    {
        _count += 1;
        _count = _count > items.Length ? items.Length : _count;

        items[top] = item;
        top = (top + 1)%items.Length;
    }

    public T Pop()
    {
        _count -= 1;
        _count = _count < 0 ? 0 : _count;

        top = (items.Length + top - 1)%items.Length;
        return items[top];
    }

    public T Peek()
    {
        return items[(items.Length + top - 1)%items.Length]; //Same as pop but without changing the value of top.
    }

    public int Count()
    {
        return _count;
    }

    public T GetItem(int index)
    {
        if (index > Count())
        {
            throw new InvalidOperationException("Index out of bounds");
        }

        else
        {
            //The first element = last element entered = index 0 is at Peek - see above.
            //index 0 = items[(items.Length + top - 1) % items.Length];
            //index 1 = items[(items.Length + top - 2) % items.Length];
            //index 2 = items[(items.Length + top - 3) % items.Length]; etc...
            //So to get an item at a certain index is:
            //items[(items.Length + top - (index+1)) % items.Length];

            return items[(items.Length + top - (index + 1))%items.Length];
        }
    }

    public void Clear()
    {
        _count = 0;
    }
}