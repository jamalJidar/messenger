"use strict";
var emojiButton = document.getElementById('emojiBtn');
let emojeTab = document.getElementById('emojeTab');
let emojeTabContent = document.getElementById('emojeTabContent');
let emojiCatgory = [];
let emojis = []
let inputText = document.getElementById('input')
emojiButton.addEventListener('click', function (event) {
     emojiCatgory = [];
    fetch('/js/emoji.json')
        .then((response) => response.json())
        .then((json) => {
            emojis = json.emojis;
          
            let cc = 0;
            emojis.forEach((e) => {
              //  if (cc > 10) return;
                var _item = emojiCatgory.find((element) => element.name === e.category);
                if (_item === undefined) {
                    emojiCatgory.push({ name: e.category, value: e.dataEmoji })
                   
                }
                
              cc = cc + 1;
            })
         
           
            /*
            <li class="nav-item">
					<a class="nav-link active" data-toggle="tab" href="#home">Home</a>
				</li>

                  <li class="nav-item">
    <a class="nav-link" id="contact-tab" data-toggle="tab" href="#contact" role="tab"
    aria-controls="contact" aria-selected="false">Contact</a>
  </li>
            */
            emojiCatgory.forEach((e, index) => {
                console.log(index)
                var li = document.createElement('li'); 
                var a = document.createElement('a');
                li.classList.add('nav-item');
                a.classList.add('nav-link');
                a.innerText = e.value;
                a.href = "#" + e.name;
                a.id = e.name + '-tab'
                var div = document.createElement('div')
                div.classList.add('tab-panel', 'fade');
                div.setAttribute('role', 'tabpanel')
                div.setAttribute('aria-labelledby', e.name + '-tab');
                //div.innerHTML = e.name + '-tab';
               
                a.addEventListener('click', function () {
                   
                /*    <div class=" show " id="home" role="tabpanel" aria-labelledby="home-tab">...</div>*/
                    div.classList.add('show', 'active');
                    var subCat = emojis.filter(x => x.category === e.name);
                   
                    subCat.forEach((s, sindex) => {
                        
                        var span = document.createElement('span'); 
                        span.innerHTML = s.dataEmoji;
                        span.style.cursor = 'pointer';
                        span.addEventListener('click', function () {
                             
                            inputText.value += s.dataEmoji
                            inputText.focus();
                        }); 
                        
                        div.appendChild(span)

                    });
                 
                })
                if (index === 0) {
                    emojeTabContent.appendChild(div)
                    a.classList.add('active');
                    div.classList.add('show', 'active');
                }
                else {


                }
                emojeTabContent.appendChild(div)
                li.append(a);
                emojeTab.appendChild(li)

            })
            
        });



})


