var ServerRoot = "http://localhost:5295";



function btn_Signup_Reset_Click() {
    document.getElementById("txt_Signup_Name").value = "";
    document.getElementById("txt_Signup_Email").value = "";
    document.getElementById("txt_Signup_Username").value = "";
    document.getElementById("txt_Signup_Password").value = "";
    // document.getElementById("msg_Signup_Response").value = "";
}
function btn_Login_Reset_Click() {
    document.getElementById("txt_Login_Username").value = "";
    document.getElementById("txt_Login_Password").value = "";
    // document.getElementById("msg_Login_Response").value = "";
}


function btn_Login_Submit_Click() {
    let username = document.getElementById("txt_Login_Username").value;
    let password = document.getElementById("txt_Login_Password").value;

    let json_To_Post = {
        method: "POST",
        body: JSON.stringify(
            {
                username: username, 
                password: password
            }
        ),
        headers: { "Content-Type": "application/json; charset=UTF-8; Access-Control-Allow-Origin: *" }
    }

    // Cleanup UI
    btn_Login_Reset_Click();
    
    // fetch api
    fetch(`${ServerRoot}/api/Login`, json_To_Post)
    .then((response) => response.json())
    .then((json) => {console.log(json); document.getElementById("msg_Login_Response").innerHTML = json.Message});

}

function btn_Signup_Submit_Click() {
    let name     = document.getElementById("txt_Signup_Name").value;
    let email    = document.getElementById("txt_Signup_Email").value;
    let username = document.getElementById("txt_Signup_Username").value;
    let password = document.getElementById("txt_Signup_Password").value;

    let json_To_Post = {
        method: "POST",
        body: JSON.stringify(
            {
                name: name, 
                email: email, 
                username: username, 
                password: password
            }
        ),
        headers: { "Content-Type": "application/json; charset=UTF-8; Access-Control-Allow-Origin: *" }
    }
    
    // Cleanup UI
    btn_Signup_Reset_Click();

    // fetch api
    fetch(`${ServerRoot}/api/Signup`, json_To_Post)
    .then((response) => response.json())
    .then((json) => {console.log(json); document.getElementById("msg_Signup_Response").innerHTML = json.Message});

}