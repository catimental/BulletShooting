using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHelper : MonoBehaviour
{
    public static PositionHelper Instance;

    public void Awake()
    {
        if (PositionHelper.Instance == null)
        {
            PositionHelper.Instance = this;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 GetNormalizedVector(Vector3 vector)
    {
        return Camera.main.WorldToViewportPoint(vector);
    }

    public Vector3 GetPositionFromNormalizedVector(float x, float y)
    {
        var viewPos = GetNormalizedVector(Vector3.zero);

        //입력된 값이 0~1사이를 못벗어나게 강제조멍해주는 함수
        viewPos.x = Mathf.Clamp01(x);
        viewPos.y = Mathf.Clamp01(y); // y좌표계도 처리완료
        
        var worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        return worldPos;
    }
}
