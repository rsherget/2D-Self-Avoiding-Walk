using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class NewBehaviourScript : MonoBehaviour
{
    private Point[,] Grid;
    private static System.Random rand = new System.Random();
    private int leng = 20;
    public List<Point> Path = new List<Point>();
    private int x = 0;
    private int y = 0;
    private Point currentPoint;
    public LineRenderer Line;
    public bool Solved;

    // Start is called before the first frame update
    void Start()
    {
        
        Solved = false;
        Line = gameObject.AddComponent<LineRenderer>();
        Line = GetComponent<LineRenderer>();
        Line.material = new Material(Shader.Find("Sprites/Default"));
        Line.startWidth = 0.2f;
        Line.startColor = Color.red;
        transform.Translate(0, 0, 0);
        Grid = new Point[leng, leng];
        
        for (int i = 0; i < leng; i++)
        {
            for (int j = 0; j < leng; j++)
            {
                Grid[i, j] = new Point(i, j);
            }
        }

        currentPoint = Grid[x, y];
        currentPoint.visited = true;
        Path.Add(currentPoint);
        Draw(new Vector3(currentPoint.x, currentPoint.y, 0));
    }

    //Update is called once per frame
    void Update() {
        if (!Solved)
        {
            currentPoint = currentPoint.Next(Grid);
            if (currentPoint == null)
            {
                Path[Path.Count - 1].Clear();
                Path.RemoveAt(Path.Count - 1);
                currentPoint = Path[Path.Count - 1];
            }
            else
            {
                Path.Add(currentPoint);
                currentPoint.visited = true;
            }

            if (Path.Count == leng * leng)
            {
                Debug.Log("Solved!");
                Solved = true;
            }
            transform.position = new Vector3(currentPoint.x, currentPoint.y, 0);
            Draw(new Vector3(currentPoint.x, currentPoint.y, 0));
        }

        //Thread.Sleep(500);
    }


    private void Draw(Vector3 vector)
    {
        Line.positionCount = Path.Count;
        Line.SetPosition(Path.Count-1, vector);
    }
}

public class Step
{
    public int x;
    public int y;
    public bool tried;

    public Step(int X, int Y)
    {
        x = X;
        y = Y;
        tried = false;
    }
}

public class Point
{
    private static System.Random rand = new System.Random();
    public int x;
    public int y;
    public bool visited;
    public List<Step> allOptions;

    public int leng = 20;

    public Point(int X, int Y)
    {
        x = X;
        y = Y;
        visited = false;
        allOptions = addOptions();
    }

    private List<Step> addOptions()
    {
        List<Step> temp = new List<Step>();
        temp.Add(new Step(1, 0));
        temp.Add(new Step(-1, 0));
        temp.Add(new Step(0, 1));
        temp.Add(new Step(0, -1));
        return temp;
    }

    public void Clear()
    {
        visited = false;
        allOptions = addOptions();
    }

    public Point Next(Point[,] Grid)
    {
        List<Step> options = ValidOptions(Grid, x, y);

        if (options.Count >= 1)
        {
            int direction = rand.Next(0, options.Count);

            for (int i = 0; i < allOptions.Count; i++)
            {
                if (allOptions[i].Equals(options[direction]))
                {
                    allOptions[i].tried = true;
                }
            }

            return Grid[x + options[direction].x, y + options[direction].y];
        }
        else
        {
            return null;
        }
    }

    private bool isValid(Point[,] Grid, int x, int y)
    {
        if (x < 0 || x >= leng || y < 0 || y >= leng)
        {
            return false;
        }
        return !Grid[x, y].visited;
    }

    private List<Step> ValidOptions(Point[,] Grid, int x, int y)
    {
        List<Step> result = new List<Step>();

        for(int i = 0; i < allOptions.Count; i++)
        {
            if(isValid(Grid, x+allOptions[i].x, y + allOptions[i].y) && !allOptions[i].tried)
            {
                result.Add(allOptions[i]);
            }
        }

        return result;
    }
}
