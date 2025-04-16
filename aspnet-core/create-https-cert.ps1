# Definir a senha do certificado
$PASSWORD = "8575a179-ea81-4064-b3de-ada8954a24e7"

# Diretório para armazenar os certificados
$DIR = "$HOME\.aspnet\https"

# Criar o diretório se não existir
if (!(Test-Path $DIR)) {
    New-Item -ItemType Directory -Path $DIR -Force | Out-Null
}

# Criar certificado
dotnet dev-certs https -ep "$DIR\aspnetapp.pfx" -p $PASSWORD

# Confiar no certificado
dotnet dev-certs https --trust

Write-Host "Certificado HTTPS para desenvolvimento criado em $DIR\aspnetapp.pfx"
Write-Host "Senha do certificado: $PASSWORD"
Write-Host "Esse certificado foi configurado para ser usado nos contêineres Docker"