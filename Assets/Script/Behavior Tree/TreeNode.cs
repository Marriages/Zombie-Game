using System.Collections.Generic;

public abstract class TreeNode
{
    protected List<TreeNode> childNodes = new List<TreeNode>();
    public void AddChild(TreeNode node)
    {
        childNodes.Add(node);
    }
    public abstract bool Execute();
    //abstract : 추상 클래스.
}
