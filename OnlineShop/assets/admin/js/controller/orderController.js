var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-activeorder').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/AdminManage/ChangeStatusOrder",
                data: { id: id },
                dataType: "Json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã thanh toán');
                    }
                    else {
                        btn.text('Chưa thanh toán');
                    }
                }
            });
        });
    }
}
user.init();