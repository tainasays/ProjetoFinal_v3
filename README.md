<h1> Projeto Final - Avaccenture - Gerenciador de Horas</h1>

Projeto desenvolvido durante o treinamento da Impacta.

**Equipe**: 

Júlia Inoscência

Natália Madeira

Rhayanna Paz

Thayna Santos

Tiemi Imayoshi
<hr />


📌 Acesso:

* Login de usuário de tipo Administrador: ana@email.com, senha: Senh@123

* Login de usuário de tipo Colaborador: joao@email.com, senha: Senh@123


<h2>⚙️ Requisitos:</h3>

- Criar um sistema que registre as horas trabalhadas e o tipo de atividade desenvolvida durante estas horas. ✅
- O usuário deve poder criar, recuperar, atualizar e excluir registros. ✅
- Autenticação e estabelecimento dos níveis de acesso. ✅
- Apontamento das horas nas atividades específicas. ✅
  
<h3>➡️ Histórias:</h3>
 <h4>1. Criar tela de login e acesso</h4> 
 
- Pré-requisitos:
Ter uma base de dados de usuários cadastrados. ✅
Framework ou biblioteca para implementação de autenticação. ✅

- Regras:
É necessário validar se o usuário e a senha inseridos correspondem ao registro do banco. ✅
- Entrada:
Usuário e senha fornecido pelo usuário. ✅
- Saída:
Acesso permitido ou negado. Caso seja permitido, há redirecionamento de tela. ✅

<h4>2. Gerenciar WBS</h4>

- Pré-requisitos:
Sistema já em funcionamento. Armazena e gerencia dados. ✅
Interface de administração para fazer operações CRUD. ✅
Como administrador, hpa as seguintes WBS padrão já cadastradas: Férias, Day-off, Sem tarefa e Implementação e desenvolvimento. ✅

- Regras:
O registro cadastrado deve conter um ID com um contador sequencial e um código que será inserido pelo usuário. ✅
O código não deve conter menos que 4 caracteres e nem mais que 10. Só são aceitos letras e números. ✅
- Entrada:
Para criar ou editar uma WBS: código, descrição e tipo ✅
Solicitação para exibir todas as WBS cadastradas. ✅
- Saída:
Para criar ou editar uma WBS: confirmar se foi feito com sucesso. ✅
Para visualizar a lista: todas as WBS cadastradas. ✅

<h4>3. Criar tela de lançamento de horas</h4>
Linhas representam as WBS e as colunas representam os dias da quinzena. ✅

- Pré-requisitos:
Usuário deve estar logado. ✅
WBS precisam estar cadastradas. ✅
Lógica para calcular horas totais por dia e totalizar horas para cada WBS. ✅

- Regras:
Validação do sistema para saber se as 8 horas foram preenchidas em cada dia útil. ✅
Validar que o preenchimento só seja permitido em dias úteis. ✅ ➡️ Indicamos a diferença de um lançamento regular para um lançamento de fim de semana com uma modal de alerta, pois, na prática, é possível fazer hora extra aos fins de semana, contando, assim, como hora excedente. 
Exibir a soma das horas registradas para cada dia. ✅
- Entrada:
Seleção de WBS e preenchimento de horas trabalhadas para cada dia da quinzena. ✅
- Saída: 
Confirmação de que as horas foram registradas com sucesso. ✅
Mensagens de alerta caso as horas registradas no dia sejam menores que as 8 horas estipuladas.✅

<h4>4. Implementar navegação entre quinzenas</h4>

- Regras:
A navegação não pode ser para um período anterior a 01/01/2024. ✅
Lançamentos futuros serão aceitos. ✅
- Entrada:
Seleção de opção para avançar ou retroceder para a próxima ou para a quinzena anterior. ✅ ➡️ A navegação entre quinzenas e meses é feita através do dropdown.
- Saída:
Redirecionamento para tela correspondente à quinzena escolhida. ✅

<h4>5. Relatórios</h4>
Acessar um relatório no PowerBI que permita: 
Identificar o maior número de apontamentos de horas.

- Regras:
O relatório deve exibir uma lista das WBS (classificadas em ordem decrescente, com base no apontamento de horas registradas). ✅

Cada entrada na lista deve incluir informações detalhadas sobre a WBS (código, descrição, tipo e total de horas apontadas). ✅

O relatório deve ser interativo, permitindo a filtragem dos dados por período de tempo específico, permitindo análise do histórico de apontamentos das horas. ✅

Deve haver opções de visualização gráfica (ex.: gráfico de barras...) para facilitar a compreensão dos dados e possibilitar a identificação das WBS com maior frequência de horas. ✅

<hr />

<h3>📺Telas </h3>

Login
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/dbb82394-539f-4277-9cb0-4e4094b08e03)

Registrar horas e perfil de colaborador
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/cc6e1f77-97d7-44e8-97e7-7d5a4659eb93)

Dados pessoais
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/708fa11b-99e9-46e9-8757-48572013619c)

Editar localização
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/03ce22dd-8015-4e86-80b2-9e4c720eb063)


Portal do Admin
<img width="920" alt="image" src="https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/2c9454ee-875b-4d88-80c9-deb8158fe5ad">

Funcionários
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/19c47546-cf8a-416a-83df-e179f8b193d4)

Cadastrar novo funcionário
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/e1d3e3a3-e4d7-4892-98a5-1275d8b59ee6)

Códigos de Custo
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/ae930b19-22e0-4f40-983b-0436c98128d8)

Cadastrar novo código
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/ef464d52-ad04-4208-9aa0-9575883e43e6)

Departamento
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/ed0040a9-0c93-4b7f-a17f-82e85c6cbbe3)

Cadastrar novo departamento
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/34694f98-6fe7-443e-98a4-781e1897e0ba)

Relatório Individual
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/3aa58ce9-b8f5-4a21-99cb-8e37b84efc6a)

Relatório Geral
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/c75aae0c-ab21-4ce3-bb85-52abbc1cb066)

Power BI
![image](https://github.com/tainasays/ProjetoFinal_v3/assets/102188509/be502866-4296-4749-8619-68b29a8dd230)


<h4>🪛 Desenvolvido com ASP.NET Core MVC.</h4>
