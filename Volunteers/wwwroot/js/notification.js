"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub").build();


var notificationBubble = document.getElementById("badge");
var count;

if (notificationBubble != null) {
    count = notificationBubble.textContent;
    notificationBubble.style.display = "block";
}



    connection.on("ReceiveMessage", function (user, message) {
        count += 1;
        notificationBubble.textContent = count;
        console.log(user, message)
    });

connection.start();