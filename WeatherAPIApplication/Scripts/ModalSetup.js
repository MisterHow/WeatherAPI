$(function () {
    $('a[data-modal]').on("click", function () {
        console.log(this.href);
        $("#modalContent").load(this.href, function () {
            $("#modalBody").modal("show");
            console.log("loaded");
            $("#formChoice").submit(function (event) {
                event.preventDefault();
                if ($("#formChoice").valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            if (result.success) {
                                $('#modalBody').modal('hide');
                            } else {
                                $("#MessageToClient").text(result.responseText);
                            }
                        },
                        error: function (xhr, status, error) {
                            $("#MessageToClient").text(xhr.responseText);
                        }
                    });
                    return false;
                }
            });
            $("#modalBody").on('hidden.bs.modal', function () {
                console.log("modal body hidden");
                location.reload();
            });
        });
        return false;
    });
});