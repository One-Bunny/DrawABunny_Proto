using System.Collections.Generic;
using UnityEngine;


namespace OneBunny
{
    public class LinePool : MonoBehaviour
    {
        public static LinePool Instance;

        [SerializeField]
        private GameObject _linePrefab;

        [SerializeField]
        private GameObject _lineObjPrefab;

        private Queue<LineController> _linePool = new();

        private const int _POOL_COUNT = 8;

        private void Awake()
        {
            Instance = this;
            Initialize(_POOL_COUNT);
        }
        private LineController CreateNewLine()
        {
            LineController newLine = Instantiate(_linePrefab, transform).GetComponent<LineController>();
            newLine.gameObject.SetActive(false);
            return newLine;
        }

        private GameObject CreateNewLineObj()
        {
            GameObject newLineObj = Instantiate(_lineObjPrefab, transform);
            newLineObj.SetActive(false);
            return newLineObj;
        }
        private void Initialize(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _linePool.Enqueue(CreateNewLine());
            }
        }
        public static LineController GetLine()
        {

            if (Instance._linePool.Count > 0)
            {
                LineController line = Instance._linePool.Dequeue();
                line.transform.SetParent(null);
                line.gameObject.SetActive(true);
                return line;
            }
            else
            {
                LineController newLine = Instance.CreateNewLine();
                newLine.transform.SetParent(null);
                newLine.gameObject.SetActive(true);
                return newLine;
            }
        }

        public static void ReturnLineToPool(LineController lineController)
        {
            lineController.gameObject.SetActive(false);
            lineController.transform.SetParent(Instance.transform);
            Instance._linePool.Enqueue(lineController);
        }

        public static void ReturnAllLinesToPool()
        {
            GameObject[] lines;
            lines = GameObject.FindGameObjectsWithTag("LINE");

            foreach (GameObject line in lines)
            {
                ReturnLineToPool(line.GetComponent<LineController>());
            }
        }
    }
}
