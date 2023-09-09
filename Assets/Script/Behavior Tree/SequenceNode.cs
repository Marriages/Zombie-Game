public class SequenceNode : TreeNode
{
    // 자식 노두 모두 순차적으로 성공해야만 성공을 반환하고, 하나라도 실패할 경우 실패를 반환 :: AND
    public override bool Execute()
    {
        foreach (TreeNode childNode in childNodes)
        {
            if (!childNode.Execute())
            {
                return false;
            }
        }

        return true;
    }
}
