const signalR = require('@microsoft/signalr');
const axios = require('axios');
const functionKey = '8vEAiL9HZPEHcubTZC5F77wDHxCu3fk3I_-BqkqWJbL8AzFu0fGLAg=='

const userId = "abcd1234"
// const negotiateEndpoint = 'http://localhost:7071/api/negotiate';
const negotiateEndpoint = 'https://awcworker.azurewebsites.net/api/negotiate';


// Function to negotiate and obtain SignalR connection info
const negotiateSignalR = async () => {
    try {
        const response = await axios.post(negotiateEndpoint, null, {
            headers: {
                'Content-Type': 'application/json',
                'userid': userId,
                'x-functions-key': functionKey
            }
        })
        const connectionInfo = response.data;
        console.log('Negotiate successful. Connection info:', connectionInfo);

        // Connect to SignalR hub using the obtained connection info
        connectToSignalR(connectionInfo.url, connectionInfo.accessToken);
    } catch (error) {
        console.error('Error during negotiation:', error);
    }
};

// Function to connect to the SignalR hub
const connectToSignalR = (url, accessToken) => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(url, { accessTokenFactory: () => accessToken })
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.start()
        .then(() => {
            console.log('SignalR connected');
        })
        .catch((error) => {
            console.error(`Error starting SignalR connection: ${error}`);
        });


    connection.onclose((error) => {
        console.log('SignalR connection closed');
        if (error) {
            console.error(`Connection closed with error: ${error}`);
        }
    });

    connection.on('newMessage', (message) => {
        console.log(`Received message from hub: ${JSON.stringify(message)}`);
    });
};

negotiateSignalR();
