# ProjetoApp

## Sobre o Projeto

O ProjetoApp é uma aplicação desenvolvida utilizando a arquitetura em camadas baseada nos princípios de Domain-Driven Design (DDD), implementada com o framework ABP. A aplicação gerencia projetos e tarefas, permitindo o acompanhamento de progresso, histórico de alterações e relatórios de desempenho.

## Arquitetura

A solução segue uma arquitetura em camadas (Layered Architecture) conforme os princípios de DDD:

- **Camada de Domínio** (`ProjetoApp.Domain` e `ProjetoApp.Domain.Shared`): Contém as entidades de negócio, regras de domínio, interfaces de repositórios e serviços de domínio.
- **Camada de Aplicação** (`ProjetoApp.Application` e `ProjetoApp.Application.Contracts`): Implementa os casos de uso da aplicação, orquestrando os objetos de domínio para executar tarefas específicas.
- **Camada de Infraestrutura** (`ProjetoApp.EntityFrameworkCore`): Implementa persistência de dados, integração com serviços externos e outros detalhes técnicos.
- **Camada de Apresentação/API** (`ProjetoApp.HttpApi.Host`): Expõe as funcionalidades da aplicação para consumo externo através de APIs.

O projeto também inclui:

- **Migrador de Banco de Dados** (`ProjetoApp.DbMigrator`): Aplicativo de console que aplica migrações e inicializa dados.
- **Testes** (`ProjetoApp.Domain.Tests` e outros): Testes unitários e de integração para validar as funcionalidades.

## Pré-requisitos

Para executar a aplicação, você precisa ter instalado:

- [.NET 9.0+ SDK](https://dotnet.microsoft.com/download/dotnet)
- [Node v20.11+](https://nodejs.org/pt-br/) (para o frontend)
- [Docker](https://www.docker.com/products/docker-desktop/) (opcional, para execução containerizada)

## Como Executar os Testes

### Executando Testes Unitários

Para executar os testes unitários do domínio, use o seguinte comando:

```bash
cd ProjetoApp\aspnet-core
dotnet test .\test\ProjetoApp.Domain.Tests\ProjetoApp.Domain.Tests.csproj
```

### Verificando a Cobertura de Testes

Para executar os testes com análise de cobertura, siga estes passos:

1. Instale as ferramentas necessárias (se ainda não tiver instalado):

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
cd test\ProjetoApp.Domain.Tests
dotnet add package coverlet.msbuild
```

2. Execute os testes com coleta de cobertura:

```bash
cd ProjetoApp\aspnet-core
dotnet test test\ProjetoApp.Domain.Tests\ProjetoApp.Domain.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

3. Gere um relatório HTML para visualizar a cobertura:

```bash
reportgenerator -reports:test\ProjetoApp.Domain.Tests\coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html
```

4. Abra o arquivo `coveragereport\index.html` no navegador para visualizar o relatório de cobertura.

### Resultados da Cobertura de Testes de Domínio

A análise de cobertura mostra os seguintes resultados para as classes principais do domínio:

| Classe | Cobertura de Linhas | Cobertura de Branches |
|--------|-------------------|-------------------|
| ProjectManager | 100% | - |
| ProjectTask | 93.1% | - |
| Project | 94.4% | - |
| ProjectTaskManager | 86.2% | 66.6% |
| ProjectTaskReportManager | 98.1% | 62.5% |
| TaskPerformanceReport | 100% | - |
| TaskComment | 77.7% | - |
| TaskHistory | 89.4% | - |

A cobertura total das classes de domínio principais é de aproximadamente **83.6%**, alcançando o objetivo mínimo de 80%.

## Executando com Docker

### Preparação do Ambiente Docker

1. **Gerar certificado para HTTPS** (necessário para desenvolvimento):

```powershell
cd ProjetoApp\aspnet-core
.\create-https-cert.ps1
```

2. **Construir e iniciar os contêineres**:

```powershell
cd ProjetoApp\aspnet-core
docker-compose up -d
```

3. **Acompanhar logs** (opcional):

```powershell
docker-compose logs -f
```

### Acessando a Aplicação

Com os contêineres em execução, você pode acessar:

- **API (HTTPS)**: <https://localhost:5001>
- **API (HTTP)**: <http://localhost:5000>

### Acessando o Banco de Dados

- **Servidor**: localhost,1434
- **Usuário**: sa
- **Senha**: StrongPassword123!
- **Banco de Dados**: ProjetoApp_Dev

### Parando os Contêineres

Para parar os contêineres:

```powershell
docker-compose down
```

Para remover também os volumes (incluindo dados do banco):

```powershell
docker-compose down -v
```

## Configurações Adicionais

A solução vem com configurações que funcionam imediatamente. No entanto, você pode alterar:

- Verifique as `ConnectionStrings` nos arquivos `appsettings.json` nos projetos `ProjetoApp.HttpApi.Host` e `ProjetoApp.DbMigrator` e altere conforme necessário.

## Recursos Adicionais

Para saber mais sobre o framework ABP utilizado na aplicação:

- [Tutorial de Desenvolvimento de Aplicação Web](https://abp.io/docs/latest/tutorials/book-store/part-01?UI=Blazor&DB=EF)
- [Estrutura do Template de Aplicação](https://abp.io/docs/latest/solution-templates/layered-web-application)
- [Documentação do ABP Framework](https://abp.io/docs/latest)

## Refinamento: Perguntas para o PO

Durante o refinamento de futuras implementações ou melhorias, as seguintes perguntas podem ser feitas ao PO para alinhar expectativas e priorizar entregas:

- Quais são as funcionalidades críticas que ainda não foram implementadas e que podem impactar diretamente os objetivos do negócio?

- Quais são os principais objetivos de experiência do usuário para o sistema, e como podemos garantir que eles sejam atendidos?

- Qual é a visão de longo prazo para este projeto, e como as próximas iterações podem alinhar-se a essa visão?

## Melhorias no Projeto

Durante a análise do projeto, foram identificados os seguintes pontos de melhoria que podem ser implementados para aprimorar a qualidade, escalabilidade e manutenção do sistema:

### 1. Logs e Monitoramento
- **Por quê?** Atualmente, não há menção de logs estruturados ou monitoramento. Isso é essencial para identificar problemas em produção e garantir a estabilidade do sistema.
- **Ação:** Implementar logs estruturados com Serilog e configurar monitoramento básico (ex.: Application Insights ou ELK Stack).
- **Impacto:** Ajuda a detectar e resolver problemas rapidamente, reduzindo o tempo de inatividade.

### 2. Automatização do Deploy
- **Por quê?** O projeto já usa Docker, mas não há menção de pipelines de CI/CD. Automatizar o deploy melhora a eficiência e reduz erros manuais.
- **Ação:** Configurar um pipeline básico de CI/CD (ex.: GitHub Actions ou Azure DevOps).
- **Impacto:** Acelera entregas e garante consistência nos ambientes.

### 3. Cobertura de Testes
- **Por quê?** Algumas classes têm cobertura de testes baixa (ex.: `TaskComment` e `TaskHistory`). Isso pode levar a bugs não detectados.
- **Ação:** Priorizar testes para áreas críticas do domínio com baixa cobertura.
- **Impacto:** Reduz riscos de regressão e melhora a confiança no código.

### 4. Planejamento de Escalabilidade
- **Por quê?** O projeto pode crescer em número de usuários e dados. Planejar agora evita problemas futuros.
- **Ação:** Revisar a arquitetura para identificar gargalos e preparar o sistema para escalar (ex.: particionamento de banco, cache).
- **Impacto:** Garante que o sistema suporte crescimento sem reescritas caras.
