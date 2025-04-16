#!/bin/bash

# Definir a senha do certificado
PASSWORD="8575a179-ea81-4064-b3de-ada8954a24e7"

# Diretório para armazenar os certificados
DIR="$HOME/.aspnet/https"

# Criar o diretório se não existir
mkdir -p $DIR

# Criar certificado
dotnet dev-certs https -ep "$DIR/aspnetapp.pfx" -p $PASSWORD

# Confiar no certificado
dotnet dev-certs https --trust

echo "Certificado HTTPS para desenvolvimento criado em $DIR/aspnetapp.pfx"
echo "Senha do certificado: $PASSWORD"
echo "Esse certificado foi configurado para ser usado nos contêineres Docker"