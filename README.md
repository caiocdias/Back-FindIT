# Back_FindIT

Back_FindIT é um sistema backend desenvolvido em **.NET 8** com **ASP.NET Core** e **Entity Framework Core** para gerenciar objetos perdidos e encontrados dentro de uma universidade.

## Tecnologias Utilizadas

- **.NET 8** – Framework para desenvolvimento backend
- **ASP.NET Core** – Para criação da API RESTful
- **Entity Framework Core 8** – ORM para interação com banco de dados
- **MySQL** – Banco de dados relacional
- **JWT (JSON Web Token)** – Autenticação de usuários

## Funcionalidades

- **Usuários e Autenticação**
  - Cadastro, login e autenticação via **JWT**
  - Busca de usuários por **ID, nome e e-mail**
  - Soft delete (desativação de usuários)

- **Permissões e Autorização**
  - Controle de permissões baseado em **UserPermission**
  - Adição e remoção de permissões para usuários

- **Gerenciamento de Itens**
  - Cadastro, atualização e remoção de itens
  - Associação de itens a categorias
  - Registro do usuário que encontrou e do usuário que resgatou o item
  - Histórico de ações sobre itens

- **Categorias**
  - Cadastro, listagem e atualização de categorias
  - Soft delete de categorias

- **Histórico de Itens**
  - Registro de ações como **Cadastro, Atualização, Remoção e Resgate** de itens

## Configuração e Execução

### Configuração do Banco de Dados

1. Instale o **MySQL** e crie um banco de dados chamado `BackFindIT`.
2. Configure a string de conexão no `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;database=BackFindIT;user=root;password=suasenha"
   }
   ```

3. Rode as **migrações do Entity Framework**:

   ```sh
   dotnet ef database update
   ```

### Executando o Projeto

1. Instale o **.NET 8** (se ainda não tiver instalado).
2. No terminal, execute:

   ```sh
   dotnet run
   ```

3. A API estará disponível em:

   ```
   http://localhost:5000/api
   ```

## Autenticação

O sistema utiliza **JWT (JSON Web Token)** para autenticação. Para acessar endpoints protegidos:

1. Faça login em `/api/User/Login`
2. Use o token retornado no **Authorization Header** como:

   ```
   Authorization: Bearer SEU_TOKEN
   ```

## Endpoints Principais

### **Usuários**
| Método | Rota                  | Descrição |
|--------|------------------------|-----------|
| `POST` | `/api/User/AddUser` | Cadastra um usuário |
| `POST` | `/api/User/Login` | Faz login e retorna um JWT |
| `GET`  | `/api/User/GetUserById/{id}` | Obtém um usuário por ID |
| `DELETE` | `/api/User/SoftDelete/{id}` | Desativa um usuário |

### **Itens**
| Método | Rota                  | Descrição |
|--------|------------------------|-----------|
| `POST` | `/api/Item/AddCategory` | Cadastra um novo item |
| `GET`  | `/api/Item/GetItem/{id}` | Obtém um item por ID |

### **Categorias**
| Método | Rota                  | Descrição |
|--------|------------------------|-----------|
| `POST` | `/api/Category/AddCategory` | Cadastra uma nova categoria |
| `GET`  | `/api/Category/ListAllCategories` | Lista todas as categorias |

## Licença

Este projeto é de código aberto e distribuído sob a licença **MIT**.
