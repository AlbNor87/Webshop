document.querySelectorAll('.addButton').forEach(function(event) {
    
    event.addEventListener("click", addToCart);

  });

function addToCart(){

    alert("The product was added to your cart!");

}