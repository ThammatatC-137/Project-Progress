using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// นี่คือคลาส ShuffleList ที่เป็น abstract class
public abstract class ShuffleList
{
    // เมธอดสำหรับสับเปลี่ยนลำดับของรายการ
    public static List<E> ShuffleListItems<E>(List<E> inputList)
    {
        // สร้างรายการใหม่เพื่อเก็บค่าต้นฉบับ
        List<E> originalList = new List<E>();
        originalList.AddRange(inputList);

        // สร้างรายการใหม่เพื่อเก็บค่าที่สุ่มมา
        List<E> randomList = new List<E>();

        // สร้างอ็อบเจ็กต์ของคลาส System.Random เพื่อใช้สุ่มตำแหน่ง
        System.Random r = new System.Random();
        int randomIndex = 0;

        // สุ่มและเพิ่มสมาชิกในรายการใหม่
        while (originalList.Count > 0)
        {
            randomIndex = r.Next(0, originalList.Count); // เลือกสมาชิกที่สุ่มมาในรายการเดิม
            randomList.Add(originalList[randomIndex]); // เพิ่มสมาชิกนั้นเข้าไปในรายการใหม่
            originalList.RemoveAt(randomIndex); // ลบสมาชิกนั้นออกจากรายการเดิมเพื่อหลีกเลี่ยงการซ้ำซ้อน
        }

        return randomList; // ส่งคืนรายการใหม่ที่สุ่มแล้ว
    }
}
