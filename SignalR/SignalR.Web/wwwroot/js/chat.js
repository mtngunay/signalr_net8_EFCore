//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/chathub")
//    .build();

//connection.on("ReceiveMessage", (user, message) => {
//    const msg = document.createElement("div");
//    msg.textContent = `${user}: ${message}`;
//    document.getElementById("messagesList").appendChild(msg);
//});

//document.getElementById("sendButton").addEventListener("click", () => {
//    const user = document.getElementById("userInput").value;
//    const message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(err => console.error(err));
//});

//connection.start().catch(err => console.error(err));





document.addEventListener("DOMContentLoaded", () => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7233/chathub", {
            transport: signalR.HttpTransportType.WebSockets,
            withCredentials: true // Kimlik doğrulama bilgilerini gönderme izni
        })
        .build();

    connection.on("ReceiveMessage", (user, message) => {
        const msg = document.createElement("div");
        msg.textContent = `${user}: ${message}`;
        document.getElementById("messagesList").appendChild(msg);
    });

    // Bağlantıyı başlatın ve bağlantı kurulduğunda butona tıklama olayını bağlayın
    connection.start().then(() => {
        console.log("SignalR bağlantısı başarıyla başlatıldı.");

        document.getElementById("sendButton").addEventListener("click", () => {
            const user = document.getElementById("userInput").value;
            const message = document.getElementById("messageInput").value;

            // Bağlantının bağlı olup olmadığını kontrol edin
            if (connection.state === signalR.HubConnectionState.Connected) {
                connection.invoke("SendMessage", user, message)
                    .catch(err => console.error("Mesaj gönderilirken hata oluştu:", err));
            } else {
                console.error("SignalR bağlantısı bağlı değil.");
            }
        });
    }).catch(err => console.error("SignalR bağlantısı başlatılırken hata oluştu:", err));
});















