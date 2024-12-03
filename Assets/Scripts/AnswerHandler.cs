using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TextMeshProUGUI = TMPro.TextMeshProUGUI;

public class AnswerHandler : MonoBehaviour
{
    private Button m_button;
    private Image m_image;
    private TextMeshProUGUI m_answerTMP;

    private string _answer;
    private void Start()
    {
        m_answerTMP = GetComponentInChildren<TextMeshProUGUI>();
        m_button = GetComponent<Button>();
        m_image = m_button.image;
        m_button.onClick.AddListener(Answer);
    }

    public void SetupAnswer(string _ans, Sprite _image)
    {
        m_answerTMP.text = _ans;
        _answer = _ans;
        m_image.sprite = _image;
    }

    public void ActivateButton(bool _enable)
    {
        m_button.interactable = _enable;
    }
    private void Answer()
    {
        AudioManager.Instance.OnButtonClick();
        GameManager.OnAnswer?.Invoke(_answer);
    }

    //Debug
    public void ShowAnswer()
    {
        m_answerTMP.fontStyle = FontStyles.Underline;
    }

    public void RemoveUnderLine()
    {
        m_answerTMP.fontStyle &= ~FontStyles.Underline;
    }


}
