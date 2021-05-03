using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraCreator : MonoBehaviour
{
    [SerializeField] private Tetra[] _tetraminos;
    private Tetra _currentTetramino;

    private float _timer;

    private Dictionary<Vector2Int, TetraQuad> _quadDictionary = new Dictionary<Vector2Int, TetraQuad>();
    private void Start()
    {
        CreateNext();
    }

    private void Update()
    {
        float speedMultiplayer = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            speedMultiplayer = 4f;
        }
        _timer += Time.deltaTime * speedMultiplayer;
        if (_timer >= 0.3f)
        {
            _timer = 0f;
            if (CheckAllowMove(Vector2Int.down))
            {
                _currentTetramino.Move(Vector2Int.down);
            }
            else
            {
                Separate();
                CheckLines();
                CreateNext();
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CheckAllowMove(Vector2Int.left))
            {
                _currentTetramino.Move(Vector2Int.left);
            }
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CheckAllowMove(Vector2Int.right))
            {
                _currentTetramino.Move(Vector2Int.right);
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _currentTetramino.RotateCW();
            if (!CheckAllowMove(Vector2Int.zero))
            {
                _currentTetramino.RotateCCW();
            }
        }

    }

    void Separate()
    {
        for (int i = 0; i < _currentTetramino.Quads.Length; i++)
        {
            int x = Mathf.RoundToInt(_currentTetramino.Quads[i].transform.position.x);
            int y = Mathf.RoundToInt(_currentTetramino.Quads[i].transform.position.y);
            Vector2Int key = new Vector2Int(x, y);
            _quadDictionary.Add(key, _currentTetramino.Quads[i]);
        }
    }

    void CreateNext()
    {
        int index = Random.Range(0, _tetraminos.Length);
        _currentTetramino = Instantiate(_tetraminos[index]);
    }

    bool CheckAllowMove(Vector2Int offset)
    {

        for (int i = 0; i < _currentTetramino.Quads.Length; i++)
        {
            TetraQuad quad = _currentTetramino.Quads[i];
            Vector2Int quadNextPosition = new Vector2Int(
                Mathf.RoundToInt(quad.transform.position.x), 
                Mathf.RoundToInt(quad.transform.position.y)
                ) + offset;
            if (quadNextPosition.y < 0)
            {
                return false;
            }
            if (quadNextPosition.x < 0)
            {
                return false;
            }
            if (quadNextPosition.x >= 10)
            {
                return false;
            }
            if (_quadDictionary.ContainsKey(quadNextPosition))
            {
                return false;
            }


        }
        return true;
    }

    void CheckLines()
    {
        bool[] fullLine = new bool[15];
        for (int y = 0; y < 16; y++)
        {
            bool isFull = true;
            for (int x = 0; x < 10; x++)
            {
                Vector2Int key = new Vector2Int(x, y);
                if (_quadDictionary.ContainsKey(key) == false)
                {
                    isFull = false;
                    break;
                }
            }

            if (isFull)
            {
                fullLine[y] = true;
            }
        }
        

        DeleteLines(fullLine);
    }

    void DeleteLines(bool[] fullLine)
    {
        int deleteLinesCount = 0;
        for (int y = 0; y < 15; y++)
        {
            if (fullLine[y])
            {
                for (int x = 0; x < 10; x++)
                {
                    Vector2Int key = new Vector2Int(x, y);
                    TetraQuad quad = _quadDictionary[key];
                    Destroy(quad.gameObject);
                    _quadDictionary.Remove(key);
                }
                deleteLinesCount += 1;
            }
            else
            {
                if (deleteLinesCount > 0)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        Vector2Int key = new Vector2Int(x, y);
                        if (_quadDictionary.ContainsKey(key))
                        {
                            TetraQuad quad = _quadDictionary[key];
                            //quad.transform.position += Vector3.down * deleteLinesCount;
                            quad.MoveToPosition(quad.transform.position + Vector3.down * deleteLinesCount);
                            _quadDictionary.Remove(key);
                            _quadDictionary.Add(key + Vector2Int.down * deleteLinesCount, quad);
                        }
                    }
                }
            }
            
        }
    }
}
