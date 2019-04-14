# Nierelacyjne bazy danych
Skład grupy:
- Paweł Galewicz 210182
- Justyna Hubert 210200
- Bartosz Jurczewski 210209
- Karol Podlewski 210294

### Instalacja Mongo
[Instalator Mongo](https://fastdl.mongodb.org/win32/mongodb-win32-x86_64-2008plus-ssl-4.0.8-signed.msi) do pobrania. 

### Tworzenie bazdy danych
Aby móc skorzystać z skryptów, należy uruchomić PowerShell jako administrator i wpisać: 
```pwsh
Set-ExecutionPolicy RemoteSigned
```

Skypt tworzący bazę danych nazywa się `importData.ps1`, korzystając z PowerShella nalezy wpisać:
```pwsh
Get-Help .\importData.ps1 -Detailed
```
aby uzyskać pomoc dotyczącą używania skyptu.

### Rest API
Wymagane narzędzia Visual Studio
- Programowanie aplikacji klasycznych dla platformy .NET
- Opracowywanie zawartości dla platformy ASP&#46;NET i sieci Web 

Wymagane pakiety NuGet:
- mongocsharpdriver 
