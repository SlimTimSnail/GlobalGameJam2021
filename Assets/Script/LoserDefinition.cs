using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "LoserDefinition", menuName = "GGJ2021/LoserDefinition")]

public class LoserDefinition : ScriptableObject
{
    public VideoClip Intro;
    public VideoClip Outro;
    public VideoClip Repeat;

    public VideoClip Idle_01;
    public VideoClip Idle_02;
    public VideoClip Idle_03;
    
    public VideoClip NotBlack;
    public VideoClip NotBlue;
    public VideoClip NotGreen;
    public VideoClip NotPink;
    public VideoClip NotWhite;
    public VideoClip NotYellow;
    public VideoClip NotSilver;

    public VideoClip NotElectronic;
    public VideoClip NotTool;
    public VideoClip NotToy;
    public VideoClip NotFood;
  
    public VideoClip TooBig;
    public VideoClip TooSmall;
}
