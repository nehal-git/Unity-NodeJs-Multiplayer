//Live Server Config Start

const { Router } = require('express');
const express = require('express');
const socketIO = require('socket.io');
const PORT = process.env.PORT || 3000;
const INDEX = '/index.html';

const server = express()
    .use((req, res) => res.sendFile(INDEX, { root: __dirname }))
    .listen(PORT, () => console.log(`Listening on ${PORT}`));
const io = socketIO(server);

//Live Server Config End

//Local Server Config Start

// const app = require('express')();
// const http = require('http').createServer(app);
// var io = require('socket.io')(http);
// http.listen(3000, function(){
//     console.log('listening on *:300');
//   });
//Local Server Config End


var clients = [];


io.on('connection', (socket) => {
   

    var currentplayer = {};
    currentplayer.name = 'unknown';

    socket.on('player connect', function (data) {
        console.log(' Player ' + currentplayer.name + ' : Conencted');
        for (var i = 0; i < clients.length; i++) {
            var playerConnected = {
                name: clients[i].name,
                position: clients[i].position,
                rotation: clients[i].rotation,
            };
            
        }
        socket.emit('other player connected', playerConnected);
        console.log(currentplayer.name + ' emit: Other Player Connected');
    });

    socket.on('play', function (data) {

        console.log(currentplayer.name + 'recv : play ');

        if (clients.length == 0) {

            

        }
        currentplayer = {
            name: data.name,
            position: data.position,
            rotation: data.rotation,
        };
        clients.push(currentplayer);
        socket.emit('play', currentplayer);
        socket.broadcast.emit('play', currentplayer);

    });
 


    socket.on('position', function (data) {

        currentplayer.position = data.position;
        socket.broadcast.emit('position', currentplayer);

    });
    socket.on('rotation', function (data) {

        currentplayer.rotation = data.rotation;
        socket.broadcast.emit('rotation', currentplayer);

    });

    socket.on('disconnect', function () {

        console.log("A Player Disconnect");
        socket.broadcast.emit('other player disconnected', currentplayer);
        for (var i = 0; i < clients.length; i++) {

            if (clients[i].name === currentplayer.name) {

                clients.splice(i, 1);
            }
        }


    });
});



