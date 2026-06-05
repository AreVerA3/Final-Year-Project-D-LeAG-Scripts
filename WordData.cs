using System.Collections.Generic;
using UnityEngine;

// 1. This holds the actual data for ONE single word
[System.Serializable]
public class SingleWord
{
    public string fullWord;
    public string displayWord;
    public string correctLetter;
    public Sprite wordPicture;
    public AudioClip wordAudio; 
}

// 2. This is the asset file that holds the list of words for a level
[CreateAssetMenu(fileName = "New Word Data", menuName = "Word Data Bank")]
public class WordData : ScriptableObject
{
    public List<SingleWord> words;
}