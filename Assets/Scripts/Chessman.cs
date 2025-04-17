using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.U2D.IK;
using System.Collections.Generic;

public class Chessman : MonoBehaviour
{
    //References
    public GameObject controller;
    public GameObject moveplate;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    // Variable to keep track of "black" or "white" player
    private string player;

    //References for all the sprites that the chesspiece can be
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Take the instantiated locaton and adjust the transform
        SetCoords();

        switch (this.name)
        {
            case "black_queen":
                this.GetComponent<SpriteRenderer>().sprite = black_queen;
                player = "black";
                break;
            case "black_knight":
                this.GetComponent<SpriteRenderer>().sprite = black_knight;
                player = "black";
                break;
            case "black_bishop":
                this.GetComponent<SpriteRenderer>().sprite = black_bishop;
                player = "black";
                break;
            case "black_king":
                this.GetComponent<SpriteRenderer>().sprite = black_king;
                player = "black";
                break;
            case "black_rook":
                this.GetComponent<SpriteRenderer>().sprite = black_rook;
                player = "black";
                break;
            case "black_pawn":
                this.GetComponent<SpriteRenderer>().sprite = black_pawn;
                player = "black";
                break;
            case "white_queen":
                this.GetComponent<SpriteRenderer>().sprite = white_queen;
                player = "white";
                break;
            case "white_knight":
                this.GetComponent<SpriteRenderer>().sprite = white_knight;
                player = "white";
                break;
            case "white_bishop":
                this.GetComponent<SpriteRenderer>().sprite = white_bishop;
                player = "white";
                break;
            case "white_king":
                this.GetComponent<SpriteRenderer>().sprite = white_king;
                player = "white";
                break;
            case "white_rook":
                this.GetComponent<SpriteRenderer>().sprite = white_rook;
                player = "white";
                break;
            case "white_pawn":
                this.GetComponent<SpriteRenderer>().sprite = white_pawn;
                player = "white";
                break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXboard()
    {
        return xBoard;
    }

    public int GetYboard()
    {
        return yBoard;
    }

    public void SetXboard(int x)
    {
        xBoard = x;
    }

    public void SetYboard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            
            initiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void initiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                CastlingMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "white_pawn":  
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }

