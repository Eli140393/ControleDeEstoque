
function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name = __RequestVerificationToken]').val();
    return data;
}

function formatar_mensagem_aviso(mensagens) {
    var template =
        '<ul>' +
        '{{ #. }}' +
        '<li>{{ . }}</li>' +
        '{{ /. }}' +
        '</ul>';

    return Mustache.render(template, mensagens);
}

function abrir_form(dados) {
    set_dados_form(dados)


    var modal_cadastro = $('#modal_cadastro');

    $('#msg_mensagem_aviso').empty();

    $('#msg_aviso').hide();
    $('#msg_mensagem_aviso').hide();
    $('#msg_erro').hide();

    bootbox.dialog({
        title: 'Cadastro de ' + tituloPagina,
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
    var template = $('#template-grid').html();
    return Mustache.render(template, dados);
}


$(document).on('click', '#btn_incluir', function () {
    abrir_form(get_dados_inclusao());
})

    .on('click', '.btn-alterar', function () {
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            url = url_recuperar;
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
            url = url_exclusao;
        param = { 'id': id };
        bootbox.confirm({
            message: "Realmente deseja excluir o " + tituloPagina + " ? ",
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
            url = url_salvar;
        param = get_dados_form();


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
                    preencher_linha_grid(linha);

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
            url = url_pagina,
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
            url = url_tam_pag_change,
            param = { 'pagina': pagina, 'tamPag': tamPag };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');
                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));

                }

                ddl.siblings().removeClass('active');
                ddl.addClass('active');
            }

        });


    });