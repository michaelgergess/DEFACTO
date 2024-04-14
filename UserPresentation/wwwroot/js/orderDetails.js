
function updateCart(productIds) {
    var cart = JSON.parse(localStorage.getItem('CartItems')) ;

    productIds = productIds || [];

    var productIdsToRemove = productIds.join(',').replace(/^\[|\]$/g, '').split(',');

    productIdsToRemove.forEach(function (productId) {
        var index = cart.indexOf(productId);
        if (index !== -1) {
            cart.splice(index, 1);
        }
    });

    localStorage.setItem('CartItems', JSON.stringify(cart));
}
