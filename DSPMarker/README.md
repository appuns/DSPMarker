# DSP Marker
Allows you to place markers to indicate the position of something. There are guide arrows that indicates the direction of the marker. You can automatically move to the position of a marker.<br>
This don't affect achievements or milestones.<br>

施設等の位置を示すマーカーを配置できます。マーカーの方向を示すガイド矢印を表示することができます。マーカーの位置に自動的に移動できます。<br>
実績やマイルストーンには影響しません。<br>
<br>
## Features 特徴
There is a marker button in the upper right corner of the screen.<br>
Below that is a list of markers.<br>
You can switch the display of the markers and marker list and guide arrows by clicking the marker button.<br>
You can switch between edit mode and guide mode by right-clicking on the marker button.<br>
In edit mode, clicking on the marker list opens ”Marker Editor” and you can place new markers or edit existing markers.<br>
The marker will be placed above the player's position at the time of creation.<br>
In guide mode, you can automatically move to the marker position by clicking the marker list. Click again to cancel the move.<br>

In "Marker Editor", there are three designs.
1. Only one icon
2. Only one string
3. Two icons and one string

The design will change automatically depending on what you have set.<br>
You can start a new line in a string.

Each marker has three options.
1. Always displayed(Even if the marker goes off the screen, it will be displayed on the edge of the screen.)
2. Seen through the planet(Display markers through the planet.)
3. Show the Arrow Guide(Displays an arrow guide that indicates the direction of the marker.)

画面右上にマーカーボタンがあります。<br>
その下にマーカーの一覧があります。<br>
マーカーボタンをクリックすることでマーカーの一覧とマーカー自体、ガイド矢印の表示を切り替えることができます。<br>
マーカーボタンを右クリックすると、編集モードとガイドモードを切り替えることができます。<br>
編集モードではマーカーリストをクリックすることで、編集ウインドウが開き、新しいマーカーを設置したり、既存のマーカーを編集することができます。<br>
マーカーは作成時のプレーヤーの位置の上空に設置されます。<br>
ガイドモードではマーカーリストをクリックすることで、マーカーの位置まで自動で移動することができます。もう一度クリックすると移動をキャンセルします。<br>

編集ウインドウでは、マーカーの３つのデザインがあります。
1. １つのアイコンだけ
2. １つの文字列だけ
3. ２つのアイコンと１つの文字列

設定したものによって自動でデザインが変わります。<br>
文字列内で改行することができます。<br>

それぞれのマーカーには３つのオプションがあります。
1. 常に表示する（マーカーが画面の外に出ても、画面の端に表示します。）
2. 惑星を透過する（惑星を透過してマーカーを表示します。）
3. 矢印ガイドを表示する（マーカーの方向を表す矢印ガイドを表示します。）

## settings 設定
You can change the settings in the config file.steamapps/common/Dyson Sphere Program/config/Appun.DSP.plugin.Marker.cfg
- Hide the key tips that appear on the right side of the screen. (DisableKeyTips:default true)
- Maximum number of markers (maxMarker:default 20)

コンフィグファイルで設定を変更することができます。steamapps/common/Dyson Sphere Program/config/Appun.DSP.plugin.Marker.cfg
- キーチップスを非表示にする（DisableKeyTips:デフォルト true）
- 表示するマーカーの数（maxMarker:デフォルト 20）

## How to install インストール方法
1. Install BepInEx<br>
2. Drag DSPMarker.dll into steamapps/common/Dyson Sphere Program/BepInEx/plugins<br>

It's easier to use the mod manager "r2modman".

1. BepInExをインストールします。<br>
2. DSPMarker.dllをsteamapps/common/Dyson Sphere Program/BepInEx/pluginsに配置します。<br>

MODマネージャの「r2modman」を使うと簡単です。

## Contact 問い合わせ先
If you have any problems or suggestions, please contact DISCORD MSP Modding server **Appun#8284** or create an issue in the repository in Github.<br>
不具合、改善案などありましたら、DISCORD「DysonSphereProgram_Jp」サーバー**Appun#8284**に連絡、または、GitHubの該当リポジトリでIssueを作成してください。<br>

## Change Log 更新履歴
### v0.0.3
- By clicking the marker button, you can now toggle the display of markers and guide arrows as well as the marker list.マーカーボタンをクリックすることで、マーカーリストだけでなく、マーカーやガイド矢印の表示も切り替えられるようになりました。
- Automatic movement can now be canceled by clicking on the marker list.マーカーリストをクリックすることで自動移動をキャンセルできるようになりました。
- Confirmed that it is compatible with game version 0.9.25.12077. ゲームバージョン0.9.25.12077に対応していることを確認しました。
### v0.0.3
- Corrected the this description. この説明文を修正しました。
### v0.0.2
- Released リリースしました。
- Published in GitHub. GitHubに公開しました。
