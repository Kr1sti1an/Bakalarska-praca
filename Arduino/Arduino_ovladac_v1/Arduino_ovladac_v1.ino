void setup() {
  Serial.begin(9600);
  pinMode(3, INPUT); // Ľavé tlačidlo (dopredu)
  pinMode(2, INPUT); // Pravé tlačidlo (brzda)
}

void loop() {
  int potValue = analogRead(A0); // Čítanie potenciometra (0 - 1023)
  bool forwardButton = !digitalRead(3); // Čítanie stavu ľavého tlačidla
  bool brakeButton = !digitalRead(2);   // Čítanie stavu pravého tlačidla

  // Posielanie dát vo formáte: "potValue,forwardButton,brakeButton"
  Serial.print(potValue);
  Serial.print(",");
  Serial.print(forwardButton);
  Serial.print(",");
  Serial.println(brakeButton);

  delay(50); // Malé oneskorenie pre stabilnejšiu komunikáciu
}