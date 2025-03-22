# Bakalárska práca
Táto aplikácia bola vytvorená na akademické účely pre moju bakalársku prácu. Hra je robená na štýl Endless driving hier (prostredie sa generuje donekonečna). Zámerom tejto aplikácie je poukázať na možnosti ovládania objektov v unity pomocou mikrokontroléra Arduino, kde ovládaný objekt je v našom prípade hráčovo auto pomocou potenciometra a dvoch tlačidiel. Všetky assety boli zo stránky https://itch.io a boli využité čisto len na akademické účely.
## Assety
https://kaylousberg.itch.io/city-builder-bits -- https://kaylousberg.itch.io/kaykit-medieval-hexagon -- https://justcreate3d.itch.io/low-poly-megapolis-pack -- https://iamsujitcu.itch.io/3d-coin
## Využité knižnice
UnityEngine, UnityEngine.SceneManagement, System.Collections, System.IO.Ports, System.Threading, TMPro, UnityEngine.UI
## Nevyhnutné funkcie pre spojenie s Arduinom
**1.** Pomocou .NET Frameworku sa využíva knižnica **System.IO.Ports** a z nej trieda **SerialPort**, ktorá sa inicializuje zadaním COM portu v našom prípade MacOS: /dev/cu.usbmodemXXXX na Windowse: reťazce typu COMX. Nevyhnutné je aj zadanie Baud rate, ideálne aby bol rovnaký ako v Arduine, čiže v našom prípade 9600. A pomocou **SerialPort seriovyPort.Open()** sa otvorí spojenie.

**2.** S využitím knižnice **System.Threading** a z nej triedy **Thread** sme zabezpečili samostatné vlákno, konkrétne **citacieVlakno = new Thread(citajSerial);** **citacieVlakno.Start();**. V tomto vlákne beží neustále čítanie riadkov pomocou **ReadLine()** a dáta sa ukladajú do premennej, z ktorej sa neskôr v **Update()** spracujú a transformujú do vstupov pre hru.
## Minimálne Požiadavky
- Unity editor(verzia 6000.0.25f1) s podporou .NET 4.x, stiahnuť tu: https://unity.com/download
- Mikrokontrolér Arduino
- Arduino IDE 2.3.x, stiahnuť tu: https://www.arduino.cc/en/software/
- Breadboard doska, 12x prepojovací kábel(individuálne podľa dĺžky káblov), 1x potenciometer, 2x tlačidlo, napájací kábel Arduina
# Návod na spustenie
## 1. Schéma zapojenia ovládača
<img width="1110" alt="Image" src="https://github.com/user-attachments/assets/84d4dc79-bf4f-444a-b7ae-3162edc3cf82" />

## 2. Sketch do Arduina
```ruby
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
```
## 3. Upload na Arduino dosku
Pripojte vyskladaný ovládač podla schémy ku svojmu počítaču, spustite program Arduino IDE a vyberte v ňom Váš port na ktorom máte pripojené arduino, stlačte šípku vľavo od výberu portu, kód by sa mal úspešne nahrať na Vaše Arduino.

