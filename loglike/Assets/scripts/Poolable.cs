using UnityEngine;

public class Poolable : MonoBehaviour
{
    public virtual void OnReuse() { } // 재사용 시 초기화용
}
