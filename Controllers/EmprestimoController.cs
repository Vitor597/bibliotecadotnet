using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Biblioteca.Controllers
{
    
    public class EmprestimoController : Controller
    {
        public IActionResult Cadastro()
        {
            LivroService livroService = new LivroService();
            EmprestimoService emprestimoService = new EmprestimoService();

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarTodos();

            if(HttpContext.Session.GetString("login") == null)
            return RedirectToAction("Index", "Home");
            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadEmprestimoViewModel viewModel)
        {
            EmprestimoService emprestimoService = new EmprestimoService();
            
            if(viewModel.Emprestimo.Id == 0)
            {
                emprestimoService.Inserir(viewModel.Emprestimo);
            }
            else
            {
                emprestimoService.Atualizar(viewModel.Emprestimo);
            }
            return RedirectToAction("Listagem");
        }

        [HttpGet]
        public IActionResult Listagem(int p = 1){

           int quantidadePorPagina = 10;

           EmprestimoService es = new EmprestimoService();

           ICollection<Emprestimo> emprestimos = es.Listar(p, quantidadePorPagina);

            int quantidadeRegistros = es.CountEmprestimos();
           
           ViewData["Paginas"] = (int)Math.Ceiling((double)quantidadeRegistros / quantidadePorPagina);

           if(HttpContext.Session.GetString("login") == null)
            return RedirectToAction("Index", "Home");
            return View(emprestimos);
        }
        [HttpPost]
        public IActionResult Listagem(string tipoFiltro, string filtro)
        {    

            FiltrosEmprestimos objFiltro = null;
            if(!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosEmprestimos();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            EmprestimoService emprestimoService = new EmprestimoService();
            ICollection<Emprestimo> emprestimos = emprestimoService.ListarTodos(objFiltro);

            if(emprestimos.Count == 0) {
             ViewData["Mensagem02"] = "Nenhum registro encontrado";
            }
            return View(emprestimos);
        }

        public IActionResult Edicao(int id)
        {
            LivroService livroService = new LivroService();
            EmprestimoService em = new EmprestimoService();
            Emprestimo e = em.ObterPorId(id);

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarTodos();
            cadModel.Emprestimo = e;
            
            return View(cadModel);
        }
    }
}