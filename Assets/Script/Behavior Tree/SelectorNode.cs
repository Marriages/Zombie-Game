public class SelectorNode : TreeNode
{
    // 자식 노드 중 하나의 노드가 성공하면 성공을 반환. 모든 자식 노드가 실패하면 실패를 반환함. : OR
    public override bool Execute()
    {
        foreach(TreeNode childNode in childNodes)
        {
            if(childNode.Execute())
            {
                return true;
            }
        }

        return false;
    }
}
