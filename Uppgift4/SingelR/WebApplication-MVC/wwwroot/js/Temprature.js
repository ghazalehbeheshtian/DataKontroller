﻿








const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7107/temperatureHub")
    .withAutomaticReconnect()
    .build();

connection.on("UpdateTemperature", function (deviceId, encryptedTemperature) {
    const temperature = DecryptTemperature(encryptedTemperature); // Implementera dekrypteringslogik här
    console.log(`${deviceId}: ${temperature}°C`);

    document.getElementById("loading").style.display = "none"; // dölj laddningsanimationen
    document.getElementById("connectionError").style.display = "none"; // dölj felmeddelandet

});

connection.start().catch(function (err) {
    // visa felmeddelandet om anslutningen misslyckas
    document.getElementById("connectionError").style.display = "block";
});

connection.onclose(function () {
    // visa felmeddelandet när anslutningen avbryts
    document.getElementById("connectionError").style.display = "block";
});

function DecryptTemperature(encryptedTemperature) {
    // Implementera dekrypteringslogik här
    return encryptedTemperature; // Denna bör ändras till riktig dekryptering
}



connection.on("Device1", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `Device 1 - says ${message}`;
});

connection.on("Device2", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `Device 2 - says ${message}`;
});
