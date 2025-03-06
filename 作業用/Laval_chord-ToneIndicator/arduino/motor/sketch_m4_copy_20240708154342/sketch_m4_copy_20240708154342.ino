int motor0 = 13; // GPIO13を使用
int motor2 = 14; // GPIO14を使用
int motor4 = 4;
int motor5 = 5;
byte command; // byte 型でコマンドを受け取る

void setup() {
  Serial.begin(115200); // シリアル通信を開始
  pinMode(motor0, OUTPUT);
  pinMode(motor2, OUTPUT);
  pinMode(motor4, OUTPUT);
  pinMode(motor5, OUTPUT);
  digitalWrite(motor0, LOW); // 初期状態をモーターフリーモードに設定
  digitalWrite(motor2, LOW);
  digitalWrite(motor4, LOW); // 初期状態をモーターフリーモードに設定
  digitalWrite(motor5, LOW);
}

void loop() {
  if (Serial.available() > 0) {
    command = Serial.read(); // シリアルからコマンドを byte 型として読み取る
    if (command == 1) {
      // motor4とmotor5を先に動かす
      digitalWrite(motor4, LOW);
      digitalWrite(motor5, HIGH);
      
      // 遅延を追加 (例: 500ミリ秒)
      delay(200);

      // motor0とmotor2を遅れて動かす
      digitalWrite(motor0, LOW);
      digitalWrite(motor2, HIGH);

      // 0.5秒後に全てのモーターを停止
      delay(500);
      digitalWrite(motor4, LOW);
      digitalWrite(motor5, LOW);
      
      delay(500);
      digitalWrite(motor0, LOW);
      digitalWrite(motor2, LOW);
     
      
    } else if (command == 0) {
      // 全てのモーターを停止
      digitalWrite(motor0, LOW);
      digitalWrite(motor2, LOW);
      digitalWrite(motor4, LOW);
      digitalWrite(motor5, LOW);
    }
  }
}
