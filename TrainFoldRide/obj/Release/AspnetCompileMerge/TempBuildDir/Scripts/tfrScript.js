/*GLOBAL VAIABLES  START*/

var depStation = document.getElementById("depStation");
var arrStation = document.getElementById("arrStation");

var myData = [];
var arrData = [];

document.getElementById("depDropDown").style.display ="none";
document.getElementById("arrDropDown").style.display ="none";

/*events*/
var calcJourneyEvent = document.getElementById("calcButton").addEventListener('click', function() {
     depArrConnectionFunc();
     arrConnectionFunc();
}, false);


var depDropDownEvent = document.getElementById("depDropDown");
depDropDownEvent.addEventListener("change", confDepStationFunc, true);

var arrDropDownEvent = document.getElementById("arrDropDown");
arrDropDownEvent.addEventListener("change", confArrStationFunc, true);


/*GLOBAL VAIABLES  END*/
/*FUNCTIONS START*/
function depArrConnectionFunc()
    {
        depStation = document.getElementById("depStation").value;
        var depArrRequest = new XMLHttpRequest();
        depArrRequest.open("GET", "https://api.tfl.gov.uk/Stoppoint/Search/"+depStation+"?modes=tube&useMultiModalCall=false", true);
  
        depArrRequest.onload = function()
        {
            if (depArrRequest.readyState ==4 && depArrRequest.status ==200)
                {
                    myData = JSON.parse(depArrRequest.responseText); 
                    popDepDropDownFunc();
                }
            else
                {
                    myData = JSON.parse(depArrRequest.responseText); 
                    console.log(myData);
                    console.log("connection failed");
                }
        }
        depArrRequest.send();   
    }  

function arrConnectionFunc()
    {
        arrStation = document.getElementById("arrStation").value;
        var arrRequest = new XMLHttpRequest();
        arrRequest.open("GET", "https://api.tfl.gov.uk/Stoppoint/Search/"+arrStation+"?modes=tube&useMultiModalCall=false", true);
        
        arrRequest.onload = function()
        {
            if (arrRequest.readyState ==4 && arrRequest.status ==200)
                {
                    arrData = JSON.parse(arrRequest.responseText); 
                    popArrDropDownFunc();
                }
            else
                {
                    arrData = JSON.parse(arrRequest.responseText); 
                    console.log(arrData);
                    console.log("connection failed");
                }
        }
        arrRequest.send();   
    }   

function popDepDropDownFunc()
    {                
        console.log(myData); 
        document.getElementById("depDropDown").style.display ="block";
		var source = document.getElementById("depDropDown");
        
        for (var i = 0; i < myData.matches.length; i++) 
        {
            optionID = myData.matches[i].icsId;
            optionName = myData.matches[i].name;
            var option = document.createElement("option");
            
            option.innerHTML = optionName;
         
            option.setAttribute("name", optionName);
            option.setAttribute("value", optionID);
            source.appendChild(option);
        } 
    }

function popArrDropDownFunc()
    {                
        console.log(arrData); 
        document.getElementById("arrDropDown").style.display ="block";
		var source = document.getElementById("arrDropDown");
        
        for (var i = 0; i < arrData.matches.length; i++) 
        {
            optionID = arrData.matches[i].icsId;
            optionName = arrData.matches[i].name;
            var option = document.createElement("option");
            
            option.innerHTML = optionName;
         
            option.setAttribute("name", optionName);
            option.setAttribute("value", optionID);
            source.appendChild(option);
        } 
    }

function confDepStationFunc()
    {
        var depDropDownName = depDropDownEvent.options[depDropDownEvent.selectedIndex].getAttribute("name");
        document.getElementById("depStation").value = depDropDownName;
        document.getElementById("depDropDown").style.display ="none";
    }

function confArrStationFunc()
    {
        var arrDropDownName = arrDropDownEvent.options[arrDropDownEvent.selectedIndex].getAttribute("name");
        document.getElementById("arrStation").value = arrDropDownName;
        document.getElementById("arrDropDown").style.display ="none"; 
    }

/*FUNCTIONS END*/
/*PROGRAM START*/
/*PROGRAM END*/

