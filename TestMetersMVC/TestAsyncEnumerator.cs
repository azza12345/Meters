﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());

    public T Current => _inner.Current;

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }
}
