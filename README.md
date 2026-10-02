# Hardware- und IT-Anforderungen für Ringerturniere

## Je Matte

- 1x Laptop mit Netzteil
- 1x Fernseher oder Beamer mit Leinwand und Kaltgerätestecker
- 1x HDMI-Kabel
- Ggf. HDMI-Adapter (USB-C, DisplayPort, Mini DisplayPort)
- 1x Gong, Tröte oder Hupe
- Optional: 1x externe Maus (wünschenswert)
- Optional: 1x Wurfkissen
- 1x Mehrfachsteckdose für Laptop und Beamer/Fernseher
- 1x Netzwerkkabel für Laptop
- 2 Personen zur Bedienung des Laptops (für Wechsel z. B. wegen Pausen)

> Empfehlung: Pro 50–75 Teilnehmer sollte eine Matte eingeplant werden. Erfahrungsgemäß liegt die Ausfallquote bei Meldungen bei ca. 15–18 %.

## Zentrale Anzeige der Wettkampffolge

### Benötigte Hardware

- 1x Laptop mit Netzteil
- 1x Netzwerkkabel
- 1x HDMI-Kabel

### Anzeigeoptionen

a) Großer Beamer mit Leinwand (Lichtverhältnisse der Halle berücksichtigen)

b) Großer Fernseher im Zuschauerbereich

c) Mehrere 27"-Monitore im Hochformat

> Empfehlung: Kombination aus a) und c)

## Netzwerkinfrastruktur

### Allgemeine Empfehlungen

- Vernetzung bevorzugt kabelgebunden durchführen
- WLAN nur bei hochwertiger Infrastruktur verwenden
- Bei Bedarf lokale Switches an den Matten einsetzen (spart Kabel) - z. B. Switch an Matte 2 und dann Weiterverteilung zu Matte 1 und Matte 3

### Switch-Anforderungen

| Matten   | Mindestanforderung    |
| -------- | --------------------- |
| 3 Matten | 6-Port Gigabit-Switch |
| 4 Matten | 8-Port Gigabit-Switch |

> Empfehlung: NETGEAR GS108

## Ausstattung Wettkampfbüro

- 1x Laserdrucker (bestenfalls mit 2 Schächten, 1x Urkunden, 1x Papier | z. B. Brother HL-L5100DNT)

  > Urkunden Druck vorher prüfen!
  > I. d. R. unterstützen Drucker offiziel 170 g/m² Papier
- 1x Ersatztoner
- Urkunden (Teilnehmerzahl +10 %)

  > Empfehlung: Offset-Druck via Saxoprint.de oder FlyerAlarm.de über DIN-A4-Flyer und 170g/m² Papier (am günstigsten)
- 1x Druckerpapier (500 Blatt)

  > bei fehlendem Internet/Aufhängen von Listen: zusätzlich 1x Druckerpapier
- 1x Laptop als zentraler Server
- 1x Laptop für Listenführer (je weiteren Listenführer ein Laptop)
- 3x kurze Netzwerkkabel (3–5 m)
- Mehrfachsteckdose

## Konfiguration der Laptops

### Eigenes Benutzerkonto

Einrichtung eines neuen Windows-Benutzers (sehr empfohlen für den Wettkampf). Datentrennung zwischen Privat und Ringen + Zugangsdaten alle bekannt (gerne auch mit Zettel am Tisch oder Aufkleber am Gerät) + Keine Authentifizierungsprobleme zwischen Netzwerkfreigaben.

| Einstellung  | Wert           |
| ------------ | -------------- |
| Benutzername | ringen         |
| Passwort     | `Durchdreher2` |

### Betriebssystem

- Windows 10
- Windows 11

### Benötigte Software

TVW Setup: https://alex-badewitz.de/Tvw52Setup.exe

UltraVNC Server: https://uvnc.com/downloads/ultravnc.html

Passwort:`klaus`

Bei Live-Streaming: OBS Studio https://obsproject.com/de

## Windows-Updates

### Vor dem Turnier

- Alle Windows-Updates einige Tage vorher installieren
- Alle Geräte neu starten
- Prüfen, dass keine Updates mehr ausstehen

### Während des Turniers

- Windows-Updates pausieren
- Keine Updates während der Veranstaltung durchführen
- Optional aktive Stunden auf die Turnierdauer setzen

## Energieeinstellungen

- Energieschema „Höchstleistung“
- Bildschirm niemals ausschalten
- Energiesparmodus deaktivieren
- Ruhezustand deaktivieren

## Weitere Einstellungen

- Optional: Desktop-Hintergrund auf Einfarbig-Schwarz einstellen oder turnierspezifisches Hintergrundbild (Bessere Außenwirkung)

- Im Task-Manager Autostart von unnötigen Apps deaktivieren

- Benachrichtigungen auf "Nicht stören" setzen (Rechts unten bei der Uhrzeit)

## Netzwerk-Konfiguration

### Empfohlen: Eigener Router oder FritzBox

Falls ein eigener Router (z. B. FritzBox) verwendet wird, sollte DHCP genutzt werden.

Vorteile:

- Schnellere Einrichtung
- Einfacherer Austausch von Geräten
- Neue Geräte können ohne Konfiguration eingebunden werden
- Weniger Fehler
- Zentrale Verwaltung

### Alternative: Statische IP-Adressen

Nur erforderlich, wenn kein Router oder DHCP-Server vorhanden ist.

| Gerät            | IP-Adresse   |
| ---------------- | ------------ |
| Server           | 192.168.1.5  |
| Matte 1          | 192.168.1.10 |
| Matte 2          | 192.168.1.20 |
| Matte 3          | 192.168.1.30 |
| Matte 4          | 192.168.1.40 |
| Zentrale Anzeige | 192.168.1.50 |

Subnetzmaske:`255.255.255.0`

### Wichtiger Hinweis

Der IP-Bereich des Turniernetzwerks darf nicht mit dem Hallennetzwerk kollidieren.

### Netzwerkprofil

**Wichtig (Sonst sehr langsam)**: Netzwerktyp auf **Privat** einstellen 
(Einstellungen > Netzwerk und Internet > Eigenschaften > Privat) 
oder per PowerShell Force (Interface-Index passend wählen!):
`get-netconnectionprofile`
`set-netconnectionprofile -InterfaceIndex 6 -NetworkCategory Private`

## Checkliste vor dem Turnier

- Netzwerkprofil auf Privat
- Datei- und Druckerfreigabe aktiviert
- Windows-Updates abgeschlossen
- Updates pausiert
- Energiesparmodus deaktiviert
- UltraVNC getestet
- Drucker getestet
- Netzwerkkabel getestet

## Internet

- WLAN-Verfügbarkeit prüfen | Tagesflat gebucht (für den Fall von FritzBox mit Prepaid SIM-Karte)
- Bei fehlendem Internet zusätzliche Papierunterlagen bereithalten

## Ersatzgeräte je 3 Matten

- 1x Ersatzfernseher oder Ersatzbeamer
- 1x Ersatz-Laptop
- Ersatz-HDMI-Kabel
- Ersatz-Netzwerkkabel
- Ersatz-Mehrfachsteckdosen
- Ersatzmaus

## Empfehlung

Alle Hardware-, Software-, Drucker- und Netzwerkfunktionen spätestens einen Tag vor dem Turnier vollständig testen.


## Get This App Run
Im Ordner Resources muss die Software kopiert werden, die benötigt wird.
Beispiel:
Resources\UltraVNC\UltraVNC_1830_x64_Setup.exe