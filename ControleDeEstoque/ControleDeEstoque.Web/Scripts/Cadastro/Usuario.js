function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_login').val(dados.Login);
    $('#txt_senha').val(dados.Senha);
    $('#ddl_perfil').val(dados.IdPerfil);


}

function set_focus_form() {

    $('#txt_nome').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Login: '',
        Senha: '',
        IdPerfil: 0
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Login: $('#txt_login').val(),
        Senha: $('#txt_senha').val(),
        IdPerfil: $('#ddl_perfil').val()
    }
}

function preencher_linha_grid(linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Login);

}

$(document).ready(function () {
    var grid = $('#grid_cadastro > tbody');
    for (var i = 0; i < linhas.length; i++) {
        grid.append(criar_linha_grid(linhas[i]));
    }
});