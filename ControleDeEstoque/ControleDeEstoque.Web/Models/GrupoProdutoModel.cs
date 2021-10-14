using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleDeEstoque.Web.Models
{
    public class GrupoProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome ")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }


        public static List<GrupoProdutoModel> RecuperarLista()
        {
            var ret = new List<GrupoProdutoModel>();
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = "Data Source=DESKTOP-KI43LUD\\SQLEXPRESS;Initial Catalog=DB_ControleEstoque;User Id=sa;Password=eliezer140393;";
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from TB_GrupoProduto order by NM_GrupoProduto";
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new GrupoProdutoModel
                        {
                            Id = (int)reader["ID_GrupoProduto"],
                            Nome = (string)reader["NM_GrupoProduto"],
                            Ativo = (bool)reader["DS_Ativo"],
                        });
                    }
                }
            }

            return ret;
        }

        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            GrupoProdutoModel ret = null;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = "Data Source=DESKTOP-KI43LUD\\SQLEXPRESS;Initial Catalog=DB_ControleEstoque;User Id=sa;Password=eliezer140393;";
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from TB_GrupoProduto where (ID_GrupoProduto = {0})", id);
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new GrupoProdutoModel
                        {
                            Id = (int)reader["ID_GrupoProduto"],
                            Nome = (string)reader["NM_GrupoProduto"],
                            Ativo = (bool)reader["DS_Ativo"],
                        };
                    }
                }
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;
            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = "Data Source=DESKTOP-KI43LUD\\SQLEXPRESS;Initial Catalog=DB_ControleEstoque;User Id=sa;Password=eliezer140393;";
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = string.Format("delete from TB_GrupoProduto where (ID_GrupoProduto = {0})", id);
                        ret = (comando.ExecuteNonQuery() > 0);

                    }
                }
            }
            return ret;
        }
        public int Salvar()
        {
            var ret = 0;
            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = "Data Source=DESKTOP-KI43LUD\\SQLEXPRESS;Initial Catalog=DB_ControleEstoque;User Id=sa;Password=eliezer140393;";
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = string.Format("insert into TB_GrupoProduto (NM_GrupoProduto, DS_Ativo) values ('{0}', {1}); select convert (int, scope_identity())", this.Nome, this.Ativo ? 1 : 0);
                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = string.Format(
                       "update TB_GrupoProduto set NM_GrupoProduto = '{1}', DS_Ativo = {2} where ID_GrupoProduto = {0}",
                       this.Id, this.Nome, this.Ativo ? 1 : 0);
                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }


                return ret;
            }
        }
    }
}
