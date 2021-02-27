var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });
        $('#btnPayment').off('click').on('click', function () {
            window.location.href = "/thanh-toan";
        });
        $('#btnUpdate').off('click').on('click', function () {
            var listProductCart = $('.itemCart');
            var carList = [];
            $.each(listProductCart, function (i, item) {
                carList.push({
                    Size: $(item).children().find('.txtSize').val(),
                    Quantity: $(item).children().find('.txtQuantity').val(),
                    Product:
                        {ID:$(item).children().find('.txtSize').data('id')},
                });
            });
            
            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(carList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang"
                    }
                }
            })
            
        });
        $('#btnDeleteAll').off('click').on('click', function () {

            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang"
                    }
                }
            })
        });
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang"
                    }
                }
            })
        });
    }
}
cart.init();