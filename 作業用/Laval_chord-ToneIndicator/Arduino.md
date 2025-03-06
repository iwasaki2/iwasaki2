デジタル入出力は20mhまで
→モータードライバを使う

東芝のTA7291Pは生産終了
L298Nモータードライバは、ON/OFFのみの制御のほかPWM制御により回転スピードを変えることも出来る。

High
Low

＜正転＞
制御信号A　High
制御信号B　Low

＜逆転＞
制御信号A　Low
制御信号B　High

＜ストップ＞
制御信号A　High
制御信号B　Low

＜ブレーキ＞
制御信号A　Low
制御信号B　Low

モータードライバ(L293D)でDCモーターを制御する
モータードライバーを使用することで、モーターの回転、反転、停止、回転数制御などを簡単に行えるようになる。

必要機材
・オスメスジャンパー
・シールドクリップ
・Arduino　Uno（互換品でも正規品と比べて特に問題なく使用でき数百円程度で購入が可能）
・ブレッドボード
・モーター


ダイオード
→逆向きの電流を防ぐ

トランジスタ
→電気の流れを制御することができる部品、回路上でスイッチの役割をしたり、電流を増幅する役割


（１）モータドライバ（MCD）へモータ駆動通電をOFFする。
※この時、通電を切ってもモータは慣性力で回るため、各U,V,W相コイルには発電された電荷が残っているため直ぐに停止しない。

（２）FET Q1、Q2、Q3をONして、
各相コイル端子をGNDに短絡（ショート）し、残存電荷をGNDへ流す。

このコイルの残存電荷を無くす制御によって、モータには電気ブレーキ制動が掛かり、ピタっと停止させることが出来ます。電気的に各相コイルをGNDへ短絡（接地ショート）させることから、「ショートブレーキ」と呼んでいます。


digitalWrite(IN1, LOW);とdigitalWrite(IN2, LOW);はショートブレーキの状態
この状態では、モーターの両端がGNDに接続され、回転が急速に停止する。
L293DのようなHブリッジモータードライバでは、以下のような状態が考えられます：

前進回転：
IN1 = HIGH, IN2 = LOW → モーターは正回転
逆回転：
IN1 = LOW, IN2 = HIGH → モーターは逆回転



https://qiita.com/yuuki122606/items/8540e5e2b173c06a658a

https://www.marutsu.co.jp/contents/shop/marutsu/datasheet/ta7291.pdf

https://ht-deko.com/arduino/esp-wroom-02.html

https://www.kyohritsu.jp/eclib/OTHER/DATASHEET/ta7291p.pdf

<img width="549" alt="スクリーンショット 2024-07-02 10 47 58" src="https://github.com/idesemi-weekly/ivrc2024-chord/assets/155946795/9fd25050-62b3-42ef-a892-964d62eb0ef3">

TA7291p・端子説明<img width="899" alt="スクリーンショット 2024-08-06 1 49 26" src="https://github.com/user-attachments/assets/1f1c30cd-e0be-4047-8379-8cb62a75aa0e">

ESP・WROOM02<img width="1113" alt="スクリーンショット 2024-08-06 1 51 30" src="https://github.com/user-attachments/assets/f73e4dcd-5266-4f6f-82da-6ca909c32e1b">


