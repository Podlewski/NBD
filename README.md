# Nierelacyjne bazy danych

#### Skład grupy

- Paweł Galewicz 210182
- Justyna Hubert 210200
- Bartosz Jurczewski 210209
- Karol Podlewski 210294

------------

### Etap 1

#### Rest API - wymagania

Wymagane narzędzia Visual Studio:

- Programowanie aplikacji klasycznych dla platformy .NET
- Opracowywanie zawartości dla platformy ASP&#46;NET i sieci Web

Wymagane pakiety NuGet:

- mongocsharpdriver

#### Tworzenie bazy danych

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

### Etap 2

#### Przygotowanie środowiska na dockerowych kontenerach

Wykorzystane narzędzia: [Docker Toolbox for Windows](https://download.docker.com/win/stable/DockerToolbox.exe)

1. Uruchamiany __Docker Quickstart Terminal__, z niego będziemy korzystać w następnych punktach.
   > __UWAGA:__ Jeżeli mieliśmy zainstalowany __GIT for Windows__ w innym niż w domyślnym folderze, w powstałym skrócie na pulpicie należy zmienić ścieżkę uruchamiania.

2. W terminalu wchodzimy do naszego repozytorium, a w nim do folderu _Cluster_ (polecenie `cd Cluster`).

3. W pierwszym z nich wpisujemy
`docker-compose build` oraz `docker-compose up -d`.
   > __UWAGA:__ Jeżeli chcemy mieć stały podgląd logów, nie korzystamy z przełącznika -d. Potrzebna nam też będzie nowa instancja terminalu.

4. W drugim, kiedy odczekamy chwilę, uruchamiamy skrypt __clusterSetup__ poleceniem `./clusterSetup.sh`.
   > __UWAGA:__ Ważne jest, aby żadna z operacji nie zwróciła wyjątku _exception: connect failed_,  co się może zdarzyć szczególnie przy dodawaniu shardów. W takim wypadku należy odczekać więcej czasu na załadowanie się wszystkiego. Można sprawdzić czy wszystko działa poprawnie poleceniami `docker ps -a` (każdy kontener powinien mieć status _up_) oraz `docker exec -it mongos1 bash` (powinniśmy móc wejść do kontenera, wychodzimy za pomocą `exit`). Jeżeli jedno i drugie zachowuje się tak jak powinno, wtedy możemy odpalić skrypt jeszcze raz.

5. W celu dodania danych do naszej bazy danych uruchamiamy skrypt __fill.sh__ poleceniem `./fill.sh`.

6. Pobieramy ip dockerowego kontenera przy pomocy `docker-machine ip default`.

7. Nasze zapytania kierujemy na port _5000_, koniecznie przy użyciu protokołu _HTTP_, na przykład chcąc dostać pacjenta o numerze _82442376_ wyślemy zapytanie _GET_ na adres `http://` __`POBRANE_IP`__ `:5000/api/patients/82442376`.

------------

### Etap 3

#### Odpalanie dockerowych kontenerów

1. Uruchamiamy __Docker Quickstart Terminal__, po czym wchodzimy do folderu _Cluster_.

2. Uruchamiamy maszyny poleceniem `./DataCenters/startDataCenters.sh`.

3. Musimy przełączyć się `eval $(docker-machine env center1)`
   > __UWAGA:__ Żeby sprawdzić, czy wszystko przebiega poprawnie do tego momentu, należy skorzystać z polecenia `docker-machine ls`, które powinno ukazać nam 3 działąjące maszyny, z __\*__ przy kolumnie aktywności dla maszyny __center1__.

4. `docker stack deploy -c docker-compose.yaml test`

5. `./DataCenters/dataCenterClusterSetup.sh`

6. Sprawdzamy IP maszyn za pomocą `docker-machine ls`.

7. Ręcznie generujemy obrazy API wykorzystując Dockerfile. W tym celu otwieramy plik _appsettings.json_, gdzie zmieniamy IP w _ConnectionStringu_ na to wybranej maszyny, po czym z pomocą polecenia `docker build --tag diabetesapi1 ../DiabetesApi/DiabetesApi/` (zakładając, że wciąż jesteśmy w terminalu w folderze _Cluster_).

8. Robimy to samo dla drugiej maszyny, tym razem z poleceniem `docker build --tag diabetesapi2 ../DiabetesApi/DiabetesApi/` (pamiętając o zmianie IP w _appsettings.json_).

9. Wypełniamy danymi maszynę __center1__ z pomocą polecenia `./fillMachine.sh` __`IP_MASZNY`__.

10. Nasze zapytania kierujemy na porty: 5000 dla  __center1__ oraz 6000 dla  __center2__.
