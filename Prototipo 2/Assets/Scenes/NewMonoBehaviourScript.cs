using UnityEngine;
using UnityEngine.SceneManagement; // trocar de cena

public class MenuController : MonoBehaviour
{
    // A fun��o Update � chamada uma vez por frame
    void Update()
    {
        // Verifica se a tecla ESPA�O foi pressionada NESTE frame
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame(); // Chama a nossa fun��o para iniciar o jogo
        }
    }

    // Fun��o que carrega a cena do jogo
    public void StartGame()
    {
        // Certifique-se de que o nome da cena est� EXATAMENTE igual ao do seu arquivo.
        // Pela sua imagem, o nome � "Game".
        SceneManager.LoadScene("Game");
    }
}