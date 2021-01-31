# Projeto WebReceitas
O projeto WebReceitas, tem como objetivo compartilhar receitas, nele os usuários poderão visualizar as receitas que são compartilhadas por outros usuarios. Neste site os usuário que desejam compartilhar suas receitas deveram se cadastrar no site e assim poderão postar suas receitas e gerenciá-las, alterando ou deletando.

Confira abaixo as principais tecnologias usadas no projeto:

# Organizacao do projeto
- Domínio: Aqui ficara as classes entidades do projeto.

- Repositório: Aqui será criada a interface com as principais funções do sistema e a sua implementação, o contesto do Entity Framework e a migrations do banco de dados.

- ApiReceitas: Aplicação back-end, ela será base para requisições de outras aplicações.

- WebAppReceitas: Aplicação front-end, aplicação de interação com usuário.

# Entity Framework
Neste projeto foi utilizado a abordagem Code First utilizando o banco de dados Sql Server o Entity Framework cria o banco de dados, faz os relacionamentos das entidades do domínio e simplifica as operações de CRUD utilizando principalmente LINQ para consulta ao banco de dados.

# AutoMapper
Usado para o mapeamento das entidades modelo da Api e do Mvc para as entidades do domínio.

# Identity
O identity foi usado com o intuito de realizar o cadastro de usuários, validação de cadastro através de e-mail, login e alteracão de senha.

# Asp Net core WebApi
Esta e a aplicação base para outras aplicações, ela será responsável pelas respostas das requisições de outras aplicações seja elas de autenticação de login, cadastro de usuário ou outras operações com banco de dados.

# Asp Net Core Mvc
A parte de visualização do projeto, através de requisições http para a Api ela obtém dados do banco de dados e exibe para o usuário.