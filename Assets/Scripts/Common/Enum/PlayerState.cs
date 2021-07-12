using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerColor
{
    Red,
    Blue,
    SkyBlue,
    LightGreen,
    Gray,
    Black
};

public enum CharaType
{
    VirtualGuy,
    Prince,
    Devil
}

public enum SkillType
{
    PushHand,
    SelfExplosion,
}

public static class ExtensionMethod
{
    public static Color PlayerColorToColor( this PlayerColor player_color)
    {
        switch( player_color)
        {
            case PlayerColor.Red:
                return new Color(1, 0, 0);

            case PlayerColor.Blue:
                return new Color(0, 0, 1);

            case PlayerColor.Black:
                return new Color(0, 0, 0);

            case PlayerColor.Gray:
                return new Color(0.45f, 0.45f, 0.45f);

            case PlayerColor.SkyBlue:
                return new Color(0.529f, 0.807f, 0.921f);

            case PlayerColor.LightGreen:
                return new Color(0.564f, 0.933f, 0.564f);

        }

        return new Color(1, 1, 1);
    }
}