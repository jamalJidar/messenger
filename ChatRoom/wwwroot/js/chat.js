"use strict";
let chat_list = document.getElementById('chat-list');

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
let modalbody = document.getElementById('modalbody');
let exampleModal = document.getElementById('exampleModal');
let listResualtSearch = document.getElementById('listResualtSearch')
var callBio = false;
let contactList = [];
let listMessages = [];
let groupList = [];
let user = {};
let CurrentUser = ''
let _messageType = 'private';
let GroupName = '';
let audio = `<audio controls="controls" autoplay="autoplay" style="display:none">
			<source src="/sms/sms.mp3" type="audio/mp3">
		</audio>`;

let attach =` this message  has attach file.
  link file  download , or preview media, or get link  meta data ;
 ` 
connection.start().then(function () {

    notifyMe()
    document.getElementById("sendButton").disabled = false;
    CurrentUser = ''
    connection.invoke("Init").then(function () {

        connection.invoke("UserProfile").catch(function (err) {
            return console.error(err.toString());
        });
        //connection.invoke("ListContact", "").catch(function (err) {
        //    let p = document.createElement('p');
        //    p.innerText = 'خطا در بارگذاری لیست مخاطبین';
        //    p.style.color = 'red';
        //    p.style.textAlign = "center";
        //    p.style.width = "100%";
        //    p.style.margin = "20px 0px 0px 0px";
        //    DOM.chatList.appendChild(p);
        //    return console.error(err.toString());
        //});


    }




    ).catch(function () { })

}).catch(function (err) {
    return console.error(err.toString());
});

let _upload =document.getElementById("_upload")
_upload.addEventListener('click', function () {
    console.log("_upload")
    var file = document.getElementById("test").files[0];
    console.log(file)
    let data = new FormData();
    data.append("formFiles",file);
    connection.invoke("Upload",data).then(function (ok) {console.log(ok) }).catch(function (err) {
        return console.error(err.toString());
    });


})
//connection.stop().then(() => {
//   // alert('Server has disconnected');
//});
connection.on("RequestOnlineUserAsSenderToReciver", function (senderId) {
    connection.invoke("ResponseOnlineUserReciver", senderId, user.id).then(function () { }).catch(function (error) { return console.log(error) });

})
connection.on("RequestOnlineUserAsReciverToSender", function (reciverId) {
    connection.invoke("ResponseOnlineUserAsReciverToSender", reciverId).then(function () { }).catch(function (error) { return console.log(error) });


})
connection.on("CheckOpenChatPrivateUser", function (message) {
     
    console.log((message.senderId === CurrentUser))
    console.log(message)
    connection.invoke("CheckOpenChatPrivateUser", message, (message.senderId === CurrentUser)).then(function () { }).catch(function (err) {

       
        return console.error(err.toString());
    });




});

window.addEventListener('beforeunload', (event) => {

    event.preventDefault();
   // connection.invoke("DisconnectAsync").catch(function (err) {return console.error(err.toString());});
    event.returnValue = '';
});

connection.on("ReqestStatusOnlineUser", function (sernderid) {

    connection.invoke("ResponseStatusOnlineUser", sernderid, user.id).then(function () { }).catch(function () { });
});

