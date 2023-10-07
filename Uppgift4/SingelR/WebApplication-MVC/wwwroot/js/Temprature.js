




    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/temperatureHub")
        .build();

    connection.on("ReceiveTemperature", function (deviceId, encryptedTemperature) {
        const temperature = DecryptTemperature(encryptedTemperature); // Implementera dekrypteringslogik här
        console.log(`${deviceId}: ${temperature}°C`);
    });

    connection.start();

    function DecryptTemperature(encryptedTemperature) {
        // Implementera dekrypteringslogik här
        return encryptedTemperature; // Denna bör ändras till riktig dekryptering
    }


