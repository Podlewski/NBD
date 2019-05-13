# Nierelacyjne bazy danych

### Skład grupy

- Paweł Galewicz 210182
- Justyna Hubert 210200
- Bartosz Jurczewski 210209
- Karol Podlewski 210294

------------

### Tworzenie bazy danych

Aby móc skorzystać z skryptów, należy uruchomić PowerShell jako administrator i wpisać: 
```pwsh
Set-ExecutionPolicy RemoteSigned
```

Skypt tworzący bazę danych nazywa się `importData.ps1`, korzystając z PowerShella nalezy wpisać:
```pwsh
Get-Help .\importData.ps1 -Detailed
```
aby uzyskać pomoc dotyczącą używania skyptu.

------------

### Rest API - wymagania

Wymagane narzędzia Visual Studio:
- Programowanie aplikacji klasycznych dla platformy .NET
- Opracowywanie zawartości dla platformy ASP&#46;NET i sieci Web 

Wymagane pakiety NuGet:
- mongocsharpdriver 

------------

### Odpalanie dockerowych kontenerów

Wykorzystane narzędzia: [Docker Toolbox for Windows](https://download.docker.com/win/stable/DockerToolbox.exe)

1. Uruchamiany dwie instancje __Docker Quickstart Terminal__, z nich będziemy korzystać w następnych punktach.
	> __UWAGA:__ Jeżeli mieliśmy zainstalowany __GIT for Windows__ w innym niż w domyślnym folderze, w powstałym skrócie na pulpicie należy zmienić ścieżkę uruchamiania. 

2. W obu terminalach wchodzimy do naszego repozytorium, a w nim do folderu _Cluster_ (polecenie `cd Cluster`).

3. W pierwszym z nich wpisujemy
`docer-compose build` oraz `docer-compose up`.
	
4. W drugim, kiedy odczekamy chwilę, uruchamiamy skrypt **clusterSetup** poleceniem `./clusterSetup.sh`.
	> __UWAGA:__ Ważne jest, aby żadna z operacji nie zwróciła wyjątku _exception: connect failed_,  co się może zdarzyć szczególnie przy dodawaniu shardów. W takim wypadku należy odczekać więcej czasu na załadowanie się wszystkiego. Można sprawdzić czy wszystko działa poprawnie poleceniami `docker ps -a` (każdy kontener powinien mieć status _up_) oraz `docker exec -it mongos1 bash` (powinniśmy móc wejść do kontenera, wychodzimy za pomocą `exit`). Jeżeli jedno i drugie zachowuje się tak jak powinno, wtedy możemy odpalić skrypt jeszcze raz.

5. Pobieramy ip dockerowego kontenera przy pomocy `docker-machine ip default`.

6. Nasze zapytania kierujemy na port _5000_, koniecznie przy użyciu protokołu _HTTP_, na przykład chcąc dostać pacjenta o numerze _82442376_ wyślemy zapytanie _GET_ na adres `http://` **`POBRANE_IP`** `:5000/api/patients/82442376`.