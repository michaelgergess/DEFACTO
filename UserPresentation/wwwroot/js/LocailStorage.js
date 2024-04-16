


let imgs = document.getElementsByClassName("heartImg");
let favorites = localStorage.getItem('favorites');
let oldData = favorites ? JSON.parse(favorites) : [];


for (var i = 0; i < imgs.length; i++) {
    var image = document.getElementsByClassName('heartImg')[i];
    let index = oldData.indexOf(image.alt);
    // if localstorage contain image
    if (index !== -1) {
      
        image.src = "/images/img/icon/Blackheart.png";

    }
    else {
        image.src = "/images/img/icon/heart.png";
       

    }

}

function saveFavorites(event, productId) {
    event.preventDefault();
    let myAnchor = document.getElementById("fav-ids");
    let myAnchorInsideBar = document.getElementsByClassName("fav-in-sidbar");

    let image = document.getElementById('fav-' + productId); 
    let inputId = "i-" + productId;

    let d = document.getElementById(inputId).value;

    let favorites = localStorage.getItem('favorites');
    let oldData = favorites ? JSON.parse(favorites) : [];

    let index = oldData.indexOf(d);
    if (index !== -1) {
        oldData.splice(index, 1);
        image.src = "/images/img/icon/heart.png";
      
    }
    else {
        oldData.push(d);
        image.src = "/images/img/icon/Blackheart.png";
        
    }
    
    
   
    localStorage.setItem('favorites', JSON.stringify(oldData));

    myAnchor.value = `${favorites}`;
    myAnchorInsideBar[0].value = `${favorites}`;
    getFavorites();
}
    $(document).ready(function() {
        $('#filterInput').on('input', function () {
            var query = $(this).val();
            $.ajax({
                url: '/product/FilterFavorites',
                type: 'GET',
                data: { query: query },
                success: function (result) {
                    $('#favoritesContainer').html(result);
                },
                error: function (xhr, status, error) {
                    console.error(xhr.responseText);
                }
            });
        });
    });


function restHeartIcon() {
    let imgs = document.getElementsByClassName("heartImg");
    let favorites = localStorage.getItem('favorites');
    let oldData = favorites ? JSON.parse(favorites) : [];


    for (var i = 0; i < imgs.length; i++) {
        var image = document.getElementsByClassName('heartImg')[i];
        let index = oldData.indexOf(image.alt);
        // if localstorage contain image
        if (index !== -1) {

            image.src = "/images/img/icon/Blackheart.png";

        }
        else {
            image.src = "/images/img/icon/heart.png";


        }

    }
}
