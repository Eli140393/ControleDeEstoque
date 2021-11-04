using System;
using System.Collections.Generic;
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
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }


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
                    comando.CommandText = "select * from TB_Usuario where DS_Usuario = @login and DS_Senha = @senha";
                    comando.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                    comando.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);



                   var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)reader["ID_Login"],
                            Nome = (string)reader["NM_Usuario"],
                            Login = (string)reader["DS_Usuario"],
                            Senha = (string)reader["DS_Senha"],


                        };
                    }
                }
            }

            return ret;
        }
    }
}