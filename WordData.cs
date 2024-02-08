using UnityEngine;
using UnityEngine.UI;

public class WordData : MonoBehaviour
{
    [SerializeField] private Text wordText; // ตัวแปรสำหรับเก็บข้อความที่แสดงบนปุ่ม
    [HideInInspector] // กำหนดให้ wordValue ไม่แสดงใน Inspector ของ Unity
    public char wordValue; // ตัวแปรสำหรับเก็บค่าตัวอักษร

    private Button buttonComponent; // ตัวแปรสำหรับเก็บคอมโพเนนต์ปุ่ม

    private void Awake()
    {
        buttonComponent = GetComponent<Button>(); // ดึงคอมโพเนนต์ปุ่มจาก GameObject นี้
        if (buttonComponent)
        {
            // ถ้ามีคอมโพเนนต์ปุ่ม กำหนดให้เมื่อปุ่มถูกคลิก ให้เรียกฟังก์ชัน WordSelected()
            buttonComponent.onClick.AddListener(() => WordSelected());
        }
    }

    // ฟังก์ชันที่ใช้ในการกำหนดข้อความและค่าตัวอักษร
    public void SetWord(char value)
    {
        wordText.text = value + ""; // กำหนดข้อความที่แสดงบนปุ่ม
        wordValue = value; // กำหนดค่าตัวอักษร
    }

    // ฟังก์ชันที่เรียกเมื่อปุ่มถูกคลิก
    private void WordSelected()
    {
        Debug.Log("WordSelected() is called."); // แสดงข้อความบนคอนโซล
        Debug.Log("This object's instance ID: " + this.GetInstanceID()); // แสดง Instance ID ของ Object นี้บนคอนโซล
        QuizManager.instance.SelectedOption(this); // เรียกฟังก์ชัน SelectedOption ใน QuizManager โดยส่งตัวเอง (WordData) เป็นพารามิเตอร์
    }
}