connection.on("ReceiveMessage", function (msg, check = false) {

 

    let showMessage = false
    if (msg.senderId === user.id)
        showMessage = true;

    else if (msg.senderId === CurrentUser) showMessage = true
    //to do check online user  
    if (!showMessage) { return; }


    var div = document.createElement("div");
    let htmlForGroup = `
	<div class="small font-weight-bold text-primary">
		${msg.senderUserName}
	</div>
	`;


    let sendStatus = ''

    switch (parseInt(msg.messageStatus)) {
        case 0:
            {
                sendStatus = `<i class="fa fa-check-circle"></i>`
                break;
            }
        case 1:
            {
                sendStatus = `<i class="fa fa-check-circle"></i>`
                            + `<i class="fa  fa-check-circle"></i>`
                break;
            }

        case 2:
            {
                sendStatus = `<i class="fas fa fa-check-circle"></i>`
                            + `<i class="fas fa fa-check-circle"></i>`
                break;
            }
    }
    msg.senderUserName === user.name ? div.classList.add('align-self-end', 'self') :
        div.classList.add('align-self-start')
    div.classList.add('my-1', 'mx-3');
    div.classList.add('rounded');
    div.classList.add('bg-white', 'shadow-sm');
    div.classList.add('message-item', 'p-1');
    div.innerHTML = `<div class="options">
		 	<a href="#"><i class="fa fa-angle-down text-muted px-2"></i>salam</a>
		</div>
		${msg.messageType == 0 ? htmlForGroup : ""}
		<div class="d-flex flex-row">
			<div class="body m-1 mr-2">
            <div class="message-medai">
            ${msg.document ? msg.documentFile :""}
            </div>
            ${msg.text}</div>
	
            <div class="time ml-auto small text-right flex-shrink-0 align-self-end text-muted" style="width:75px;">
				${mDate(msg.time).getTime()}
				${(msg.senderUserName === user.name) ? sendStatus : ""}
			</div>
		</div>`;
    document.getElementById("messages").appendChild(div);
    DOM.messages.scrollTo(0, DOM.messages.scrollHeight);
    var ms = listMessages.find(x => x.id === mgs.id && x.messageStatus !== 2 && senderId !== user.id);
    document.body.append(audio);



    listMessages.push(msg)

});
connection.on("ListContact", function (list) {
   DOM.chatList.innerHTML = ''
    for (var i in list) {
         let div = document.createElement('div');
        div.classList.add('chat-list-item', 'd-flex')
        div.classList.add('flex-row', 'w-100')
        div.classList.add('p-2', 'border-bottom')
        let TypeMessage = list[i].messageType === 1 ? `<span class="message-type fa fa-group"> </span>` : '';

        let online = `<i class="fas fa fa-circle"></i>`;
        div.innerHTML = ` 
        ${TypeMessage}
        <img src="${list[i].profileImage}" alt="Profile Photo" class="img-fluid rounded-circle mr-2" style="height:50px;width: 50px;">
		
            <div class="w-50">
				<div class="name" >${list[i].name}</div>
                <div class="small last-message"> ${list[i].count_LastMessage.value} 
                <i class="fa fa-check-circle mr-1" style="display:none"></i>
                
                </div>
           	</div>
			 <div class="flex-grow-1 text-right">
				<div class="small time">${list[i].lastSeen}</div>
				${list[i].count_LastMessage.key >
                0 ? "<div class=\"badge badge-success badge-pill small\" id=\"unread-count\">" + list[i].count_LastMessage.key + "</div>" : ""}
			  ${list[i].isOnline === false ? '' : online}
                
                </div>
	 
		`;

        let id = list[i].id
        let messageType = list[i].messageType;
        //list[i].messageType
        div.addEventListener('click', function () {

            CurrentUser = id;

            if (messageType === 0) {
                connection.invoke("LsitMessage", id, user.id).catch(function (err) { });

                _messageType = 'private'
            }
            else if (messageType === 1) {
                _messageType = 'group'
                console.log(id)
                connection.invoke("LsitMessageGruop", id).catch(function (err) {


                });

            }



        })
        DOM.chatList.append(div);
    }
    DOM.messages.scrollTo(0, DOM.messages.scrollHeight);
});
connection.on("LsitMessage", function (_messages, uid) {
    let MenuList = [{ name: 'copy', value: 'copy' },
    { name: 'delete', value: 'delete' },
    { name: 'reply', value: 'reply' }
    ];

    let test1 = [{ name: 'copy' },
    { name: 'delete' },
    { name: 'reply' }
    ];

    DOM.messages.innerHTML = ''

    _messages.forEach((msg) => {

        let htmlForGroup = `
	<div class="small font-weight-bold text-primary">
		${msg.senderUserName}
	</div>
	`;
        let menu = ``;
        var _delete = document.createElement('a');
        _delete.innerText = 'text';
        _delete.addEventListener('click', function () {
            alert('dasdas')
        });

        `<a class="dropdown-item" name='delete' > Delete</a >`
        /*
        <a class="dropdown-item" name='copy'>Copy</a>
                              <a class="dropdown-item" name='delete' > Delete</a >
                              <a class="dropdown-item" neme='reply'>Reply</a>
        */







        let sendStatus = `<i class="${parseInt(msg.messageStatus) < 2 ? "fa " : "fas fa"} fa-check-circle"></i>`;
        sendStatus += parseInt(msg.messageStatus) > 0 ? `<i class="${parseInt(msg.messageStatus) < 2 ? "fa" : "fas fa"} fa-check-circle"></i>` : ``;

        DOM.messages.innerHTML += `
	<div class="align-self-${msg.senderUserName === user.name ? "end self" : "start"} p-1 my-1 mx-3 rounded bg-white shadow-sm message-item">
		<div class="options">
			 
             <div class="nav-item dropdown ml-auto">
					<a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">
						<i class="fa fa-angle-down text-muted px-2""></i></a>
				 <div class="dropdown-menu dropdown-menu-right">
                        <a class="dropdown-item" onclick="alert(' Coming Soon ... ')"> کپی	</a>
						<a class="dropdown-item" onclick="alert(' Coming Soon ... ')">پاسخ</a>
						<a class="dropdown-item" onclick="alert(' Coming Soon ... ')">ارسال به دیگری</a>
						<a class="dropdown-item" onclick="alert(' Coming Soon ... ')">ویرایش</a>
						<a class="dropdown-item" onclick="alert(' Coming Soon ... ')">حذف</a>

                 </div>
				</div>
		</div>
		${msg.messageType == 0 ? htmlForGroup : ""}
		<div class="d-flex flex-row">
			<div class="body m-1 mr-2">
            <div class="message-medai"> 
                ${msg.document ? msg.documentFile : ""}
            </div>    

            ${msg.text}</div>
			<div class="time ml-auto small text-right flex-shrink-0 align-self-end text-muted" style="width:75px;">
				${mDate(msg.time).getTime()}
				${(msg.senderUserName === user.name) ? sendStatus : ""}
			</div>
		</div>
	</div>
	`;


        listMessages.push(msg)
    });
    DOM.messages.scrollTo(0, DOM.messages.scrollHeight);
    mClassList(DOM.messageAreaOverlay).add("d-none");
    mClassList(DOM.inputArea).contains("d-none", (elem) => elem.remove("d-none").add("d-flex"));


});
connection.on("LsitMessageGroup", function (_group, _messages) {


    GroupName = _group.name;
    DOM.messageAreaName.innerHTML = _group.name

    DOM.messageAreaPic.src = _group.profileImg

    DOM.messages.innerHTML = ''

    _messages.forEach((msg) => {


        let htmlForGroup = `
	<div class="small font-weight-bold text-primary">
		${msg.senderUserName}
	</div>
	`;
        DOM.messages.innerHTML += `
	<div class="align-self-${msg.senderUserName === user.name ? "end self" : "start"} p-1 my-1 mx-3 rounded bg-white shadow-sm message-item">
		<div class="options">
			 
             <div class="nav-item dropdown ml-auto">
					<a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">
						<i class="fa fa-angle-down text-muted px-2""></i></a>
				 <div class="dropdown-menu dropdown-menu-right">
              

                 </div>
				</div>
		</div>
		
		<div class="d-flex flex-row">
			<div class="body m-1 mr-2">
            <div class="message-medai">   </div>    

            ${msg.text}</div>
			<div class="time ml-auto small text-right flex-shrink-0 align-self-end text-muted" style="width:75px;">
				
				
			</div>
		</div>
	</div>
	`;


        listMessages.push(msg)
    });



    DOM.messages.scrollTo(0, DOM.messages.scrollHeight);
    mClassList(DOM.messageAreaOverlay).add("d-none");
    mClassList(DOM.inputArea).contains("d-none", (elem) => elem.remove("d-none").add("d-flex"));
})


