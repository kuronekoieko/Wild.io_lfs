using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内で使う変数
/// UIに表示するときはUniRxで値を監視するのがおすすめ
/// ・Unityで学ぶMVPパターン ~ UniRxを使って体力Barを作成する ~
/// https://qiita.com/Nakashima_Hibari/items/5e0c36c3b0df78110d32
/// </summary>
public class Variables : MonoBehaviour
{
    public static ScreenState screenState;
    public static GameState gameState;
    public static float timer;
    public static PlayerProperty[] playerProperties;
    public static int playerCount;
    public static List<PlayerSize> playerSizes;
    public static bool isKilled;
}
