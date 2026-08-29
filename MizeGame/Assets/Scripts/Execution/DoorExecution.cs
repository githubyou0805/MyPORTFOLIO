using UnityEngine;

public class DoorExecution : MonoBehaviour
{
    [SerializeField] private GameObject doorPrefab;

    private DoorLogic logic;

    private GameObject doorObject;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (logic.TryOpen())
            {
                OpenDoor();
            }
        }
    }
    public void Spawn()
    {
        int seed = System.Environment.TickCount;

        logic = new DoorLogic(
            seed,
            15
        );

        logic.SpawnAwayFromPlayer();

        Vector3 position = new Vector3(
            DoorData.X,
            DoorData.Y,
            -1
        );

        doorObject = Instantiate(
            doorPrefab,
            position,
            Quaternion.identity,
            transform
        );
    }
    public void Respawn()
    { 
        // 古い扉を削除
        if (doorObject != null) 
        { 
            Destroy(doorObject); doorObject = null;
        }
        // 現在のDoorDataの位置に扉を再生成
        Vector3 position = new Vector3( DoorData.X, DoorData.Y, -1 ); 
        doorObject = Instantiate( doorPrefab, position, Quaternion.identity, transform );
    }
    private void SpawnDoor()
    {
        logic.SpawnAwayFromPlayer();

        Vector3 position = new Vector3(
            DoorData.X,
            DoorData.Y,
            -1
        );

        doorObject = Instantiate(
            doorPrefab,
            position,
            Quaternion.identity,
            transform
        );
    }

    private void OpenDoor()
    {
        if (doorObject != null)
        {
            Destroy(doorObject);
        }

        Debug.Log("扉が開きました！");
    }
}
