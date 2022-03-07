# VirtualGamepad

ViGEm.NETをUnityで使用できるようにしている。

表示のためのGUIをPrefabにしている。

https://github.com/ViGEm/ViGEm.NET


## 使用方法

下記ページよりViGEmのインストールを行う。

https://github.com/ViGEm/ViGEmBus/releases

Unityを起動して実行

```cs
    [SerializeField] ViGEmGamePad gamePad = null;

    // Update is called once per frame
    void Update()
    {
        float v = Time.time;
        gamePad.LeftThumb = new Vector2( Mathf.Sin(v), Mathf.Cos(v));
        gamePad.RightThumb = new Vector2( -Mathf.Cos(v), -Mathf.Sin(v));

        gamePad.RightTrigger = (Mathf.Sin(v) + 1) / 2f;

        gamePad.BtnA = Mathf.Abs(Mathf.Sin(v)) > 0.5f;
    }
```

コンパネのゲームパッドに Controller (XBOX 360 For Windows) と表示され、上記入力値が反映される。


## OSC

Package Manager で Scoped Registry の登録

Name: hecomi  
URL: https://registry.npmjs.com  
Scope: com.hecomi  

https://github.com/hecomi/uOSC
