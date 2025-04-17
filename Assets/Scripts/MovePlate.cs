using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    //Board postions, not world positions
    int matrixX;
    int matrixY;

    //false movement, true: attacking
    public bool attack = false;

    public void Awake()
    {
        if (attack)
        {
            //change to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("Black");
            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("White");
            
            Destroy(cp);
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXboard(),
        reference.GetComponent<Chessman>().GetYboard());

        reference.GetComponent<Chessman>().SetXboard(matrixX);
        reference.GetComponent<Chessman>().SetYboard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        // Rokade (Castling)
        if (reference.name.Contains("king"))
        {
            int y = reference.GetComponent<Chessman>().GetYboard();

            // Kingside castling
            if (matrixX == 6)
            {
                GameObject rook = controller.GetComponent<Game>().GetPosition(7, y);
                controller.GetComponent<Game>().SetPositionEmpty(7, y);
                rook.GetComponent<Chessman>().SetXboard(5);
                rook.GetComponent<Chessman>().SetYboard(y);
                rook.GetComponent<Chessman>().SetCoords();
                controller.GetComponent<Game>().SetPosition(rook);
                rook.GetComponent<Chessman>().SetHasMoved(true);
            }

            // Queenside castling
            if (matrixX == 2)
            {
                GameObject rook = controller.GetComponent<Game>().GetPosition(0, y);
                controller.GetComponent<Game>().SetPositionEmpty(0, y);
                rook.GetComponent<Chessman>().SetXboard(3);
                rook.GetComponent<Chessman>().SetYboard(y);
                rook.GetComponent<Chessman>().SetCoords();
                controller.GetComponent<Game>().SetPosition(rook);
                rook.GetComponent<Chessman>().SetHasMoved(true);
            }
        }

        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<Chessman>().DestroyMovePlates();

        reference.GetComponent<Chessman>().SetHasMoved(true);

    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject getreference()
    {
        return reference;
    }
}
