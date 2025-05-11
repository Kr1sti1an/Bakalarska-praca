# Bakalárska práca
Táto aplikácia bola vytvorená na akademické účely pre moju bakalársku prácu. Hra je robená na štýl Endless driving hier (prostredie sa generuje donekonečna). Zámerom tejto aplikácie je poukázať na možnosti ovládania objektov v unity pomocou mikrokontroléra Arduino, kde ovládaný objekt je v našom prípade hráčovo auto pomocou potenciometra a dvoch tlačidiel. Všetky assety boli zo stránky https://itch.io a boli využité čisto len na akademické účely. Pri tvorbe hry boli využité poznatky zo štúdia a niektoré prvky v hre boli inšpirované od tvorcu tutoriálov: https://www.patreon.com/c/prettyflygames/home
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
<img width="1022" alt="Image" src="https://github.com/user-attachments/assets/345928ba-6bb1-45de-b7a2-21d47abcf1f6" />

## 2. Sketch do Arduina
```ruby
void setup() {
  Serial.begin(9600);
  pinMode(3, INPUT_PULLUP); // Ľavé tlačidlo (dopredu)
  pinMode(2, INPUT_PULLUP); // Pravé tlačidlo (brzda)
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
- Pripojte vyskladaný ovládač podla schémy ku svojmu počítaču -> spustite program Arduino IDE a vyberte v ňom Váš port na ktorom máte pripojené arduino -> stlačte šípku vľavo od výberu portu -> kód by sa mal úspešne nahrať na Vaše Arduino.
<img width="373" alt="Image" src="https://github.com/user-attachments/assets/ab7a6656-5358-46cf-b129-ff399cebc61f" />

- Následne nastavte Baud rate na 9600.

<img width="567" alt="Image" src="https://github.com/user-attachments/assets/7a707ddc-5a30-43a6-afa4-ba0bdd06d699" />

- V Serial Monitore by ste mali vidieť toto:
<img width="351" alt="Image" src="https://github.com/user-attachments/assets/24528fd5-d427-4d7d-aca0-a26f3a2f52cf" />

- Po úspešnom nahratí na Arduino je vhodné Arduino IDE vypnúť, aby sa predišlo rušeniu komunikácie s Unity.(Arduino IDE môže byť uprednostnené a v Unity Vám nebude fungovať Arduino).

## 4. Pridanie hry do Unity
- Spustite si Váš Unity Hub, a uistite sa že máte nainštalovanú verziu editora **6000.0.25f1**
- Naklonujte alebo downloadnite si .zip súbor Unity hry
- V Unity Hub pridajte tento projekt
- Po otvorení hry v editore nájdite v **scéne Hra** v hierarchií na ľavo herný objekt s názvom **ArduinoInputManager**
- V tomto objekte nastavte v inšpektori na pravo Váš názov portu (ten ktorý ste využili pri nahrávani sketchu do Arduina) a Baudovú rýchlosť (Baud rate)
<img width="316" alt="Image" src="https://github.com/user-attachments/assets/dd352163-72c9-4541-8af3-5376a0bac1a9" />

- Kliknite na Play
- Po spustení hry v scéne by sa Vám malo na konzole dole vypísať: "Arduino ovládač úspešne pripojený!"
- Užite si hru!

## Youtube Video Link:
https://youtube.com/shorts/qMUF2x0hr3g?si=o-k-hLWOig5WbyEJ
