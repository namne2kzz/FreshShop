let checkout = {
    init: function () {
        checkout.registerEven();
    },
    registerEven: function () {
        $('.shipping-option').each(function () {
            const total = $('#text-total').text();
            $(this).on('change', function () {
                let shipping = 0;
                if ($(this).is(':checked')) {
                    if ($(this).val() == 2) {
                        shipping = 30000;
                    }
                    else if ($(this).val() == 3) {
                        shipping = 50000;
                    }
                }
                console.log(shipping);
                $('#text-shipping').html(shipping + ' VND');               
                $('#text-total').html(parseInt(total) + shipping);
            });
        });
    },
}
checkout.init();