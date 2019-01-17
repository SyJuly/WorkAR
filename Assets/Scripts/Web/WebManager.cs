using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebManager : MonoBehaviour
{
    public Trello Trello { get; private set; }
    public Google Google { get; private set; }

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
        Trello = new Trello();
        Google = new Google();
    }
    /*<<------------------Singleton-----------------------*/

}
