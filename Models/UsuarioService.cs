using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.EntityFrameworkCore;

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

        public ICollection<Usuario> Listar()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Usuario> query = bc.Usuarios;
                
                
                //ordenação padrão
                return query.OrderBy(l => l.Id).ToList();
            }
        }

      
        public Usuario ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);
            }
        }

        public void Remover(Usuario l)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                 bc.Usuarios.Remove(l);
                 bc.SaveChanges();
            }
        }

        public Usuario Login(Usuario user)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                 Usuario usuario = bc.Usuarios.Single(u => u.Login.Contains(user.Login));

                 if(usuario.Login == user.Login && usuario.Senha == user.Senha){
                     user.Id = usuario.Id;
                     user.Login = usuario.Login;
                     user.Senha = usuario.Senha;
                     
                 }

                 else{
                     user = null;
                 }

                return user;

            }
        }


    }
}