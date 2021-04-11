using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Biblioteca.Models
{
    public class UsuarioService
    {
        public void Inserir(Usuario l)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Usuarios.Add(l);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Usuario l)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
              Usuario usuario = bc.Usuarios.Find(l.Id);
               usuario.Login = l.Login;
                usuario.Senha = l.Senha;

                bc.SaveChanges();
            }
        }

        public ICollection<Livro> ListarTodos(FiltrosLivros filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Livro> query;
                
                if(filtro != null)
                {
                    //definindo dinamicamente a filtragem
                    switch(filtro.TipoFiltro)
                    {
                        case "Autor":
                            query = bc.Livros.Where(l => l.Autor.Contains(filtro.Filtro));
                        break;

                        case "Titulo":
                            query = bc.Livros.Where(l => l.Titulo.Contains(filtro.Filtro));
                        break;

                        default:
                            query = bc.Livros;
                        break;
                    }
                }
                else
                {
                    // caso filtro não tenha sido informado
                    query = bc.Livros;
                }
                
                //ordenação padrão
                return query.OrderBy(l => l.Titulo).ToList();
            }
        }

        public ICollection<Livro> ListarDisponiveis()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                //busca os livros onde o id não está entre os ids de livro em empréstimo
                // utiliza uma subconsulta
                return
                    bc.Livros
                    .Where(l =>  !(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id)) )
                    .ToList();
            }
        }

        public Livro ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Livros.Find(id);
            }
        }
    }
}