using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeDS
{
    private TreeNode _rootNode;

    public BehaviorTreeDS(TreeNode rootNode)
    {
        _rootNode= rootNode;
    }
    public void Tick()
    {
        _rootNode.Execute();
    }
}