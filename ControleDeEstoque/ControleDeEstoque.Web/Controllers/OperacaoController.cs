using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleDeEstoque.Web.Controllers
{
    public class OperacaoController : Controller
    {
        // GET: Operacao
        public ActionResult EntradaProduto()
        {
            return View();
        }
        public ActionResult SaidaEstoque()
        {
            return View();
        }
        public ActionResult LancPerdaProduto()
        {
            return View();
        }
        public ActionResult Inventario()
        {
            return View();
        }
    }
}