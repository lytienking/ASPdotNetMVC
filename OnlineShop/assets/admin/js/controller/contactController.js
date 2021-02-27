var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/AdminManage/ChangeStatusContact",
                data: { id: id },
                dataType: "Json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã trả lời');
                    }
                    else {
                        btn.text('Chưa trả lời');
                    }
                }
            });
        });
    }
}
user.init();