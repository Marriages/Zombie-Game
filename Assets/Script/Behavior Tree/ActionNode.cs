
using System;
using System.Diagnostics;

public class ActionNode : TreeNode
{
    // 주어진 동작을 수행하는 노드
    private Action _action;

    public ActionNode(Action action)
    {
        _action = action;
    }

    public override bool Execute()
    {
        _action?.Invoke();
        return true;
    }
}
