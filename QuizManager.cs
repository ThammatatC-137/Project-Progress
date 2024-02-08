using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Linq;
using TMPro;

// นี้คือคลาส QuizManager ที่ใช้สำหรับจัดการกับการทำงานของเกมทายคำ
public class QuizManager : MonoBehaviour
{
    // ตัวแปร instance เป็น static เพื่อให้สามารถเข้าถึงได้จากทุกที่โดยไม่ต้องสร้างออบเจ็กต์ใหม่
    public static QuizManager instance;

    // ตัวแปรที่ใช้เก็บข้อมูลคำถามและตัวเลือก
    [SerializeField]
    private QuizDataScriptable questionData;
    [SerializeField]
    private Image questionImage;
    [SerializeField]
    private WordData[] answerWordArray;
    [SerializeField]
    private WordData[] optionsWordArray;

    // ตัวแปรสำหรับการตรวจสอบคำตอบและตำแหน่งปัจจุบันในคำถาม
    private int currentAnswerIndex = 0;
    private bool correctAnswer = true;
    private List<int> selectedWordIndex;
    private int currentQuestionIndex;
    private string answerWord;

    // ตัวแปรที่ใช้เก็บ GameObject ที่แสดงข้อความเมื่อเกมสำเร็จ
    [SerializeField]
    private GameObject gameCompleteObject;

    // ฟังก์ชัน Awake เรียกตอนที่ Object ถูกสร้างขึ้น
    private void Awake()
    {
        // ตรวจสอบว่า instance ยังไม่ถูกกำหนดหรือไม่
        if (instance == null)
            instance = this; // กำหนด instance เป็นตัวปัจจุบัน
        else
            Destroy(this.gameObject); // ถ้ามี instance อื่นอยู่แล้ว ให้ทำลายตัวเอง
        selectedWordIndex = new List<int>(); // สร้าง List เพื่อเก็บตำแหน่งของคำตอบที่เลือกไว้
    }

    // ฟังก์ชัน Start เรียกตอนเริ่มเกม
    private void Start()
    {
        // ตรวจสอบว่ายังมีคำถามในรายการหรือไม่
        if (currentQuestionIndex < questionData.questions.Count)
        {
            SetQuestion(); // กำหนดคำถามแรก
        }
        else
        {
            Debug.LogWarning("No more questions available."); // ถ้าไม่มีให้แสดงข้อความเตือน
        }
    }

    // ฟังก์ชัน SetQuestion ใช้สำหรับกำหนดคำถามและตัวเลือก
    private void SetQuestion()
    {
        currentAnswerIndex = 0; // รีเซ็ตตัวแปรที่ใช้เก็บคำตอบ
        selectedWordIndex.Clear(); // ล้าง List ของตำแหน่งคำตอบที่เลือกไว้
        questionImage.sprite = questionData.questions[currentQuestionIndex].questionImage; // กำหนดภาพถาม
        answerWord = questionData.questions[currentQuestionIndex].answer; // กำหนดคำตอบ

        ResetQuestion(); // รีเซ็ตคำถามและตัวเลือก
        char[] charArray = new char[optionsWordArray.Length]; // สร้างอาร์เรย์ของตัวอักษร

        // ตรวจสอบคำตอบและเติมตัวอักษรลงในอาร์เรย์
        for (int i = 0; i < answerWord.Length; i++)
        {
            charArray[i] = char.ToUpper(answerWord[i]);
        }

        // เติมตัวอักษรสุ่มในช่องว่าง
        for (int i = answerWord.Length; i < optionsWordArray.Length; i++)
        {
            charArray[i] = (char)UnityEngine.Random.Range(65, 91);
        }

        // สับเปลี่ยนตำแหน่งของตัวอักษร
        charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();

        // กำหนดตัวอักษรในตัวเลือก
        for (int i = 0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].SetWord(charArray[i]);
        }
    }

    // ฟังก์ชัน SelectedOption ใช้สำหรับเลือกตัวเลือกและตรวจสอบคำตอบ
    public void SelectedOption(WordData wordData)
    {
        if (currentAnswerIndex >= answerWord.Length || currentQuestionIndex >= questionData.questions.Count)
        {
            return; // ถ้าคำตอบเต็มหรือไม่มีคำถามแล้ว ออกจากฟังก์ชัน
        }

        selectedWordIndex.Add(wordData.transform.GetSiblingIndex()); // เก็บตำแหน่งของตัวเลือก
        answerWordArray[currentAnswerIndex].SetWord(wordData.wordValue); // แสดงตัวเลือกที่เลือกไว้
        wordData.gameObject.SetActive(false); // ซ่อนตัวเลือก
        currentAnswerIndex++; // เพิ่มจำนวนตัวอักษรที่เลือก

        if (currentAnswerIndex >= answerWord.Length)
        {
            // ตรวจสอบคำตอบ
            correctAnswer = true;
            for (int i = 0; i < answerWord.Length; i++)
            {
                if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordArray[i].wordValue))
                {
                    correctAnswer = false;
                    break;
                }
            }
            if (correctAnswer) // ถ้าคำตอบถูกต้อง
            {
                Debug.Log("Correct");
                currentQuestionIndex++; // เลื่อนไปคำถามถัดไป
                if (currentQuestionIndex < questionData.questions.Count)
                {
                    Invoke("SetQuestion", 0.5f); // เริ่มคำถามใหม่
                }
                else
                {
                    Debug.LogWarning("You won");
                    gameCompleteObject.SetActive(true); // แสดงข้อความเมื่อเกมสำเร็จ
                    Time.timeScale = 0; // หยุดเวลา
                }
            }
            else
            {
                Debug.Log("Incorrect"); // ถ้าคำตอบผิด
            }
        }
    }

    // ฟังก์ชัน ResetQuestion ใช้สำหรับรีเซ็ตคำถามและตัวเลือก
    private void ResetQuestion()
    {
        // รีเซ็ตตัวเลือกที่ผู้เล่นเลือกไว้
        for (int i = 0; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(true);
            answerWordArray[i].SetWord('_');
        }

        // ซ่อนตัวเลือกที่ไม่ถูกเลือก
        for (int i = answerWord.Length; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(false);
        }
        // แสดงตัวเลือกที่ให้ผู้เล่นเลือก
        for (int i = 0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].gameObject.SetActive(true);
        }
    }

    // ฟังก์ชัน ResetLastWord ใช้สำหรับรีเซ็ตตัวเลือกที่ผู้เล่นเลือกล่าสุด
    public void ResetLastWord()
    {
        if (selectedWordIndex.Count > 0)
        {
            int index = selectedWordIndex[selectedWordIndex.Count - 1];
            optionsWordArray[index].gameObject.SetActive(true);
            selectedWordIndex.RemoveAt(selectedWordIndex.Count - 1); // ลบตำแหน่งที่เลือกออกจาก List
            currentAnswerIndex--; // ลดจำนวนตัวอักษรที่ผู้เล่นเลือกไว้
            answerWordArray[currentAnswerIndex].SetWord('_'); // รีเซ็ตตัวอักษรที่แสดง
        }
    }
}


[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}
