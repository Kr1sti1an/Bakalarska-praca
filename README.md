# Bakalárska práca
Táto aplikácia bola vytvorená na akademické účely pre moju bakalársku prácu. Hra je robená na štýl Endless driving hier (prostredie sa generuje donekonečna). Zámerom tejto aplikácie je poukázať na možnosti ovládania objektov v unity pomocou mikrokontroléra Arduino, kde ovládaný objekt je v našom prípade hráčovo auto pomocou potenciometra a dvoch tlačidiel. Všetky assety boli zo stránky https://itch.io a boli využité čisto len na akademické účely.
## Assety
https://kaylousberg.itch.io/city-builder-bits -- https://kaylousberg.itch.io/kaykit-medieval-hexagon -- https://justcreate3d.itch.io/low-poly-megapolis-pack -- https://iamsujitcu.itch.io/3d-coin
## Využité knižnice
UnityEngine, UnityEngine.SceneManagement, System.Collections, System.IO.Ports, System.Threading, TMPro, UnityEngine.UI
## Nevyhnutné funkcie
**1.** Pomocou .NET Frameworku sa využíva knižnica **System.IO.Ports** a z nej trieda **SerialPort**, ktorá sa inicializuje zadaním COM portu v našom prípade MacOS: /dev/cu.usbmodemXXXX na Windowse: reťazce typu COMX. Nevyhnutné je aj zadanie Baud rate, ideálne aby bol rovnaký ako v Arduine, čiže v našom prípade 9600. A pomocou **SerialPort seriovyPort.Open()** sa otvorí spojenie.

**2.** S využitím knižnice **System.Threading** a z nej triedy **Thread** sme zabezpečili samostatné vlákno, konkrétne **citacieVlakno = new Thread(citajSerial);** **citacieVlakno.Start();**. V tomto vlákne beží neustále čítanie riadkov pomocou **ReadLine()** a dáta sa ukladajú do premennej, z ktorej sa neskôr v **Update()** spracujú a transformujú do vstupov pre hru.
## Návod na spustenie


