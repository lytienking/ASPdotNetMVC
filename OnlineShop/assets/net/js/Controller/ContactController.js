var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var content = $('#txtContent').val();
            $.ajax({
                url: '/Contact/Send',
                type: 'post',
                dataType: 'Json',
                data: {
                    content: content
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