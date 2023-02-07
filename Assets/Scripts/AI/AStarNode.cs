
public class AStarNode
{
    //float 대신 int를 사용하면 계산 속도가 빨라짐
    public int xPos;
    public int yPos;

    public int hCost;
    public int gCost;
    public int fCost { get { return gCost + hCost; } }

    public bool isWall; //벽 여부
    public AStarNode parent; //부모노드

    public AStarNode(int xPos, int yPos, bool isWall)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.isWall = isWall;
        parent = null;
    }
}
