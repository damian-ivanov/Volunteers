"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
var count = 0;
connection.on("ReceiveMessage", function (user, message) {
   
    var li = document.createElement("li");
    var notificationBubble = document.getElementById("badge");
    notificationBubble.style.display = "block";
    count += 1;
    notificationBubble.textContent = count;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});