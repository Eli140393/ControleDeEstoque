using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleDeEstoque.Web.Models
{
    public class UnidadeMedidaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome ")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha a sigla ")]
        public string Sigla { get; set; }
        public bool Ativo { get; set; }


        public static List<UnidadeMedidaModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<UnidadeMedidaModel>();
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = 0;
                    if (pagina != 1)
                    {
                        pos = (pagina - 1) * tamPagina;
                        pos++;
                    }
                    else
                    {
                        pos = (pagina - 1) * tamPagina;

                    }

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                    "select * from TB_UnidadeMedida order by NM_UnidadeMedida offset {0} rows fetch next {1} rows only",
                    pos > 0 ? pos - 1 : 0, tamPagina);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UnidadeMedidaModel
                        {
                            Id = (int)reader["ID_UnidadeMedida"],
                            Nome = (string)reader["NM_UnidadeMedida"],
                            Sigla = (string)reader["SG_UnidadeMedida"],
                            Ativo = (bool)reader["DS_Ativo"],
                        });
                    }
                }
            }

            return ret;
        }


        public static int RecuperarQuantidade()
        {
            var ret = 0;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from TB_UnidadeMedida";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static UnidadeMedidaModel RecuperarPeloId(int id)
        {
            UnidadeMedidaModel ret = null;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from TB_UnidadeMedida where (ID_UnidadeMedida = @id)";
                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UnidadeMedidaModel
                        {
                            Id = (int)reader["ID_UnidadeMedida"],
                            Nome = (string)reader["NM_UnidadeMedida"],
                            Sigla = (string)reader["SG_UnidadeMedida"],
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
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from TB_UnidadeMedida where (ID_UnidadeMedida = @id)";
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

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
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = "insert into TB_UnidadeMedida (NM_UnidadeMedida, SG_UnidadeMedida, DS_Ativo) values (@nome, @sigla, @ativo); select convert (int, scope_identity())";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@sigla", SqlDbType.VarChar).Value = this.Sigla;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = this.Ativo ? 1 : 0;
                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update TB_UnidadeMedida set NM_UnidadeMedida = @nome, SG_UnidadeMedida = @sigla, DS_Ativo = @ativo where ID_UnidadeMedida = @id";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@sigla", SqlDbType.VarChar).Value = this.Sigla;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = this.Ativo ? 1 : 0;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

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
