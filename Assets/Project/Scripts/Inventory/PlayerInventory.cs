using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public int bagSlotCount = 12;
    public int equipSlotCount = 3;

    public List<InventoryItem> bagItems = new List<InventoryItem>();
    public List<InventoryItem> equipItems = new List<InventoryItem>();

    private void Awake()
    {
        // 인벤토리 매니저 인스턴스 등록
        Instance = this;
        // bagItems와 equipItems를 슬롯 수만큼 null로 채우기
         
        bagItems.Clear();   // 1. 기존 가방 아이템 리스트 초기화 
        equipItems.Clear(); // 2. 기존 장비 아이템 리스트 초기화
                
        FillEmptySlots(bagItems, bagSlotCount);     // 3. 가방 슬롯 개수만큼 빈 슬롯 생성
        FillEmptySlots(equipItems, equipSlotCount); // 4. 장비 슬롯 개수만큼 빈 슬롯 생성
    }

    private void FillEmptySlots(List<InventoryItem> list, int slotCount)
    {
        // 5. 리스트의 개수가 슬롯 개수보다 적으면 빈 슬롯 추가
        while (list.Count < slotCount)
        {
            list.Add(null); // 6. 빈 슬롯을 의미하는 null 추가
        }
    }

    public bool AddItem(ItemData itemData, int count = 1)
    {
        // 같은 아이템이 있으면 개수 누적
        if (itemData == null) return false; // 1. 추가할 ItemData가 없습니다.
        if (count <= 0) return false;       // 2. 추가할 아이템 개수가 0 이하입니다.
                                            
        if (itemData.canStack)              // 3. 이미 있는 스택 아이템에 추가
        {
            for (int i = 0; i < bagItems.Count; i++)    // 4. 가방 슬롯을 처음부터 끝까지 검사
            {
                InventoryItem item = bagItems[i];       // 5. 현재 슬롯에 들어있는 아이템 가져오기

                // 6. 슬롯에 아이템이 있고, 같은 아이템이며, 최대 스택 개수보다 적게 쌓여있는지 확인
                if (item != null && item.data == itemData && item.count < itemData.maxStack)
                {
                    // 7. 현재 스택에 추가할 수 있는 개수 계산
                    int addCount = Mathf.Min(count, itemData.maxStack - item.count); 
                    item.count += addCount; // 8. 기존 스택 아이템 개수 증가 
                    count -= addCount;      // 9. 추가한 개수만큼 남은 획득 개수 감소

                    // 10. 모든 아이템을 스택에 추가했다면 성공 처리
                    if (count <= 0)
                    {
                        // 11. 스택 추가 성공 로그 출력
                        Debug.Log(itemData.itemName + " 스택 추가 성공"); 
                        return true; // 12. 아이템 추가 성공 반환
                    }
                }
            }
        }

        // 빈 칸을 찾아 새 아이템 넣기 

        for (int i = 0; i < bagItems.Count; i++) // 1. 빈 슬롯에 새로 추가
        {
            // 2. 현재 슬롯이 비어있거나 아이템 데이터가 없는 슬롯인지 확인
            if (bagItems[i] == null || bagItems[i].data == null)
            {
                // 3. 스택 가능한 아이템이면 최대 스택 수까지 추가하고, 아니면 1개만 추가
                int addCount = itemData.canStack ? Mathf.Min(count, itemData.maxStack) : 1;
                // 4. 빈 슬롯에 새 인벤토리 아이템 생성 후 추가
                bagItems[i] = new InventoryItem(itemData, addCount);
                count -= addCount; // 5. 추가한 개수만큼 남은 획득 개수 감소

                // 6. 새 슬롯 추가 성공 로그 출력
                Debug.Log(itemData.itemName + " 새 슬롯에 추가 성공");

                if (count <= 0)  // 7. 모든 아이템을 추가했다면 성공 처리
                {
                    return true;    // 8. 아이템 추가 성공 반환
                }
            }
        }





        return false;
    }

 
    public void MoveItem(List<InventoryItem> fromList, int fromIndex, List<InventoryItem> toList, int toIndex)
    { 
        if (!IsValidIndex(fromList, fromIndex) || !IsValidIndex(toList, toIndex)) return;

        //if (fromList[fromIndex] == null) return 
        // 기존에는 null만 검사했지만, 아이템 데이터가 없거나 개수가 0 이하인 경우도 빈 슬롯으로 봐야 함
        // 1. fromList[fromIndex] == null 검사 대신 IsEmpty()를 사용해 더 정확하게 빈 아이템을 검사
        InventoryItem fromItem = fromList[fromIndex];
        if (IsEmpty(fromItem)) return;  // 3. 이동할 아이템이 비어 있으면 이동하지 않음

        // 4. 가방에서 장비 슬롯으로 이동하는 경우인지 확인
        bool isBagToEquip = fromList == bagItems && toList == equipItems;

        // 5. 장비 슬롯에서 가방으로 이동하는 경우인지 확인
        bool isEquipToBag = fromList == equipItems && toList == bagItems;

        
        if (isBagToEquip) // 6. 가방 → 장착
        { 
            MoveOneItemToEquip(fromIndex, toIndex); // 11. 가방 아이템을 장비 슬롯으로 이동 처리 
            return;  // 7. 장착 이동 처리를 끝! 함수 종료               
        }
         
        
        if (isEquipToBag) // 8. 장착 → 가방
        {
            
            MoveEquipItemToBag(fromIndex, toIndex); // 9. 장착 아이템을 가방 슬롯으로 이동 처리

            
            return;  // 9. 장착 해제 처리를 끝냈으므로 함수 종료
        }
         
         
        InventoryItem temp = toList[toIndex]; 
        toList[toIndex] = fromList[fromIndex]; 
        fromList[fromIndex] = temp;
    }

    private void MoveOneItemToEquip(int bagIndex, int equipIndex)
    { 
        InventoryItem bagItem = bagItems[bagIndex]; // 1. 가방 슬롯에 있는 아이템 가져오기 
        if (IsEmpty(bagItem)) return;               // 2. 가방 아이템이 비어 있으면 장착 처리 중단 
        if (!IsEmpty(equipItems[equipIndex]))       // 3. 장착 슬롯에 이미 아이템이 있으면 막음
        { 
            Debug.Log("장착 슬롯이 이미 사용 중입니다."); // 4. 장착 슬롯에 못 넣는다고 체크!
            return;                                 // 5. 여기 사용 못해 돌아가!
        } 
        ItemData itemData = bagItem.data;           // 6. 가방 아이템의 아이템 데이터 가져오기

        // 7. 가방에 여러 개가 있어도 장착 슬롯에는 1개만 들어가도록 처리
        equipItems[equipIndex] = new InventoryItem(itemData, 1);  
        bagItem.count--;                // 8. 가방에서는 장착한 1개만큼 개수 감소 
        if (bagItem.count <= 0)         // 9. 0개가 되면 가방 슬롯 비움
        { 
            bagItems[bagIndex] = null;  // 10. 아이템 개수가 0 이하면 가방 슬롯을 빈 슬롯으로 변경
        }  
        Debug.Log(itemData.itemName + " 1개 장착"); // 11. 장착 성공 로그 출력
    }
    private bool IsEmpty(InventoryItem item)
    {
        // 2. 아이템이 없거나, 아이템 데이터가 없거나, 개수가 0 이하이면 빈 슬롯으로 판단
        return item == null || item.data == null || item.count <= 0;
    }
  
    private void MoveEquipItemToBag(int equipIndex, int bagIndex)
    {
        
        InventoryItem equipItem = equipItems[equipIndex];   // 1. 장비 슬롯에 있는 아이템 가져오기 
        if (IsEmpty(equipItem)) return;                     // 2. 장착 슬롯 아이템이 비어 있으면 돌아가!  
        InventoryItem bagItem = bagItems[bagIndex];         // 3. 이동할 가방 슬롯에 있는 아이템 가져오기

        if (IsEmpty(bagItem))                               // 4. 가방 슬롯이 비어 있으면 그대로 이동
        {
            // 5. 장착 슬롯의 아이템 새 InventoryItem으로 만들어 가방 슬롯에 넣기
            bagItems[bagIndex] = new InventoryItem(equipItem.data, equipItem.count);
            equipItems[equipIndex] = null;                  // 6. 장착 슬롯 비우기 
            return;                                         // 7. 이동 처리가 끝났으므로 함수 종료
        }
        if (bagItem.data == equipItem.data && bagItem.data.canStack)  // 8. 같은 아이템이면 개수 합치기
        {
            int space = bagItem.data.maxStack - bagItem.count;        // 9. 남은 가방 슬롯 공간 계산 
            int addCount = Mathf.Min(space, equipItem.count);         // 10. 남은 갯수 - 장착된 갯수 
            bagItem.count += addCount;                                // 11. 가방 아이템 개수 증가 
            equipItem.count -= addCount;                              // 12. 장착 아이템 개수 감소


            if (equipItem.count <= 0)                               // 13. 장착 아이템 개수가 0 이하면 
            {
                equipItems[equipIndex] = null;                      // 14. 장착 슬롯 비우기
            }

            Debug.Log($"{bagItem.data.itemName}가방에 {addCount}개 합침"); // 15. 합치기 성공 로그 출력

            return;                                                        // 16. 합치기 처리가 끝
        }
        

        // 다른 아이템이면 교환  
        InventoryItem temp = bagItems[bagIndex];    // 17. 가방 슬롯에 있던 아이템을 임시로 저장 

        // 18. 장착 슬롯 아이템을 새 InventoryItem으로 만들어 가방 슬롯에 넣기
        bagItems[bagIndex] = new InventoryItem(equipItem.data, equipItem.count);
        equipItems[equipIndex] = temp;          // 19. 임시로 저장한 가방 아이템을 장착 슬롯으로 이동  

    }


    // 1. 리스트가 존재하고, 인덱스가 0 이상이며, 리스트 범위 안에 있는지 확인
    private bool IsValidIndex(List<InventoryItem> list, int index)
    {
        return list != null && index >= 0 && index < list.Count;
    }

    public void RemoveOneBagItem(int bagIndex)
    {
        
        if (!IsValidIndex(bagItems, bagIndex)) return;  // 1. 전달받은 가방 슬롯 인덱스가 유효한지 확인 
        InventoryItem item = bagItems[bagIndex];        // 2. 해당 가방 슬롯에 있는 아이템 가져오기 

        if (item == null || item.data == null) return;  // 3. 슬롯에 아이템, 아이템 데이터 없음 돌아가 
        item.count--;                                   // 4. 슬롯 아이템 개수 1개 감소 
        Debug.Log($"{item.data.itemName} 1개 사용");    // 5. 아이템 1개 삭제 로그 출력
         
        if (item.count <= 0)                            // 6. 슬롯 아이템 개수가 0 이하가 되면
        { 
            bagItems[bagIndex] = null;                  // 7. 가방 슬롯을 빈 슬롯으로 변경
        }
    }
}
