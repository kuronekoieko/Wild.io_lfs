using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create PlayerSettingSO", fileName = "PlayerSettingSO")]
public class PlayerSettingSO : ScriptableObject
{
    public PlayerSetting[] playerSettings;

    private static PlayerSettingSO _i;
    public static PlayerSettingSO i
    {
        get
        {
            string PATH = "ScriptableObjects/" + nameof(PlayerSettingSO);
            //初アクセス時にロードする
            if (_i == null)
            {
                _i = Resources.Load<PlayerSettingSO>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if (_i == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _i;
        }
    }
}

[System.Serializable]
public class PlayerSetting
{
    public Color color;
    public string name;
}