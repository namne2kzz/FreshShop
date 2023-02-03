let cart = {
    init: function () {
        cart.registerEvent();
    },
    registerEvent: function () {
        let cartList = [];
        let couponRequest = null;
        let subtotal = 0;
        let discount = 0;
        let coupon = 0;
        let total = 0;
        $('.cart-item-remove').each(function () {
            $(this).off('click').on('click', function (e) {
                let tr = $(this).closest('tr');
                let id = $(this).data('id');              
                e.preventDefault();
                Swal.fire({
                    title: 'Xóa sản phẩm',
                    text: "Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng?",
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
                            url: 'http://localhost:5000/vi/Cart/RemoveItem',
                            data: { id: id },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Thành công',
                                        'Xóa sản phẩm hàng thành công!',
                                        'success'
                                    );
                                    tr.remove();
                                }
                                else {
                                    //window.location.reload();
                                    Swal.fire(
                                        'Thất bại',
                                        "Xóa sản phẩm thất bại",
                                        'error'
                                    );
                                }

                            }
                        });

                    }
                });
            });
        });

        $('#cart-button-delete').off('click').on('click', function (e) {           
            e.preventDefault();
            Swal.fire({
                title: 'Xóa giỏ hàng',
                text: "Bạn có chắc chắn muốn xóa tất cả sản phẩm này khỏi giỏ hàng?",
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
                        url: 'http://localhost:5000/vi/Cart/DeleteCart',
                        type: 'POST',
                        dataType: 'json',
                        success: function (res) {
                            if (res.status == true) {
                                Swal.fire(
                                    'Thành công',
                                    'Xóa giỏ hàng thành công!',
                                    'success'
                                );
                                $('#tblCart > tbody > tr').remove();
                            }
                            else {
                                //window.location.reload();
                                Swal.fire(
                                    'Thất bại',
                                    "Xóa giỏ hàng thất bại",
                                    'error'
                                );
                            }

                        }
                    });
                }
            });
        });

        $('.select-checked').each(function () {
            $(this).on('change', function () {
                if ($(this).is(':checked')) {

                    cartList.push({ CartId: $(this).data('id'), ProductId: $(this).val(), Quantity: $(this).data('quantity') });

                    subtotal += parseInt($(this).data('price')) * $(this).data('quantity');
                    $('#text-subtotal').html(subtotal + ' VND');

                    discount += parseInt($(this).data('discount'));
                    $('#text-discount').html(discount + ' VND');

                    total = subtotal - discount - coupon;
                    $('#text-total').html(total + ' VND');

                }
                else {
                    subtotal -= parseInt($(this).data('price')) * $(this).data('quantity');
                    $('#text-subtotal').html(subtotal + ' VND');

                    discount -= parseInt($(this).data('discount'));
                    $('#text-discount').html(discount + ' VND');

                    total = subtotal - discount - coupon;
                    $('#text-total').html(total + ' VND');

                    cartList.splice(cartList.indexOf({ CartId: $(this).data('id'), ProductId: $(this).val(), Quantity: $(this).data('quantity') }), 1);
                }

            });
        });

        $('#coupon-submit').on('click', function () {
            let code = $('#text-code-coupon').val();
            if (code.trim() != '') {
                $.ajax({
                    url: "http://localhost:5000/vi/Cart/ApplyCoupon",
                    data: { code: code },
                    dataType: "json",
                    type: "POST",
                    success: function (res) {
                        if (res.status == true) {
                            coupon = res.discount;
                            couponRequest = { Id: res.id, Code: code, Discount: res.discount }
                            $('#text-coupon').html(res.discount + ' VND');

                            total = subtotal - discount - coupon;
                            $('#text-total').html(total + ' VND');
                        }
                        else {
                            coupon = 0;
                            $('#text-coupon').html(coupon + ' VND');

                            total = subtotal - discount - coupon;
                            $('#text-total').html(total + ' VND');

                            Swal.fire(
                                'Oops...',
                                'Mã giảm giá không đúng!',
                                'error'
                            )
                        }
                    }
                });
            }
            else {
                $('#text-code-coupon').focus();
                let c = document.getElementById('text-code-coupon');
                c.setCustomValidity("Nhập mã giảm giá...");
                c.reportValidity();
            }
        });

        $('#coupon-unsubmit').on('click', function () {
            coupon = 0;
            couponRequest = null;
            $('#text-coupon').html(coupon + ' VND');

            total = subtotal - discount - coupon;
            $('#text-total').html(total + ' VND');
        });

        $('#link-checkout').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: 'http://localhost:5000/vi/Cart/GetCheckout',
                data: { cartList: cartList },
                type: 'POST',
                dataType: 'Json',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = '/vi/Cart/Checkout?listCheckout=' + JSON.stringify(res.listCheckout) + '&couponRequest=' + JSON.stringify(couponRequest);
                    }
                    else {
                        Swal.fire(
                            'Oops...',
                            'Chưa có sản phẩm được chọn!',
                            'error'
                        )
                    }
                }
            });
        });
    },
}
cart.init();