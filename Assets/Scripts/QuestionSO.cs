using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Question")]
public class QuestionSO : ScriptableObject
{
    public int _id;
    [TextArea(2,3)]
    public string _question;
    public string _answer;
    public string[] _choices;

    public bool CheckAnswer(string _ans)
    {
        return _answer.Equals(_ans);
    }
}
