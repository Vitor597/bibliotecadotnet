using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }
        public IActionResult Cadastro()
        {
            //Autenticacao.CheckLogin(this);
            if(HttpContext.Session.GetString("login") != "admin")
            return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario u)
        {
            UsuarioService usuarioService = new UsuarioService();

            if(u.Id == 0)
            {
                usuarioService.Inserir(u);
            }
            else
            {
                usuarioService.Atualizar(u);
            }

            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem()
        {
            if(HttpContext.Session.GetString("login") != "admin")
            return RedirectToAction("Index", "Home");
          
           ICollection<Usuario> usuarios;

           UsuarioService l = new UsuarioService();

           usuarios = l.Listar();

           return View(usuarios);

           
        }

        [HttpGet]
        public IActionResult Edicao(int id)
        {
            
            UsuarioService ls = new UsuarioService();
            Usuario user = new Usuario();
            user  = ls.ObterPorId(id);
            return View(user);
        }
        
        [HttpPost]
         public IActionResult Edicao(Usuario l)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
            
            Usuario user = bc.Usuarios.Find(l.Id);
            user.Login = l.Login;
            user.Senha = l.Senha;

            bc.SaveChanges();

            return RedirectToAction("Listagem");
            }
            
        }

         public IActionResult Remover(int id)
        {
            
            
            
            Usuario user = new Usuario();
            UsuarioService us = new UsuarioService();
            user = us.ObterPorId(id);
            us.Remover(user);

        

            return RedirectToAction("Listagem");
            
            
        }
    }
}