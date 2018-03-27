GeekBurger.Dashboard & Geekburguer.LabelLoader

Matéria: Arquitetura de Integração e Micro serviços.

Professor: Daniel Makiyama

	Integrantes
•	35516 – Leonardo Felipe Forjanes Gandra

•	37411 – Diego Balduino

•	30868 - Wagner

•	31976 - Eduardo Nuner

•	31551 – Fernando Albuquerque

	Escopo
Geekburger é uma empresa que tem em mente personalizar o modo que pessoas com restrições alimentares possam realizar seus pedidos, de forma muito mais rápida e prática. Sua arquitetura está baseada em micro serviços que se comunicam entre si, fazendo todo o processo funcionar de forma independente.

•	Dashboard

Neste processo o Dashboard será responsável por coletar as mensagens de ordem finalizada e consolidar o número de pedidos por minuto. Também receberá uma mensagem da loja informando a lista de produtos por usuário, checando se a oferta de produtos x usuários é adequada.

•	LabelLoader

O LabelLoader será responsável por varrer uma pasta de imagens de rótulos e enviar para o serviço de ingredientes. 

	Swagger.json
A página gerada pelo swagger no projeto Geekburger.Dashboard se encontra na seguinte URL: https://dashboardgeekburger.azurewebsites.net/swagger/

O Json gerado pelo swagger no projeto Geekburger.Dashboard se encontra na seguinte URL: https://dashboardgeekburger.azurewebsites.net/swagger/v1/swagger.json

	Arquitetura

•	Contratos: São responsáveis por definir quais serão os dados de entrada e os de saída da aplicação, facilitando bastante a usabilidade do micro serviço.

•	Endpoints: É o caminho que o micro serviço disponibiliza para aqueles que vão consumir seu serviço devem utilizar, para cada método publicado, diferentes endpoints devem ser informados

•	Swagger: O Swagger é um projeto composto por algumas ferramentas que auxiliam o desenvolvedor de APIs REST em algumas tarefas como: modelagem de API, geração de documentos legível da API e geração de códigos do cliente e do servidor com suporte a várias linguagens de programação.

•	Models: Responsável por armazenar as entidades que serão usadas no processo dos micro serviços. 

	Estrutura dos métodos
