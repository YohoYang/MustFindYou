using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumSpace
{
    /// <summary>
    /// 控制屏幕正常显示还是变暗
    /// </summary>
    public enum PanelState
    {
        None,
        Fade,
        Show
    }
    /// <summary>
    /// 选择当前需要改变状态的的屏幕
    /// </summary>
    public enum PanelType
    {
        Left,
        Right
    }
    public enum MoveDir
    {
        W,
        A,
        S,
        D
    }

}
