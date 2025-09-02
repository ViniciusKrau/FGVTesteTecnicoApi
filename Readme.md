## Como rodar o projeto com Docker Compose

1. Certifique-se de ter o [Docker](https://www.docker.com/) instalado em sua máquina.


2. Abra o terminal da pasta raiz:

```sh
cd d:\Codes\.Net\TesteTecnicoApi
```

3. Execute o comando abaixo para subir os containers:

```sh
docker-compose up -d
```

4. Aguarde até que todos os serviços estejam iniciados.


5. Para parar e remover os containers, execute:

```sh
docker-compose down
```

> O banco de dados será inicializado automaticamente com os dados do script `init.sql`.