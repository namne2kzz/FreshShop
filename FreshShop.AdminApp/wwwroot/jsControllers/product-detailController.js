
const productDetail = {
    init: function () {
        productDetail.registerEvents();
        productDetail.deleteImage();
        productDetail.deletePromotion();
    },

    registerEvents: function () {

    },
    deleteImage: function () {

        $('.productImage-link-delete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                var request = {

                    ProductId: $(this).data("product"),
                    ImageId: $(this).data("image")
                }
                Swal.fire({
                    title: 'Xóa hình ảnh',
                    text: "Bạn có chắc chắn muốn xóa hình ảnh này?",
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
                            url: 'http://localhost:5002/Product/DeleteImage',
                            data: { productImageDeleteRequest: JSON.stringify(request) },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {

                                    window.location.href = "/Product/Index";
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa hình ảnh thành công!',
                                        'success'
                                    );
                                }
                                else {

                                    window.location.href = "/Product/Index"
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa hinh ảnh thất bại",
                                        'error'
                                    );
                                }

                            }
                        })

                    }
                })
            })
        })
    },

    deletePromotion: function () {

        $('#btnPromotionDelete').off('click').on('click', function (e) {

            e.preventDefault();
            var id = $(this).data('id');
            Swal.fire({
                title: 'Xóa giảm giá sản phẩm',
                text: "Bạn có chắc chắn muốn xóa giảm giá này?",
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
                        url: 'http://localhost:5002/Promotion/Delete',
                        data: { id:id },
                        type: 'POST',
                        dataType: 'json',
                        success: function (res) {
                            if (res.status == true) {

                                window.location.href = "/Product/Index";
                                Swal.fire(
                                    'Thành công',
                                    'Xóa giảm giá thành công!',
                                    'success'
                                );
                            }
                            else {

                                window.location.href = "/Product/Index"
                                Swal.fire(
                                    'Thất bại',
                                    "Xóa giảm giá thất bại",
                                    'error'
                                );
                            }

                        }
                    })

                }
            })
        })

    },
};
productDetail.init();