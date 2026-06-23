using System;

public class FuncPredicate {
    private readonly Func<bool> _predicate;

    public FuncPredicate(Func<bool> predicate) {
        _predicate = predicate;
    }

    public bool Evaluate() => _predicate();
}