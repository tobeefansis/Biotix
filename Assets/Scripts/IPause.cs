﻿using UnityEngine;
using System.Collections;

public interface IPause
{
    void Pause();
    void Resume();
    bool IsPause { get; set; }    
}