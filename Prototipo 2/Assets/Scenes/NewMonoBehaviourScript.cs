// MenuController.cs (Modificado para Toque/Clique)
using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para trocar de cena

public class MenuController : MonoBehaviour
{
    // A fun��o Update � chamada uma vez por frame
    void Update()
    {
        // --- L�GICA DE INPUT PARA TOQUE/CLIQUE ---

        if (Input.GetMouseButtonDown(0))
        {
            StartGame(); 
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    // Fun��o que carrega a cena do jogo
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}