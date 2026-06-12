using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropItem : MonoBehaviour
{
    public ItemData itemData;
    public int count = 1;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        // Player 태그인지 확인
        if (!other.CompareTag("Player")) return; 
        // PlayerInventory.Instance.AddItem() 호출
        if (PlayerInventory.Instance == null) return;
        bool added = PlayerInventory.Instance.AddItem(itemData, count);

        // 획득 성공 시 Destroy(gameObject)
        if (added)
        {
            Destroy(gameObject);
        }

        Debug.Log("포션 충돌 : " + other.name);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("Player 태그 아님");
            return;
        }

        Debug.Log("Player 태그 확인");

        if (PlayerInventory.Instance == null)
        {
            Debug.Log("PlayerInventory.Instance 없음");
            return;
        }

        Debug.Log("아이템 추가 결과 : " + added);

        if (added)
        {
            Debug.Log("포션 획득 성공");
            Destroy(gameObject);
        }
    }
}
