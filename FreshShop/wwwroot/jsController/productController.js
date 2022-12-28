let product = {
    init: function () {
        product.registerEvent();
    },
    registerEvent: function () {

        $('#link-review-submit').on('click', function (e) {
            e.preventDefault();
            console.log("des");
            var message = $('#text-review').val();
            var productId = $('#text-review').data('productid');
            $.ajax({
                url: "http://localhost:5000/vi/Product/AddReview",
                data: { productId : productId, message : message },
                dataType: "json",
                type: "POST",
                success: function (res) {
                    if (res.status == true) {
                        location.reload();
                    }
                }
            });
        });
            
    }
}
product.init();