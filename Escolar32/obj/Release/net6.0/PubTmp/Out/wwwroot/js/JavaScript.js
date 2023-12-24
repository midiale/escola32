//Máscara de CPF
function mascara(i, t)
{
    if (t === "cpf")
{
    i.setAttribute("maxlength", "14");

i.addEventListener("input", function (event)
{
            var value = event.target.value.replace(/\D/g, "");

            if (value.length > 3 && value.length < 7)
{
    event.target.value = value.substring(0, 3) + "." + value.substring(3);
            }
            else if (value.length >= 7 && value.length < 11)
{
    event.target.value = value.substring(0, 3) + "." + value.substring(3, 6) + "." + value.substring(6);
            } else if (value.length >= 11)
{
    event.target.value = value.substring(0, 3) + "." + value.substring(3, 6) + "." + value.substring(6, 9) + "-" + value.substring(9);
            }

        });
    }
}
//Macara de Telefone
function mask(o, f)
{
    setTimeout(function () {
        var v = mphone(o.value);
        if (v != o.value) {
            o.value = v;
        }
    }, 1);
}

function mphone(v)
{
    var r = v.replace(/\D/g, "");
r = r.replace(/^0/, "");
    if (r.length > 10)
{
    r = r.replace(/^(\d\d)(\d{5})(\d{4}).*/, "($1) $2-$3");
    } else if (r.length > 5)
{
    r = r.replace(/^(\d\d)(\d{4})(\d{0,4}).*/, "($1) $2-$3");
    } else if (r.length > 2)
{
    r = r.replace(/^(\d\d)(\d{0,5})/, "($1) $2");
    } else {
    r = r.replace(/^(\d*)/, "($1");
    }
return r;
}
//Função Buscar CEP

// Crie um arquivo JavaScript separado, por exemplo, script.js

// Importe o jQuery utilizando uma função de autoexecução
(function () {
    var script = document.createElement('script');
    script.src = 'https://code.jquery.com/jquery-3.6.0.min.js';
    script.onload = function () {
        // Agora que o jQuery está carregado, podemos usar seu código aqui
        $(document).ready(function () {
            $('#Cep').on('change', function () {
                var cep = $(this).val();
                if (cep.length === 8) {
                    $.ajax({
                        url: 'https://viacep.com.br/ws/' + cep + '/json/',
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {
                            $('#Endereco').val(data.logradouro);
                            $('#Bairro').val(data.bairro);
                            $('#Cidade').val(data.localidade);
                        },
                        error: function () {
                           message.alert("CEP não encontrado");
                        }
                    });
                }
            });
        });
    };
    document.head.appendChild(script);
})();

