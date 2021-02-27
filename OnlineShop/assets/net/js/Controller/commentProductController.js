var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnComment').off('click').on('click', function () {
            var content = $('#txtContent').val();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: '/Product/Comment',
                type: 'post',
                dataType: 'Json',
                data: {
                    content: content,
                    id:id
                },
                success: function (res) {
                    if (res.status == true) {
                        window.alert('Gửi thành công');
                        contact.resetForm();
                    }
                }
            });
        });
    },
    resetForm: function () {
        $('#txtContent').val('');
    }
}
contact.init();