    public void CastlingMovePlate()
{
    Game sc = controller.GetComponent<Game>();

    if (HasMoved()) return; // raja sudah bergerak

    int row = (player == "white") ? 0 : 7;

    // Kingside castling
    Chessman kingsideRook = sc.GetPosition(7, row)?.GetComponent<Chessman>();
    if (kingsideRook != null && kingsideRook.name.Contains("rook") && !kingsideRook.HasMoved())
    {
        if (sc.GetPosition(5, row) == null && sc.GetPosition(6, row) == null)
        {
            if (!sc.IsPositionUnderAttack(4, row, player) &&
                !sc.IsPositionUnderAttack(5, row, player) &&
                !sc.IsPositionUnderAttack(6, row, player))
            {
                MovePlateSpawn(6, row); // tujuan raja saat rokade kanan
            }
        }
    }

    // Queenside castling
    Chessman queensideRook = sc.GetPosition(0, row)?.GetComponent<Chessman>();
    if (queensideRook != null && queensideRook.name.Contains("rook") && !queensideRook.HasMoved())
    {
        if (sc.GetPosition(1, row) == null && sc.GetPosition(2, row) == null && sc.GetPosition(3, row) == null)
        {
            if (!sc.IsPositionUnderAttack(4, row, player) &&
                !sc.IsPositionUnderAttack(3, row, player) &&
                !sc.IsPositionUnderAttack(2, row, player))
            {
                MovePlateSpawn(2, row); // tujuan raja saat rokade kiri
            }
        }
    }
}

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();
        
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
{
    Game sc = controller.GetComponent<Game>();

    int[] xMoves = { -1, -1, -1, 0, 0, 1, 1, 1 };
    int[] yMoves = { -1, 0, 1, -1, 1, -1, 0, 1 };

    for (int i = 0; i < 8; i++)
    {
        int x = xBoard + xMoves[i];
        int y = yBoard + yMoves[i];

        if (sc.PositionOnBoard(x, y) && !sc.IsPositionUnderAttack(x, y, player))
        {
            PointMovePlate(x, y);
        }
    }
}

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        } 
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            
            // Pengecekan untuk langkah awal pion (bisa maju dua langkah)
            if ((player == "white" && yBoard == 1) || (player == "black" && yBoard == 6))
            {
                int twoStepY = (player == "white") ? y + 1 : y - 1;
                if (sc.PositionOnBoard(x, twoStepY) && sc.GetPosition(x, twoStepY) == null)
                {
                    MovePlateSpawn(x, twoStepY);
                }
            }
        }

        // Pengecekan serangan diagonal
        if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null &&
            sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x + 1, y);
        }

        if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null &&
            sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x - 1, y);
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY, bool isAttack = false)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(moveplate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = isAttack;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        MovePlateSpawn(matrixX, matrixY, true);
    }

    public List<Vector2Int> GetAttackPositions()
    {
        List<Vector2Int> attackPositions = new List<Vector2Int>();

        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                AddLineAttackPositions(attackPositions, 1, 0);
                AddLineAttackPositions(attackPositions, 0, 1);
                AddLineAttackPositions(attackPositions, 1, 1);
                AddLineAttackPositions(attackPositions, -1, 0);
                AddLineAttackPositions(attackPositions, 0, -1);
                AddLineAttackPositions(attackPositions, -1, -1);
                AddLineAttackPositions(attackPositions, -1, 1);
                AddLineAttackPositions(attackPositions, 1, -1);
                break;
            case "black_knight":
            case "white_knight":
                AddKnightAttackPositions(attackPositions);
                break;
            case "black_bishop":
            case "white_bishop":
                AddLineAttackPositions(attackPositions, 1, 1);
                AddLineAttackPositions(attackPositions, 1, -1);
                AddLineAttackPositions(attackPositions, -1, 1);
                AddLineAttackPositions(attackPositions, -1, -1);
                break;
            case "black_king":
            case "white_king":
                AddKingAttackPositions(attackPositions);
                break;
            case "black_rook":
            case "white_rook":
                AddLineAttackPositions(attackPositions, 1, 0);
                AddLineAttackPositions(attackPositions, 0, 1);
                AddLineAttackPositions(attackPositions, -1, 0);
                AddLineAttackPositions(attackPositions, 0, -1);
                break;
            case "black_pawn":
                AddPawnAttackPositions(attackPositions, -1);
                break;
            case "white_pawn":
                AddPawnAttackPositions(attackPositions, 1);
                break;
        }

        return attackPositions;
    }

    private void AddLineAttackPositions(List<Vector2Int> attackPositions, int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y))
        {
            attackPositions.Add(new Vector2Int(x, y));
            if (sc.GetPosition(x, y) != null) break;
            x += xIncrement;
            y += yIncrement;
        }
    }

    private void AddKnightAttackPositions(List<Vector2Int> attackPositions)
    {
        int[] xMoves = { 1, 1, -1, -1, 2, 2, -2, -2 };
        int[] yMoves = { 2, -2, 2, -2, 1, -1, 1, -1 };

        for (int i = 0; i < 8; i++)
        {
            int x = xBoard + xMoves[i];
            int y = yBoard + yMoves[i];

            if (controller.GetComponent<Game>().PositionOnBoard(x, y))
            {
                attackPositions.Add(new Vector2Int(x, y));
            }
        }
    }

    private void AddKingAttackPositions(List<Vector2Int> attackPositions)
    {
        int[] xMoves = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] yMoves = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int x = xBoard + xMoves[i];
            int y = yBoard + yMoves[i];

            if (controller.GetComponent<Game>().PositionOnBoard(x, y))
            {
                attackPositions.Add(new Vector2Int(x, y));
            }
        }
    }

    private void AddPawnAttackPositions(List<Vector2Int> attackPositions, int direction)
    {
            Game sc = controller.GetComponent<Game>();

            int x1 = xBoard + 1;
            int x2 = xBoard - 1;
            int y = yBoard + direction;

            if (sc.PositionOnBoard(x1, y)) attackPositions.Add(new Vector2Int(x1, y));
            if (sc.PositionOnBoard(x2, y)) attackPositions.Add(new Vector2Int(x2, y));
    }

    private bool hasMoved = false;

    public bool HasMoved() => hasMoved;
    public void SetHasMoved(bool moved) => hasMoved = moved;

    }