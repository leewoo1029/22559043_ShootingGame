using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charactor : MonoBehaviour
{
    public int maxHp = 100;
    int currentHp;

    public Slider hpSlider; // UI 슬라이더 연결

    void Start()
    {
        currentHp = maxHp;

        // 초기 UI 설정
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }

    // 몬스터와 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("Damage");
            TakeDamage(20); // 데미지 20
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        currentHp -= damage;

        // 체력 0 이하 방지
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        Debug.Log("현재 체력: " + currentHp);

        // UI 업데이트
        hpSlider.value = currentHp;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("캐릭터 사망");
        Destroy(gameObject);
    }
}