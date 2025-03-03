const int motor0 = 0;  // IN1 ピン
const int motor2 = 2;  // IN2 ピン

void setup() {
    // モータードライバーの制御ピンを出力モードに設定
    pinMode(motor0, OUTPUT);
    pinMode(motor2, OUTPUT);
}

void loop() {
    // 前進回転
    digitalWrite(motor0, HIGH);
    digitalWrite(motor2, LOW);
    delay(3000);  // 3秒間待つ

    // 停止
    digitalWrite(motor0, HIGH);
    digitalWrite(motor2, HIGH);
    delay(1000);  // 2秒間停止

    // 逆転
    digitalWrite(motor0, LOW);
    digitalWrite(motor2, HIGH);
    delay(3000);  // 5秒間逆回転

      // 停止
    digitalWrite(motor0, LOW);
    digitalWrite(motor2, LOW);
    delay(1000);  // 2秒間停止
}
