
using System;

public class ConditionalNode : TreeNode
{
    private Func<bool> _condition;
    private Action _action;

    public ConditionalNode(Func<bool> condition, Action action)
    {
        _condition = condition;
        _action = action;
    }
    public override bool Execute()
    {
        if(_condition.Invoke()==true)
        {
            _action?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }
}
