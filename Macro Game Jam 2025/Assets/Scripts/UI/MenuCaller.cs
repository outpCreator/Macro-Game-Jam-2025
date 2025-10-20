using UnityEngine;

public class MenuCaller : MonoBehaviour
{
    [SerializeField] string menuName;

    public void CallMenu()
    {
        UIManager.Instance.ShowMenu(menuName);
    }
}
