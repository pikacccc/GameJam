using System.Collections;
using Runtime.PKGameCore.PKGameInit;
using Runtime.PKGameCore.PKUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.PKGameCore
{
    public class GamePrepare : MonoBehaviour
    {
        public string gameScene;

        public GameObject gameInit;

        private void Start()
        {
            StartCoroutine(InitGame());
        }

        IEnumerator InitGame()
        {
            //这里延迟0.1秒加载是为了保证URP初始化成功
            yield return new WaitForSeconds(0.1f);
            
            //初始化管理器
            var obj = Instantiate(this.gameInit);
            obj.GetComponent<GameInit>().Init();

            yield return null;
            if (gameScene != "") SceneManager.LoadScene(gameScene);
        }
    }
}