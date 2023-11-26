
 
function setBio() {
    console.log("start")
    var bio = document.getElementById('bio'); 
    var _w = bio.offsetWidth;
    
    let i = 0;
    let speed = 800;
    let indent = 0;
    let timer =  setInterval(function () {
        if (i >=_w  ) {  console.log(_w)
            clearInterval(timer);
            setBio();
            indent = 0;
            i = 0;
        }
        bio.style.textIndent = (indent+=100)+"px" ;
        i += 100;
    }, speed);

    

}

