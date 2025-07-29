// UIManager.cs
using UnityEngine;
using UnityEngine.UI; // Essencial para componentes de UI como Slider e Text
using TMPro;          // Essencial para componentes de TextMeshPro

public class UIManager : MonoBehaviour
{
    // Padr�o Singleton para ser facilmente acess�vel
    public static UIManager instance;

    [Header("Refer�ncias da UI de Mana")]
    [SerializeField] private Slider manaBar;
    [SerializeField] private TextMeshProUGUI manaText;

    void Awake()
    {
        // Configura��o do Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fun��o p�blica que ser� chamada por outros scripts para atualizar a UI
    public void UpdateManaUI(float currentMana, float maxMana)
    {
        // Atualiza a barra de mana (Slider)
        if (manaBar != null)
        {
            // O valor do slider vai de 0 a 1.
            // Para isso, dividimos a mana atual pela m�xima para obter uma porcentagem.
            manaBar.value = currentMana / maxMana;
        }

        // Atualiza o texto de mana
        if (manaText != null)
        {
            // 'F0' formata o n�mero para n�o ter casas decimais.
            manaText.text = $"{currentMana.ToString("F0")} / {maxMana.ToString("F0")}";
        }
    }
}