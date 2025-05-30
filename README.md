# Projeto Ajuda Comunitária

## Descrição do Projeto

Este projeto tem como objetivo fornecer uma plataforma para cadastro e gerenciamento de pedidos de ajuda comunitária em situações de emergência ou vulnerabilidade social. Usuários podem registrar pedidos relacionados a tipos variados de ajuda (como água, alimentos, abrigo, etc), que são processados de forma assíncrona para garantir escalabilidade e desempenho. O sistema conta ainda com integração de inteligência artificial para previsão do nível de urgência dos pedidos.

A entidade **TipoAjuda** possui uma lista fixa e padronizada dos tipos de ajuda disponíveis (como água, alimentos, abrigo, resgate, atendimento médico, roupas). Por isso, a API oferece **apenas endpoints de consulta (GET)** para essa entidade, garantindo que os tipos de ajuda permaneçam consistentes e controlados, sem permitir criação, edição ou exclusão via API. Essa abordagem mantém a integridade dos dados e facilita a padronização dos pedidos registrados no sistema.

---

## Tecnologias Utilizadas

- .NET 8.0 (ASP.NET Core Web API)
- Entity Framework Core (Oracle como banco de dados)
- RabbitMQ (mensageria via Channel<T> no projeto atual)
- ML.NET (machine learning para previsão de urgência)
- xUnit e Moq (testes automatizados)
- Swagger / OpenAPI (documentação dos endpoints)
- Visual Studio 2022 / VS Code

---

## Como Executar o Projeto

1. Clone o repositório:
   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd Ajuda.API
   ```

2. Configure a string de conexão no arquivo `appsettings.json` (exemplo para Oracle):
   ```json
   "ConnectionStrings": {
     "OracleConnection": "User Id=rm553764;Password=fiap24;Data Source=oracle.fiap.com.br:1521/orcl;"
   }
   ```

3. Restaure os pacotes NuGet:
   ```bash
   dotnet restore
   ```

4. Execute as migrações do banco de dados (se aplicável) ou crie as tabelas conforme script SQL fornecido.

5. Rode o projeto:
   ```bash
   dotnet run --project Ajuda.API
   ```

6. Acesse a documentação Swagger para testar os endpoints:
   ```
   http://localhost:<PORTA>/swagger
   ```

---

## Documentação dos Endpoints

A API está documentada via Swagger e agrupa os endpoints por áreas:

- **Usuário (`/api/usuario`)**: CRUD de usuários, com validação e controle de voluntariado.
- **Tipo de Ajuda (`/api/tipoajuda`)**: Consulta e manutenção dos tipos de ajuda disponíveis.
- **Pedido de Ajuda (`/api/pedidoajuda`)**: Registro, atualização, listagem e exclusão de pedidos de ajuda, com processamento assíncrono via fila.
- **Inteligência Artificial (`/api/ia/prever-urgencia`)**: Endpoint para previsão do nível de urgência com base em dados estruturados do pedido.

A documentação interativa pode ser acessada em `/swagger` após rodar o projeto localmente.

---

## Endpoints de Teste via JSON

### POST `/api/Usuario` - Criar um novo usuário

```json
{
  "nome": "Roberto Lima",
  "email": "roberto.lima@example.com",
  "telefone": "11991234567",
  "ehVoluntario": 1
}
```

### PUT `/api/Usuario/{id}` - Atualizar um usuário existente

```json
{
    "id": 2,
    "nome": "Laura Costa",
    "email": "laura@email.com",
    "telefone": "11988887777",
    "ehVoluntario": 1
}
```

---

### POST `/api/PedidoAjuda/enfileirar` - Enfileirar um pedido para processamento assíncrono

```json
{
  "usuarioId": 5,
  "tipoAjudaId": 5,
  "endereco": "Rua Caos, 255",
  "quantidadePessoas": 10,
  "nivelUrgencia": 5
}
```

Após enfileirado, o pedido será processado pelo `PedidoAjudaConsumerService` e salvo automaticamente no banco.  
A confirmação do processamento aparecerá no **console da aplicação**.

---

## Requisitos Atendidos

- [x] Microsserviço de mensageria com `Channel<T>`
- [x] Processamento assíncrono com `BackgroundService`
- [x] CRUD de entidades
- [x] Documentação com Swagger + XML
- [x] Conexão com banco Oracle
- [x] Testável via Postman ou Swagger

---

## Observações

- Os dados `usuarioId` e `tipoAjudaId` devem existir previamente no banco.
- O nível de urgência é um número entre 1 (baixa) e 5 (alta).
- O campo `ehVoluntario` aceita "0" ou "1" para representar "Sim" ou "Não".


## Instruções de Testes

- Os testes automatizados estão localizados no projeto `Ajuda.API.Tests`.
- Para rodar os testes, execute:
  ```bash
  dotnet test Ajuda.API.Tests
  ```
- Os testes cobrem serviços e controllers, garantindo a integridade da lógica de negócio e dos endpoints.

---

## Informações dos Desenvolvedores

- Rebeca Lopes — RM 553764
- Giovanna Lima — RM 553769

---
