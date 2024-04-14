$(document).ready(function () {
    fetchCartItems(); 
});
function fetchCartItems() {
    var CartItems = localStorage.getItem("CartItems");

    if (!CartItems) {
        CartItems = "[]";
    }
    $.ajax({
        url: '/Product/GetCartItems',
        type: 'GET',
        data: { "carts": CartItems },
        success: function (data) {
            $('#CartItems').html(data);
        },
        error: function (xhr, status, error) {
            console.error('Error fetching cart items:', error);
        }
    });
}
function removeItem(productId) {
    var cartItems = JSON.parse(localStorage.getItem('CartItems'));
    var inputId = "i-" + productId;
    var d = document.getElementById(inputId).value;
    var index = cartItems.indexOf(d);

    if (index !== -1) {
        cartItems.splice(index, 1);
        localStorage.setItem('CartItems', JSON.stringify(cartItems));
        fetchCartItems();
        updateCartQuantityDisplay();
    } else {
        alert('Item with ID ' + productId + ' not found in the cart.');
    }
}


// Function to retrieve the base price of the product
function getBasePrice(productId) {
    let priceInput = $("#price-" + productId);
    let priceValue = priceInput.val().replace('$', ''); // Remove the '$' symbol
    return parseFloat(priceValue);
}

// Function to increment quantity
function incrementQuantity(element, productId) {
    let quantitySpan = $(element).closest('.quantity-controls').find('.quantity-text');
    let currentValue = parseInt(quantitySpan.text().replace('Quantity ', ''));
    let quantityId = "quantity-" + productId;
    let maxQuantity = $("#" + quantityId).val();

    if (!isNaN(currentValue) && !isNaN(parseInt(maxQuantity)) && parseInt(maxQuantity) > currentValue) {
        quantitySpan.text('Quantity ' + (currentValue + 1));
        updateTotalPrice1(productId, currentValue + 1);
        $("#OrderQuantity-" + productId).val(currentValue + 1);
    }
    else {
            quantitySpan.text('Quantity 1');
        updateTotalPrice1(productId,  1);
        $("#OrderQuantity-" + productId).val(1);

    }
}

// Function to decrement quantity   
function decrementQuantity(element, productId) {
    let quantitySpan = $(element).closest('.quantity-controls').find('.quantity-text');
    let currentValue = parseInt(quantitySpan.text().replace('Quantity ', ''));

    if (!isNaN(currentValue) && currentValue > 1) {
        quantitySpan.text('Quantity ' + (currentValue - 1));
        updateTotalPrice1(productId, currentValue - 1);
        $("#OrderQuantity-" + productId).val(currentValue - 1);

    }
    else {
        quantitySpan.text('Quantity 1');
        updateTotalPrice1(productId,  1);
        $("#OrderQuantity-" + productId).val( 1);

    }
}

// Function to update the total price based on quantity
function updateTotalPrice1(productId, quantity) {
    let totalPriceElement = $("#total-price-" + productId);
    let basePrice = getBasePrice(productId);
    let totalPrice = basePrice * quantity;

    totalPriceElement.text(totalPrice.toFixed(2));
}
function updateIsSelected(checkbox, index, productId) {
    var selectedItemsInput = document.getElementsByName("selectedItems[" + index + "].IsSelected")[0];
    selectedItemsInput.value = checkbox.checked;
   
}
// Function to update the total price based on the isChecked status and product ID
//// Function to update the total price based on the isChecked status and product ID
//function updateTotalPrice(isChecked, productId) {
//    var totalPriceElement = document.getElementById("total-price-" + productId);
//    var totalPriceElementValue = parseFloat(document.getElementById("total_price").textContent.trim()) || 0;
//    var quantityInput = document.getElementById("OrderQuantity-" + productId); // Get the quantity input element
//    var quantity = parseInt(quantityInput.value);
//    var basePrice = getBasePrice(productId);

//    // Calculate the total price
//    var totalPrice = quantity * basePrice;
//    if (isChecked) {
//        totalPriceElementValue += totalPrice;
//    } else {
//        totalPriceElementValue -= totalPrice;
//    }

//    totalPriceElement.textContent = "$" + totalPrice.toFixed(2);
//    document.getElementById("total_price").textContent = totalPriceElementValue.toFixed(2);
//}

//// Function to handle changes in the quantity input
//function handleQuantityChange(productId) {
//    var quantityInput = document.getElementById("OrderQuantity-" + productId);
//    quantityInput.addEventListener("change", function () {
//        // Call updateTotalPrice function when the quantity input changes
//        updateTotalPrice(false, productId); // Set isChecked to false assuming the checkbox is unchecked
//    });
//}

// Example usage:
// Call handleQuantityChange function for each product where the quantity input might change







function getSizes(productId) {
    var colorName = $("#color-" + productId + " option:selected").val();
    var sizeID = "size-" + productId;
    $.ajax({
        url: '/product/GetSizes',
        type: 'GET',
        data: { "ProductId": productId, "ColorName": colorName },
        success: function (data) {
            // Check if there are sizes available
            if (data && data.length > 0) {
                var items = '';
                $.each(data, function (index, size) {
                    items += '<option value="' + size + '">' + size + '</option>';
                });
                $("#" + sizeID).html(items);
                $("#" + sizeID).parent().show(); // Show the parent div containing the dropdown
                $('.quantity-text').text('Quantity 1');
                getQuantityAndPrice(productId, colorName);
            } else {
                $('.quantity-text').text('Quantity 1');
                $("#" + sizeID).html('');
                // Set the dropdown to have no selected value
                $("#" + sizeID).val('');
                getQuantityAndPrice(productId, colorName);
            
                $("#" + sizeID).parent().hide(); // Hide the parent div containing the dropdown
            }
        },
        error: function (xhr, status, error) {
            console.error('Error fetching sizes:', error);
        }
    });
}

function getQuantityAndPrice(productId, colorName) {
  
    var sizeName = $("#size-" + productId + " option:selected").val();
    $.ajax({
        url: '/product/GetQuantityAndPrice', 
        type: 'GET',
        data: { "ProductId": productId, "ColorName": colorName, "SizeName": sizeName },
        success: function (data) {
            $("#quantity-" + productId).val(data.quantity);
           // $("#itemID-" + productId).val(data.itemId);
            $("#check-" + productId).val(data.itemId);

            $("#price-" + productId).val("$" + data.price.toFixed(2));
            $("#total-price-" + productId).text("$" + data.price.toFixed(2));
            $('.quantity-text').text('Quantity 1');

        },
        error: function (xhr, status, error) {
            console.error('Error fetching quantity and price:', error);
        }
    });
}

