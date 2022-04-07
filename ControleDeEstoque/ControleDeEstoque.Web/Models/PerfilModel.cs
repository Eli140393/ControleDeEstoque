using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ControleDeEstoque.Web
{
    public class PerfilModel
    {
        public int IdPerfil { get; set; }

        [Required(ErrorMessage = "Preencha o nome ")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }
         

        public static List<PerfilModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<PerfilModel>();
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
                        pos = (pagina - 1 ) * tamPagina;

                    }

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                    "select * from TB_PerfilUsuario order by NM_PerfilUsuario offset {0} rows fetch next {1} rows only",
                    pos > 0 ? pos - 1 : 0, tamPagina);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new PerfilModel
                        {
                            IdPerfil = (int)reader["ID_PerfilUsuario"],
                            Nome = (string)reader["NM_PerfilUsuario"],
                            Ativo = (bool)reader["DS_Ativo"],
                        });
                    }
                }
            }

            return ret;
        }
        public static List<PerfilModel> RecuperarListaAtivos()
        {
            var ret = new List<PerfilModel>();
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                  
                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                    "select * from TB_PerfilUsuario where DS_Ativo = 1 order by NM_PerfilUsuario");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new PerfilModel
                        {
                            IdPerfil = (int)reader["ID_PerfilUsuario"],
                            Nome = (string)reader["NM_PerfilUsuario"],
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
                    comando.CommandText = "select count(*) from TB_GrupoProduto";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static PerfilModel RecuperarPeloId(int id)
        {
            PerfilModel ret = null;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from TB_PerfilUsuario where (ID_PerfilUsuario = @id)";
                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new PerfilModel
                        {
                            IdPerfil = (int)reader["ID_PerfilUsuario"],
                            Nome = (string)reader["NM_PerfilUsuario"],
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
                        comando.CommandText = "delete from Tb_PerfilUsuario where (ID_PerfilUsuario = @id)";
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
            var model = RecuperarPeloId(this.IdPerfil);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = "insert into TB_PerfilUsuario (NM_PerfilUSuario, DS_Ativo) values (@nome, @ativo); select convert (int, scope_identity())";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = this.Ativo ? 1 : 0;
                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update TB_PerfilUsuario set NM_PerfilUsuario = @nome, DS_Ativo = @ativo where ID_PerfilUSuario = @id";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = this.Ativo ? 1 : 0;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.IdPerfil;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.IdPerfil;
                        }
                    }
                }


                return ret;
            }
        }
    }
}
