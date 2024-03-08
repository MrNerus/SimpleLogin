var ServerRoot = "http://localhost:5295";

function btn_Submit_Click() {
    let username = document.getElementById("txt_Username").value;
    let password = document.getElementById("txt_Password").value;

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
    
    // fetch api
    fetch(`${ServerRoot}/api/Login`, json_To_Post)
    .then((response) => response.json())
    .then((json) => console.log(json));

}