using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebManager : MonoBehaviour
{
    public Trello Trello { get; } = new Trello();
    public Google Google { get; } = new Google();

    /*------------------Singleton---------------------->>*/
    private static WebManager _instance;

    public static WebManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    /*<<------------------Singleton-----------------------*/

}
