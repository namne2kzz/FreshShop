const product = {
    init: function () {
        product.registerEvents();
        product.deleteProduct();
    },
    registerEvents: function () {

    },
    deleteProduct: function () {

        $('.product-link-delete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Xóa sản phẩm',
                    text: "Bạn có chắc chắn muốn xóa sản phẩm này?",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'Cancle',
                    // customClass: 'swal-wide'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            url: 'http://localhost:5002/Product/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    window.location.href = "/Product/Index";
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa sản phẩm thành công!',
                                        'success'
                                    );
                                }
                                else {
                                    window.location.href = "/Product/Index";
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa sản phẩm thất bại",
                                        'error'
                                    );
                                }

                            }
                        })

                    }
                })
            })
        })
    }

}
product.init();