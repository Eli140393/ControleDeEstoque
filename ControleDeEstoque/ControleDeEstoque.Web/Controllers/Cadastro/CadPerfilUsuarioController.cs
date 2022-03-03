using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleDeEstoque.Web.Controllers.Cadastro
{

    [Authorize(Roles = "Gerente")]

    public class CadPerfilUsuarioController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;


        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 });
            ViewBag.QuantidadeLinhasPorPaginas = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = PerfilModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = PerfilModel.RecuperarQuantidade();

            var difQuantidadePaginas = (quant % ViewBag.QuantidadeLinhasPorPaginas) > 0 ? 1 : 0;
            ViewBag.QuantidadePaginas = (quant / ViewBag.QuantidadeLinhasPorPaginas) + difQuantidadePaginas;

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PerfilPagina(int pagina, int tamPag)
        {
            var lista = PerfilModel.RecuperarLista(pagina, tamPag);


            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult RecuperarPerfil(int id)
        {
            return Json(PerfilModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult SalvarPerfil(PerfilModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;
            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.Salvar();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }

                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }


            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ExcluirPerfil(int id)
        {
            return Json(PerfilModel.ExcluirPeloId(id));
        }
    }
}