function changepic(div) {
    document.getElementById(div).src = "https://upload.wikimedia.org/wikipedia/commons/1/1b/Square_200x200.png";
}

function notifyMe() { }



connection.on("UserProfile", function (result) {

    user.name = result.name;
    user.id = result.id;
    user.number = result.number;
    user.pic = result.pic

    //connection.on("ContctList", function (list) {
    //    contactList.push(list.contact)
    //    messages.push(list.message)
    //    //  init(result);
    //});

});



document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("input").value;
    if (message.length > 0) {

        if (_messageType === 'private') {
            connection.invoke("sendMessage", CurrentUser, message, null).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
            
            connection.invoke("sendMessageG", GroupName, message).catch(function (err) {
                return console.error(err.toString());
            });
        }



        document.getElementById("input").value = ''
        event.preventDefault();
        document.getElementById('typeMeeage').innerHTML = "";

    }

});
//document.getElementById("addNewGroup").addEventListener("click", function (event) {

//    alert('addNewGroup')

//});

 
document.getElementById("input").addEventListener("keydown", function (event) {
    if (_messageType === 'private') {
        if (event.keyCode !== 13) {

            connection.invoke("UserType", CurrentUser, true).then(function () { }).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
            var message = document.getElementById("input").value;
            if (message.length > 0) {
                connection.invoke("sendMessage", CurrentUser, message, null).catch(function (err) {
                    return console.error(err.toString());
                });
                document.getElementById("input").value = ''

                document.getElementById('typeMeeage').innerHTML = "";

            }
            event.preventDefault();
        }
    }
});
connection.on("messageMedai", function (recvId,message , messageid ) {
    
    if (_messageType === 'private') {
       
        connection.invoke("UserType", recvId, true).then(function () { }).catch(function (err) {
                return console.error(err.toString());
            });
         
            
            if (message.length > 0) {
                connection.invoke("sendMessage", recvId, message , messageid).catch(function (err) {
                    return console.error(err.toString());
                });
                document.getElementById("Caption").value = ''

                document.getElementById('typeMeeage').innerHTML = "";

           
             
        }
    }


});
connection.on("UserType", function (res, type) {

    if (res === CurrentUser && type == true) {
        document.getElementById('typeMeeage').innerHTML = "در حال نوشتن ...";
    }
    else {
        document.getElementById('typeMeeage').innerHTML = "";
    }


});





