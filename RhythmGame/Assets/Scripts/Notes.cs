using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteData data;
    public float speed = 5f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (data.type == "long")
        {
            // ロングノーツの長さを可視化したい場合はここで LineRenderer などを使う
        }

        if (transform.position.y < -5)
            Destroy(gameObject);
    }
}
