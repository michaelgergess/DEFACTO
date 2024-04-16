
let element = document.getElementById("hiddenInputID")
if (element.tagName === "INPUT") {
    element.value = ID; // Set the value for input or textarea elements
}
let rat_btns = document.getElementsByName("rating")

for (let i = 0; i < rat_btns.length; i++) {
    rat_btns[i].addEventListener("click", () => {
        addRate(rat_btns[i]);  // You can still pass the object for clarity
    });
}

function addRate(obj) {
    
    $.ajax({
        type: "GET",
        url: "/Review/rateItem",
        data: {

            ReviewRate: obj.value,
            productID: ID,
            ReviewMessage:' '
        },
        success: function (result) {
            $('#rateMessage').html(result);
        },
        error: function (xhr, status, error) {
            console.error('can not add rate ', error);
        }
    });
}
function toggleReviewForm() {
    const reviewForm = document.getElementById("collapseExample");
    const isHidden = reviewForm.style.display === "none"; // Check current visibility

    if (isHidden) {
        reviewForm.style.display = "block"; // Show the form if hidden
    } else {
        reviewForm.style.display = "none"; // Hide the form if visible
    }
}

// Attach the click event listener to the button
const reviewButton = document.querySelector(".add-reviwe-btn");
reviewButton.addEventListener("click", toggleReviewForm);