using UnityEngine;
using System.Collections;

public class JsonObject : MonoBehaviour
{
    public Vector3 position;
    public int ID;
    public virtual string GetJson()
    {
        position = transform.position;
        ID = gameObject.GetInstanceID();
        return JsonUtility.ToJson(this);
    }

    public virtual void SetJson(string value)
    {
        JsonUtility.FromJsonOverwrite(value, this);
        transform.position = position;
    }
}