
function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name = __RequestVerificationToken]').val();
    return data;
}

function formatar_mensagem_aviso(mensagens) {
    var ret = "";
    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }

    return '<ul>' + ret + '</ul>'
}

function abrir_form(dados) {
    set_dados_form(dados)


    var modal_cadastro = $('#modal_cadastro');

    $('#msg_mensagem_aviso').empty();

    $('#msg_aviso').hide();
    $('#msg_mensagem_aviso').hide();
    $('#msg_erro').hide();

    bootbox.dialog({
        title: tituloPagina,
        message: modal_cadastro,

    })
        .on('shown.bs.modal', function () {
            modal_cadastro.show(0, function () {
                set_focus_form();
                $('#txt_nome').focus();
            });
        })
        .on('hidden.bs.modal', function () {
            modal_cadastro.hide().appendTo('body');
        });

}

function criar_linha_grid(dados) {
    var ret =
        '<tr data-id=' + dados.Id + '>' +
        set_dados_grid(dados);
        '<td>' +
        '<a class="btn btn-primary btn-alterar" role="button" style="margin-right: 3px"><i class="glyphicon glyphicon-pencil"></i> Alterar</a>' +
        '<a class="btn btn-danger btn-excluir" role="button"><i class="glyphicon glyphicon-trash"></i> Excluir</a>' +
        '</td>' +
        '</tr>';
    return ret;
}