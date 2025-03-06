// LEDを接続するピン番号を定義
const int ledPin0 = 0;
const int grPin2 = 2;

void setup() {
    // 各ピンを出力モードに設定
    pinMode(ledPin0, OUTPUT);
    pinMode(grPin2, OUTPUT);
}

void loop() {
    // LED1を点灯して待つ
    digitalWrite(ledPin0, HIGH);
    delay(500);
    digitalWrite(ledPin0, LOW);
    
    // LED2を点灯して待つ
    digitalWrite(grPin2, HIGH);
    delay(500);
    digitalWrite(grPin2, LOW);
    
    // 点滅が終わった後、0,1秒待ち
    delay(100);
}
