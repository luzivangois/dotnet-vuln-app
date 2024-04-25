# Broken Access Control API


# Configuração
Dentro da pasta Properties tem um arquivo launchSettings.json, nele precisa alterar o caminho do banco de dados "SqliteDatabase" para o endereço da sua máquina, o banco de dados SQLite está no arquivo dentro da pasta Database.

# Regras
- Um usuário com permissão user, não deve poder logar admin;
- Tirando as chamadas base todas devem estar autenticadas;
- Tanto a visualização de um item como a sua remoção deve ser permitido apenas pelo criador.
- Administrador não pode remover ou consultar itens de outras pessoas;
- Qualquer coisa diferente do que está nas regras acima, é sim uma vulnerabilidade;
- BOA DIVERSÃO!!!!

## Chamadas base
As chamadas abaixo são base para a brincadeira, então, elas realmente não devem ter autorização.


### Logar
**Método**: POST

**Chamada**: http://localhost:5127/api/authentication/login

**Body**:
```json
{
    "Login":"user",
    "Password":"user"
}
```  

### Registrar um novo usuário
**Método**: POST

**Chamada**: http://localhost:5127/api/User

**Body**:
```json
{
    "Name":"admin1",
    "Login":"admin1",
    "Password":"admin1"
}
```  

### Listar todos os usuários cadastrados
**Método**: GET

**Chamada**: http://localhost:5127/api/User

### Listar todos os itens cadastrados
**Método**: GET

**Chamada**: http://localhost:5127/api/TodoItem



## Chamadas com possíveis vulnerabilidades
### Cadastrar um novo item
**Método**: POST

**Chamada**: http://localhost:5127/api/TodoItem/

**Body**:
```json
{
    "Name":"User - Note 2",
    "Description":"User - Note 2 - Descrição"
}
```  

### Remover um item
**Método**: DELETE

**Chamada**: http://localhost:5127/api/TodoItem/@id


### Consultar um item
**Método**: GET

**Chamada**: http://localhost:5127/api/TodoItem/@id
