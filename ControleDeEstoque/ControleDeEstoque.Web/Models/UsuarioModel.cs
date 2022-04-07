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
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Informe o login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o perfil")]
        public int  IdPerfil { get; set; }


        public static UsuarioModel ValidarUsuario(string login, string senha)
        {
             UsuarioModel ret = null;
            using(var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from TB_Usuario where DS_Login = @login and DS_Senha = @senha";
                    comando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);



                   var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["ID_Usuario"],
                            Nome = (string)reader["NM_Usuario"],
                            Login = (string)reader["DS_Login"],
                            Senha = (string)reader["DS_Senha"],
                            IdPerfil = (int)reader["ID_PerfilUsuario"]

                        };
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
                    comando.CommandText = "select count(*) from TB_Usuario";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static List<UsuarioModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<UsuarioModel>();
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
                    "select * from TB_Usuario order by NM_Usuario offset {0} rows fetch next {1} rows only",
                    pos > 0 ? pos - 1 : 0, tamPagina);
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UsuarioModel
                        {
                            Id = (int)reader["ID_Usuario"],
                            Nome = (string)reader["NM_Usuario"],
                            Login = (string)reader["DS_Login"],
                            IdPerfil = (int)reader["ID_PerfilUsuario"]
                        });
                    }
                }
            }

            return ret;
        }

        public static UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from TB_Usuario where (ID_Usuario = @id)";
                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["ID_Usuario"],
                            Nome = (string)reader["NM_Usuario"],
                            Login = (string)reader["DS_Login"],
                            IdPerfil = (int)reader["ID_PerfilUsuario"]

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
                        comando.CommandText = "delete from TB_Usuario where (ID_Usuario = @id)";
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
                        comando.CommandText = "insert into TB_Usuario (NM_Usuario, DS_Login, DS_Senha, ID_PerfilUsuario ) values (@nome, @login, @senha, @perfil); select convert (int, scope_identity())";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;
                        comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);
                        comando.Parameters.Add("@perfil", SqlDbType.Int).Value = this.IdPerfil;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update TB_Usuario set NM_Usuario = @nome, DS_Login = @login, ID_PerfilUsuario = @perfil " +
                        (!string.IsNullOrEmpty(this.Senha) ? " , DS_Senha = @senha " : "" ) +
                        "where ID_Usuario = @id";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@login", SqlDbType.VarChar).Value = this.Login;
                        comando.Parameters.Add("@perfil", SqlDbType.Int).Value = this.IdPerfil;

                        if (!string.IsNullOrEmpty(this.Senha))
                        {
                            comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(this.Senha);

                        }
                        comando.Parameters.Add("@id", SqlDbType.VarChar).Value = this.Id;


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
