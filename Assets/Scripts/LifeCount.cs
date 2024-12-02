using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{
    public Sprite noLifeSprite;

    public Image[] lifeImage;
    private float animationScale = 0.2f;
    public void RemoveHeart(int _life)
    {
        //lifeImage[_life].sprite = noLifeSprite;
        lifeImage[_life].gameObject.LeanScale(Vector3.one * animationScale, 0.2f).setOnComplete(() =>
        {
            lifeImage[_life].sprite = noLifeSprite;
            lifeImage[_life].gameObject.LeanScale(Vector3.one, 0.2f);
        });
    }
}
