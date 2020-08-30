
public class Player
{
    private string username;
    private Coordinates coordinates;
    private Atributes atributes;
}
public class Coordinates
{
    private float x;
    private float y;

    public float X   // property
    {
        get { return x; }   // get method
       // set { x = value; }  // set method
    }
    public float Y   // property
    {
        get { return y; }   // get method
        //set { y = value; }  // set method
    }

    //public override string ToString() => $"({X}, {Y})";
}
public class Atributes
{
    private string Character;
    private int HP;
    private int Strengh;
    private int Int;
    private int Dex;

}