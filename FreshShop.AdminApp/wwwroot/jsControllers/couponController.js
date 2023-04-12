const coupon = {
    init: function () {
        coupon.registerEvents();
        coupon.deleteCoupon();
    },
    registerEvents: function () {
        $('.coupon-link-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = $(this).data('id');

            $.ajax({
                url: "http://localhost:5002/Coupon/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('Có hiệu lực');
                    }
                    else {
                        btn.text('Khóa');
                    }
                }
            });
        });

    },
    deleteCoupon: function () {
        $('.coupon-link-delete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Xóa mã giảm giá',
                    text: "Bạn có chắc chắn muốn xóa mã giảm giá này?",
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
                            url: 'http://localhost:5002/Coupon/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    window.location.href = "/Coupon/Index";
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa mã giảm giá thành công!',
                                        'success'
                                    );
                                }
                                else {
                                    window.location.href = "/Coupon/Index";
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa mã giảm giá thất bại",
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
coupon.init();