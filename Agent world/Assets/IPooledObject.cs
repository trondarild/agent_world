using UnityEngine;

public interface IPooledObject
{
    // Start is called before the first frame update
    void OnObjectSpawn();
    bool IsDead();
    void Update();
    
}