//document.getElementById('openListContact').addEventListener('click', function () {

//});

connection.on("ShowProfileOtherUser", function (result) {

    DOM.messageAreaPic.src = result.profileImage
    DOM.messageAreaName.innerHTML = result.name
    DOM.messageAreaDetails.innerHTML = result.lastSeen;
    if (!callBio) {
        //setBio();
        callBio = true
    }
});

document.getElementById('input').addEventListener('mouseout', function () {


})

document.getElementById('BtnNewGroup').addEventListener('click', function () {
    let _groupName = document.getElementById('InputGroupName').value;

    connection.invoke("addNewGroup", _groupName).catch(function (err) {
        return console.error(err.toString());
    });


});

document.getElementById('btnSearch').addEventListener('click', function () {

    let searchValue = document.getElementById('inputSearch').value;
    if (searchValue.length < 2)
        listResualtSearch.style.display = 'none'
    connection.invoke("Search", searchValue).catch(function (err) {
        return console.error(err.toString());
    });


});

connection.on("Search", function (result) {
    console.log("users")


    //    let listResualtSearch = document.getElementById('listResualtSearch');
    console.table(result)
    /*
        <li class="list-group-item">Cras justo odio</li>
    	
    */
    result.forEach((_user) => {
        let _li = document.createElement('li');
        _li.classList.add('list-group-item');
        _li.innerText = _user.fullName;
        listResualtSearch.appendChild(_li)
        listResualtSearch.style.display = 'block'
    });


});



connection.on("addNewGroup", function (result) {

    alert(result)
});



//config contact  - create element , ...


let getById = (id, parent) => parent ? parent.getElementById(id) : getById(id, document);
let getByClass = (className, parent) => parent ? parent.getElementsByClassName(className) : getByClass(className, document);

const DOM = {
    chatListArea: getById("chat-list-area"),
    messageArea: getById("message-area"),
    inputArea: getById("input-area"),
    chatList: getById("chat-list"),
    messages: getById("messages"),
    chatListItem: getByClass("chat-list-item"),
    messageAreaName: getById("name", this.messageArea),
    messageAreaPic: getById("pic", this.messageArea),
    messageAreaNavbar: getById("navbar", this.messageArea),
    messageAreaDetails: getById("details", this.messageAreaNavbar),
    messageAreaOverlay: getByClass("overlay", this.messageArea)[0],
    messageInput: getById("input"),
    profileSettings: getById("profile-settings"),
    profilePic: getById("profile-pic"),
    profilePicInput: getById("profile-pic-input"),
    inputName: getById("input-name"),
    username: getById("username"),
    displayPic: getById("display-pic"),
    typeMessage: getById("typeMessage"),
};

let mClassList = (element) => {
    return {
        add: (className) => {
            element.classList.add(className);
            return mClassList(element);
        },
        remove: (className) => {
            element.classList.remove(className);
            return mClassList(element);
        },
        contains: (className, callback) => {
            if (element.classList.contains(className))
                callback(mClassList(element));
        }
    };
};

// 'areaSwapped' is used to keep track of the swapping
// of the main area between chatListArea and messageArea
// in mobile-view
let areaSwapped = false;

// 'chat' is used to store the current chat
// which is being opened in the message area
let chat = null;

// this will contain all the chats that is to be viewed
// in the chatListArea
let chatList = [];

// this will be used to store the date of the last message
// in the message area
let lastDate = "";

// 'populateChatList' will generate the chat list
// based on the 'messages' in the datastore



let showChatList = () => {
    if (areaSwapped) {
        mClassList(DOM.chatListArea).remove("d-none").add("d-flex");
        mClassList(DOM.messageArea).remove("d-flex").add("d-none");
        areaSwapped = false;
    }
};



let showProfileSettings = () => {
    DOM.profileSettings.style.left = 0;
    DOM.profilePic.src = user.pic;
    DOM.inputName.value = user.name;
};

let hideProfileSettings = () => {
    DOM.profileSettings.style.left = "-110%";
    //  DOM.username.innerHTML = user.name;
};

window.addEventListener("resize", e => {
    if (window.innerWidth > 575) showChatList();
});

function init(user) {
    console.log(user)
    DOM.username.innerHTML = user.name;
    //DOM.displayPic.src = user.pic;
    //DOM.profilePic.stc = user.pic;
    DOM.profilePic.addEventListener("click", () => DOM.profilePicInput.click());
    DOM.profilePicInput.addEventListener("change", () => console.log(DOM.profilePicInput.files[0]));
    DOM.inputName.addEventListener("blur", (e) => user.name = e.target.value);
    generateChatList();

    /*	console.log("Click the Image at top-left to open settings.");*/
};



//datastory





// message status - 0:sent, 1:delivered, 2:read


