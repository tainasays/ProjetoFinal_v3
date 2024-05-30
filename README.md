<h1> Projeto Final - Gerenciador de Horas</h1>

**Equipe**: 

Júlia Inoscência

Natália Madeira

Rhayanna Paz

Thayna Santos

Tiemi Imayoshi
<hr />

Projeto desenvolvido durando o treinamento da Impacta.

<h2>⚙️ Requisitos:</h3>

- Criar um sistema que registre as horas trabalhadas e o tipo de atividade desenvolvida durante estas horas.
- O usuário deve poder criar, recuperar, atualizar e excluir registros.
- Autenticação e estabelecimento dos níveis de acesso.
- Apontamento das horas nas atividades específicas.
  
<h3>➡️ Histórias:</h3>
 <h4>1. Criar tela de login e acesso</h4>
 
- Pré-Requisitos:
Ter uma base de dados de usuários cadastrados.
Framework ou biblioteca para implementação de autenticação.

- Regras:
É necessário validar se o usuário e a senha inseridos correspondem ao registro do banco.
- Entrada:
Usuário e senha fornecido pelo usuário.
- Saída:
Acesso permitido ou negado. Caso seja permitido, há redirecionamento de tela.

<h4>2. Gerenciar WBS</h4>

- Pré-requisitos:
Sistema já em funcionamento. Armazena e gerencia dados.
Interface de administração para fazer operações CRUD.
Como administrador, hpa as seguintes WBS padrão já cadastradas: Férias, Day-off, Sem tarefa e Implementação e desenvolvimento.

- Regras:
O registro cadastrado deve conter um ID com um contador sequencial e um código que será inserido pelo usuário.
O código não deve conter menos que 4 caracteres e nem mais que 10. Só são aceitos letras e números.
- Entrada:
Para criar ou editar uma WBS: código, descrição e tipo
Solicitação para exibir todas as WBS cadastradas.
- Saída:
Para criar ou editar uma WBS: confirmar se foi feito com sucesso.
Para visualizar a lista: todas as WBS cadastradas.

<h4>3. Criar tela de lançamento de horas</h4>
Linhas representam as WBS e as colunas representam os dias da quinzena.

- Pré-requisitos:
Usuário deve estar logado.
WBS precisam estar cadastradas.
Lógica para calcular horas totais por dia e totalizar horas para cada WBS.

- Regras:
Validação do sistema para saber se as 8 horas foram preenchidas em cada dia útil.
Validar que o preenchimento só seja permitido em dias úteis.
Exibir a soma das horas registradas para cada dia.
- Entrada:
Seleção de WBS e preenchimento de horas trabalhadas para cada dia da quinzena.
- Saída: 
Confirmação de que as horas foram registradas com sucesso.
Mensagens de alerta caso as horas registradas no dia sejam menores que as 8 horas estipuladas.

<h4>4. Implementar navegação entre quinzenas</h4>

- Regras:
A navegação não pode ser para um período anterior a 01/01/2024.
Lançamentos futuros serão aceitos.
- Entrada:
Seleção de opção para avançar ou retroceder para a próxima ou para a quinzena anterior.
- Saída:
Redirecionamento para tela correspondente à quinzena escolhida.

<h4>5. Relatórios</h4>
Acessar um relatório no PowerBI que permita: 
Identificar o maior número de apontamentos de horas.

- Regras:
O relatório deve exibir uma lista das WBS (classificadas em ordem decrescente, com base no apontamento de horas registradas).

Cada entrada na lista deve incluir informações detalhadas sobre a WBS (código, descrição, tipo e total de horas apontadas).

O relatório deve ser interativo, permitindo a filtragem dos dados por período de tempo específico, permitindo análise do histórico de apontamentos das horas.

Deve haver opções de visualização gráfica (ex.: gráfico de barras...) para facilitar a compreensão dos dados e possibilitar a identificação das WBS com maior frequência de horas. 

<h4>🪛 Desenvolvido com .ASP NET Core MVC.</h4>
