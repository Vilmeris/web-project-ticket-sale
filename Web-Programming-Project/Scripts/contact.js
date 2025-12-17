//Bu kısımda bilgileri database'e kaydetmeyi henüz öğrenmediğimiz için sadece sayfa yenilenene kadar bilgileri tutuyor-->
//Eğer girilen mailler uyuşursa mesaj sayısını arttırıyor

var messageCount = 0;
var lastUserEmail = ""; 
var submissionTimes = []; 

function validateForm() {

    var form = document.forms["form1"];

    var name = form["nameText"].value;
    var lastName = form["lastNameText"].value;
    var email = form["emailText"].value;
    var subject = form["subjectText"].value;
    var message = form["messageText"].value;

    if (name === "" || lastName === "" || email === "" || subject === "" || message === "") {
        alert("Please fill in all the required parts.");
        return false;
    }

    if (lastUserEmail !== "" && lastUserEmail !== email) {
        messageCount = 0; 
        submissionTimes = []; 
    }

    messageCount++; 
    lastUserEmail = email;

    var userDate = form["dateText"].value;
    var userTime = form["timeText"].value;
    var combinedTimestamp = userDate + " " + userTime;
    submissionTimes.push(combinedTimestamp);

    var messageContainer = document.getElementById("Message");
    var countElement = document.getElementById("msgCount");
    var listElement = document.getElementById("msgList");

    countElement.textContent = messageCount; 

    for (var i = 0; i < submissionTimes.length; i++) {
        var newListItem = document.createElement("li"); 
        newListItem.textContent = submissionTimes[i]; 
        listElement.appendChild(newListItem); 
    }
    
    document.getElementById("form1").style.display = "none";
    messageContainer.style.display = "block";

    form.reset();
    return false;
}

function resetOutput() {
    document.getElementById("Message").style.display = "none";
    document.getElementById("form1").style.display = "block";
}
