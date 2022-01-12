


function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#cbx_ativo').prop('checked', dados.Ativo);
}

function set_focus_form() {

    $('#txt_nome').focus();
}

function set_dados_grid(dados) {
    return  '<td>' + dados.Nome + '</td>' +
        '<td>' + (dados.Ativo ? 'SIM' : 'NÃO') + '</td>' +
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Ativo: true
    };
}

$(document).on('click', '#btn_incluir', function () {
    abrir_form(get_dados_inclusao());
})

    .on('click', '.btn-alterar', function () {
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            url = urlRecuperar;
        param = { 'id': id };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                abrir_form(response);
            }
        });

    })

    .on('click', '.btn-excluir', function () {
        var btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),
            url = urlExclusao;
        param = { 'id': id };
        bootbox.confirm({
            message: 'Realmente deseja excluir o grupo de produto ?',
            buttons: {
                confirm: {
                    label: 'Sim',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-success'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post(url, add_anti_forgery_token(param), function (response) {
                        if (response) {
                            tr.remove();
                        }
                    });
                }
            }
        });
    })

    .on('click', '#btn_confirmar', function () {
        var btn = $(this),
            url = urlSalvar;
        param = {
            Id: $('#id_cadastro').val(),
            Nome: $('#txt_nome').val(),
            Ativo: $('#cbx_ativo').prop('checked')
        };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response.Resultado == "OK") {
                if (param.Id == 0) {
                    param.Id = response.IdSalvo;
                    var table = $('#grid_cadastro').find('tbody'),
                        linha = criar_linha_grid(param);
                    table.append(linha);
                }
                else {
                    var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
                    linha
                        .eq(0).html(param.Nome).end()
                        .eq(1).html(param.Ativo ? 'SIM' : 'NÃO');
                }

                $('#modal_cadastro').parents('.bootbox').modal('hide');
            }
            else if (response.Resultado == "ERRO") {
                $('#msg_aviso').hide();
                $('#msg_mensagem_aviso').hide();
                $('#msg_erro').show();

            }

            else if (response.Resultado == "AVISO") {
                $('#msg_mensagem_aviso').html(formatar_mensagem_aviso(response.Mensagens));
                $('#msg_aviso').show();
                $('#msg_mensagem_aviso').show();
                $('#msg_erro').hide();
            }
        });
    })

    .on('click', '.page-item', function () {
        var btn = $(this),
            tamPag = $('#ddl_tam_pag').val(),
            pagina = btn.text(),
            url = urlPagina,
            param = { 'pagina': pagina, 'tamPag': tamPag };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');
                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));

                }

                btn.siblings().removeClass('active');
                btn.addClass('active');
            }

        });
    })

    .on('change', '#ddl_tam_pag', function () {
        var ddl = $(this),
            tamPag = ddl.val(),
            pagina = 1,
            url = urlPagina,
            param = { 'pagina': pagina, 'tamPag': tamPag };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');
                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));

                }

                btn.siblings().removeClass('active');
                btn.addClass('active');
            }

        });


    });