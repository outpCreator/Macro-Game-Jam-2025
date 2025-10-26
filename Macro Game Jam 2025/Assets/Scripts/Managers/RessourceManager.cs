using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class RessourceManager : MonoBehaviour
{
    public static RessourceManager instance { get; private set; }

    public int count;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ObtainRessource()
    {
        count++;
    }

    public bool SpendRessource()
    {
        if (count > 0)
        {
            count--;
            return true;
        }
        return false;
    }
}
