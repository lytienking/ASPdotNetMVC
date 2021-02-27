var App =
{
    changeStatus() {
        $("#changeStatus").change(function () {
            var status = $(this).val();
            var id = $(this).data("id");

            $.ajax({
                type: "post",
                url: "/Admin/AdminManage/ChangeStatusOrder",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ bill_id: id, status: status }),
                success: function (data) {
                    window.location.href = "/Admin/AdminManage/OrderIndex"
                }
            });
        })
    }

}
$(document).ready(function () {
    App.changeStatus()
})