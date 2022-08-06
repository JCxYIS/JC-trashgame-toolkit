
# JC-trashgame-toolkit

https://github.com/JCxYIS/JC-trashgame-toolkit

> My Unity toolbox, which contains convenient scripts/prefabs for instant use  
> 把一些常用的東西放起來 這樣就不用一直重寫  

> Note: many assets of this pack is gather from the web, please refer to the references below.  

## Installation

### 首次使用
Navigate to unity project directory
```
cd Assets
git submodule add https://github.com/JCxYIS/JC-trashgame-toolkit
```

### 重新加入
```
git submodule init
git submodule update
```

## Shipped Plugins
This package is shipped with 
- [DOTween Free](http://dotween.demigiant.com/), you may need to setup first.
- [LitJson](https://litjson.net/)  

Check if ur project conflict with 'em

This package also requires Addressables Package. Install it from the package manager.



<!-- ---------------------------------------------------------- -->

## Installation
- In Unity, open the Package Manager
- Click "+" button on the top left
- Select "add package from git URL"
- `https://github.com/JCxYIS/JC-trashgame-toolkit.git`
- Click add. Done.

## :game_die: Prefabs
### ProgressBar ==TODO==
血條/進度條 

<!-- --- -->

### PromptBox
彈出式提示/確認框

![](https://i.imgur.com/z7nWbkc.png)
![](https://i.imgur.com/F6vJFE5.png)


```csharp
PromptBox.CreateMessageBox("Hello");
```

選項不需要全部都填，可以只填需要的
```csharp
PromptBox.Create(new PromptBoxSettings{
    Content = "8+7 等於幾？？",
    ConfirmButtonText = "15",
    ConfirmCallback = () => {
        PromptBox.CreateMessageBox("答對了!");
    },
    CancelButtonText = "48763",
    CancelCallback = () => {
        PromptBox.CreateMessageBox("答錯了");
    },
});
```

### LeaderboardService
使用 globalstats.io 作為排行榜服務
使用前需要先呼叫 `Init()` 傳入 API Access token 與 Username 等資訊。

```
```


### Loading Screen
全屏讀取視窗

![](https://i.imgur.com/RI21R8f.png)

```csharp
IEnumerator GoSceneCoroutine(string sceneName)
{
    // do load
    var loadGame = SceneManager.LoadSceneAsync(sceneName);
    loadGame.allowSceneActivation = false;
    while(!loadGame.isDone) 
    {
        LoadingScreen.SetContext("Loading Scene...");
        LoadingScreen.SetProgress(loadGame.progress);

        if(LoadingScreen.IsFullyShown)
        {
            loadGame.allowSceneActivation = true;
        }
        yield return null;
    }
    
    // do some scene init here
    LoadingScreen.SetProgress(1); // hide the loading screen
}
```

<!-- --- -->





<!-- ---------------------------------------------------------- -->





## :video_game: Components

### UguiTextSpaceReplacer
把 Unity UGUI 的 Text 替換空白 (` `) 成 `\u00A0`，避免自動換行

### CheatCode
綁定鍵位，輸入密技以執行某些動作。

e.g. 下圖：遊戲中連續輸入密技 JCCOOL 就會觸發底下 OnActivate  
![](https://i.imgur.com/cJOMRre.png)  


<!-- --- -->

## :computer: Scripts 

### Singleton
很常見的單例設計典範

```csharp
public class XXXManager : MonoSingleton<GameManager>
{
    //...
}
```
```csharp
XXXManager.Instance.DoSomething();
```

> Ref: https://gist.github.com/onevcat/6025819


<!-- --- -->

### WebApiHelper
方便呼叫 web api (e.g. GET, POST)。
可以先進 WebApiHelper 設定 ROOT_URL，之後使用相對路徑來叫就 OK (e.g. 如果下面範例 ROOT_URL 設定 `https://reqres.in/api`，則底下網址填 `users` 就好) 

```csharp
WebApiHelper.CallApi<List<ReqresUser>>("GET", "https://reqres.in/api/users", (succ, msg, data)=>{
    if (succ)
    {
        string s = "Total: " + data.Count + "Users\n";
        foreach (var user in data)
        {
            s += user.first_name + " " + user.last_name + "\n";
        }
        PromptBox.CreateMessageBox(s);
    }
    else
    {
        PromptBox.CreateMessageBox("Error: "+msg);
    }
});
```



<!-- --- -->

### ReadOnly Attribute
讓欄位在 Inspector 顯示，但是不讓你在上面修改。

```csharp
[ReadOnly] public GameObject SomeObject;
```

> Ref: https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/

<!-- --- -->

### List.Random / List.RandomN
從 List 隨機抽出一或多個元素。

```csharp
List<int> list = new List<int>{1, 2, 3, 4, 5};
var rand1 = list.Random();  // 4
var rand3 = list.RandomN(3); // 3, 1, 4
var rand3repeat = list.RandomN(3, true); // 5, 2, 2
```

### List.Shuffle
將一個 List 打亂

```csharp
List<int> list = new List<int>{1, 2, 3, 4, 5};
list.Shuffle();  // 4, 1, 3, 2, 5
```


<!-- --- -->

### Pop UI
利用繼承 PopUI 這個 class 來實作彈出式 UI 面板。

UI 階層如下所示 (或是你可以直接去翻裡面的 Promptbox)
![](https://i.imgur.com/vADeWRo.png)

要使用時，直接呼叫裡面的 `Show()` 和 `Hide()` 就行了

注意：要使用 `Awake()`, `Update()`、或要覆寫 `Show()`, `Hide()`時，需要使用 `public override ...()`並回調 `base`，e.g. 

```csharp
public override void Show()
{
    // Do something here
    // ...
    base.Show();
}
```


<!-- ---------------------------------------------------------- -->


## :frame_with_picture: Sprites

### RadialSprite
圓形或方形。可以用在讀取進度之類的，因為很常用到就擺進來了

### GradientSprite
漸層圖樣，因為很常用到就擺進來了


---

## Also check these cool stuffs
- 編輯器擴充插件 https://github.com/dbrizov/NaughtyAttributes

