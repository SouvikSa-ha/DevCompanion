var buttons = document.querySelectorAll(".notes");

for (var i = 0; i < buttons.length; i++) {
    buttons[i].addEventListener("click", function () {
        var item = document.querySelector('.active');
        (item) ? item.classList.remove('active') : '';
        this.classList.add('active');
    })
}
console.log("hello");