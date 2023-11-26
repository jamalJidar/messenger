var emojiBtn = document.getElementById('emojiBtn');

emojiBtn.addEventListener('click', function () {
    fetch('./emoji.json')
        .then((response) => response.json())
        .then((json) => console.log(json));
});

