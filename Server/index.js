//Live Server Config Start

const express = require('express')();
const { nanoid } = require('nanoid')
//const socketIO = require('socket.io');
//const PORT = process.env.PORT || 3000 ;
//const server = express().listen(PORT, () => console.log('Server Started'));
//const io = socketIO(server);

const server = require('http').createServer(express);

const io = require('socket.io')(server, {

    // below are engine.IO options
    pingInterval: 20000,
    pingTimeout: 25000,
    cookie: false
});

server.listen(process.env.PORT || 3000, () => console.log('Server Started'));


var Player = require('./Classes/Player.js');
var Room = require('./Classes/Room.js');
var players = [];
var sockets = [];
var rooms2 = [];
var rooms4 = [];

io.on('connection', (socket) => {
    console.log('A Player Conencted');
    var currentRoom = 'default';

    //Creating New Player
    var player = new Player();
    var thisPlayerID;
    var thisPlayerName;
    socket.on('create', function () {


        // Setting New Player ID
        thisPlayerID = player.id;
        thisPlayerName = player.name;
        players[thisPlayerID] = player;
        sockets[thisPlayerID] = socket;
        socket.emit('register', { id: thisPlayerID });




    })

    //socket.on('rejoin', function (data) {

    //    console.log(data);
    //    if (data != undefined) {
    //        thisPlayerID = data;
    //        players[thisPlayerID] = player;
    //        sockets[thisPlayerID] = socket;
    //        socket.emit('spawn_other_players');

    //         socket.emit('rejoin', {msg: 'reconnected'});

    //        console.log('A Player Conencted:', data);
    //    }

    //})

   



    socket.on('two_player', function (data) {

        console.log(data);
        if (rooms2.length != 0) {
            let room_index;
            let canjoin;
            for (var i = 0; i < rooms2.length; i++) {
                room_index = i;
                if (rooms2[i].count < 2) { //join room

                    canjoin = true;
                   
                    break;
                }
                else {
                    canjoin = false;

                }
            }
            if (canjoin == true)
            {

                const roomID = rooms2[room_index].roomID;
                socket.join(roomID);
                currentRoom = roomID;
                rooms2[room_index].playerid.push(thisPlayerID);
                rooms2[room_index].count += 1;
                console.log("Joined ", rooms2[room_index]);
                socket.emit('message', {
                    //room: rooms2[rooms2.length - 1]
                    roomID: rooms2[room_index].roomID,
                    playerID: rooms2[room_index].playerid,
                    count: rooms2[room_index].count
                });

                io.to(rooms2[room_index].roomID). emit('checkPlayerCount', { count: rooms2[room_index].count });
                //socket.broadcast.emit('message', {
                //    roomID: rooms2[room_index].roomID,
                //    playerID: rooms2[room_index].playerid,
                //    count: rooms2[room_index].count

                //});
                return;
            }
            if (canjoin == false)
            {
                var roomID = CreateRoomID();
               
                socket.join(roomID);
                currentRoom = roomID;

                const room = new Room();
                room.roomID = roomID;
                room.playerid.push(thisPlayerID);
                room.count = 1;
                rooms2.push(room);
                console.log("Created ", rooms2[room_index+1]);
                socket.emit('message', {
                    //room: rooms2[rooms2.length - 1]
                    roomID: rooms2[room_index + 1].roomID,
                    playerID: rooms2[room_index + 1].playerid,
                    count: rooms2[room_index + 1].count
                });
                //socket.broadcast.emit('message', {
                //    roomID: rooms2[room_index + 1].roomID,
                //    playerID: rooms2[room_index + 1].playerid,
                //    count: rooms2[room_index + 1].count

                //});
                return;
            }
            //if (rooms2[rooms2.length - 1].count < 2) { //join room
            //    const roomID = rooms2[rooms2.length - 1].roomID;

            //    socket.join(roomID);

            //    currentRoom = roomID;
            //    rooms2[rooms2.length - 1].playerid.push(thisPlayerID);
            //    rooms2[rooms2.length - 1].count += 1;
            //    console.log("Joined 2 Player Room:", rooms2[rooms2.length - 1]);
            //    socket.emit('message', {
            //      //room: rooms2[rooms2.length - 1]
            //        roomID: rooms2[rooms2.length - 1].roomID,
            //        playerID: rooms2[rooms2.length - 1].playerid,
            //        count: rooms2[rooms2.length - 1].count
            //    });
            //    socket.broadcast.emit('message', {
            //        roomID: rooms2[rooms2.length - 1].roomID,
            //        playerID: rooms2[rooms2.length - 1].playerid,
            //        count: rooms2[rooms2.length - 1].count

            //    });
            //    return;
            //}
            //else {
            //    //create room
            //    var roomID = CreateRoomID();

            //    socket.join(roomID);
            //    currentRoom = roomID;

            //    const room = new Room();
            //    room.roomID = roomID;
            //    room.playerid.push(thisPlayerID);
            //    room.count = 1;
            //    rooms2.push(room);
            //    console.log("Created 2 Player Room: ", rooms2[rooms2.length - 1]);
            //    socket.emit('message', {
            //      //room: rooms2[rooms2.length - 1]
            //        roomID: rooms2[rooms2.length - 1].roomID,
            //        playerID: rooms2[rooms2.length - 1].playerid,
            //        count: rooms2[rooms2.length - 1].count
            //    });
            //    socket.broadcast.emit('message', {
            //        roomID: rooms2[rooms2.length - 1].roomID,
            //        playerID: rooms2[rooms2.length - 1].playerid,
            //        count: rooms2[rooms2.length - 1].count

            //    });
            //    return;
            //}



            // }


        }
        else {
            //create room when room length is null or zero
            const roomID = CreateRoomID();

            socket.join(roomID);
            currentRoom = roomID;

            const room = new Room();
            room.roomID = currentRoom;
            room.playerid.push(thisPlayerID);
            room.count = 1;
            rooms2.push(room);

            console.log("New 2 Player Room: ", rooms2[rooms2.length - 1], " AI available: ", data.isAI);
            //console.log(rooms.length);
            socket.emit('message', {
                roomID: rooms2[rooms2.length - 1].roomID,
                playerID: rooms2[rooms2.length - 1].playerid,
                count: rooms2[rooms2.length - 1].count

            });
            //socket.broadcast.emit('message', {
            //    roomID: rooms2[rooms2.length - 1].roomID,
            //    playerID: rooms2[rooms2.length - 1].playerid,
            //    count: rooms2[rooms2.length - 1].count

            //});

        }
        // console.log("Rooms length End ", rooms2.length);
    });



    socket.on('createAI', function () {



    })

    socket.on('four_player', function () {
        //  console.log("Rooms length Start ", rooms4.length);

        if (rooms4.length != 0) {
            let room_id;
            // for (var i = 0; i < rooms.length; i++) {

            if (rooms4[rooms4.length - 1].count < 4) { //join room
                let roomID = rooms4[rooms4.length - 1].roomID;

                socket.join(roomID);
                //   socket.emit('register', { id: thisPlayerID, roomID: roomID });
                currentRoom = roomID;
                rooms4[rooms4.length - 1].playerid.push(thisPlayerID);
                rooms4[rooms4.length - 1].count += 1;
                console.log("Joined 4 Player Room: ", rooms4[rooms4.length - 1]);
                socket.emit('message', {
                    room: rooms4[rooms4.length - 1]
                });
                return;
            }
            else {

                //create room
                var roomID = CreateRoomID();
                //  socket.emit('register', { id: thisPlayerID, roomID: roomID });
                socket.join(roomID);
                currentRoom = roomID;

                const room = new Room();
                room.roomID = roomID;
                room.playerid.push(thisPlayerID);
                room.count = 1;
                rooms4.push(room);
                console.log("Created 4 Player Room: ", rooms4[rooms4.length - 1]);
                socket.emit('message', {
                    room: rooms4[rooms4.length - 1]

                });
                return;
            }



            // }


        }
        else {
            //create room when room length is null or zero
            const roomID = CreateRoomID();
            // socket.emit('register', { id: thisPlayerID, roomID: roomID });
            socket.join(roomID);
            currentRoom = roomID;

            const room = new Room();
            room.roomID = currentRoom;
            room.playerid.push(thisPlayerID);
            room.count = 1;
            rooms4.push(room);


            console.log("New 4 Player Room: ", rooms4[rooms4.length - 1]);
            //console.log(rooms.length);
            socket.emit('message', {
                room: rooms4[rooms4.length - 1]

            });

        }
        // console.log("Rooms length End ", rooms4.length);
    });


    //socket.on('reJoin', function (data) {
    //    let roomID = data.roomID;


    //});

    socket.on('deleteRoom', function (data) {
        console.log(data);
        let roomID = data.roomName;
        let roomCount = data.playerCount;
        if (roomCount == 2) {
            for (var i = 0; i < rooms2.length; i++) {

                if (rooms2[i].roomID == roomID) {

                    rooms2[i].count -= 1;
                    if (rooms2[i].count == 0) {

                        rooms2.splice(i, 1);
                        console.log(roomID, " Deleted");

                    }



                }

            }


        }
        if (roomCount == 4) {
            for (var i = 0; i < rooms4.length; i++) {
                if (rooms4[i].roomID == roomID) {

                    rooms4[i].count -= 1;
                    if (rooms4[i].count == 0) {

                        rooms4.splice(i, 1);
                        console.log(roomID, " Deleted");
                    }


                }

            }

        }




    })



    //socket.on('message', function (message) {
    //    socket.to(currentRoom).emit('message', {
    //        sender: socket.id,
    //        content: message
    //    })
    //  //  console.log(`Relayed "${message}" from ${socket.id} to #${currentRoom}`)
    //})

    //Tell the client that this is my player ID

    socket.on('spawn', function (data) {
        // console.log(data);
        player.spawned = data.spawned;
        player.username = data.username;
        player.room = data.room;

        socket.emit('spawn', player);//Telling myself I have spawned

        socket.broadcast.emit('spawn', player);//Telling other players I have spawned

        // console.log("A new player with id: " + thisPlayerID + "has joined");

    });

    socket.on('spawn_other_players', function () {

        for (var playerID in players) {
            // This loop reinitiate event once more on pre spawned clients
            if (playerID != thisPlayerID) {
                socket.emit('spawn', players[playerID]);


            }
        }


    });


    socket.on('transform', function (data) {

        player.position = data.position;
        player.rotation = data.rotation;
         console.log(data.position);
        socket.broadcast.to(data.room._room).emit('transform', player)

    });



    socket.on('removeplayer', function (data) {

        console.log("A Player Disconnect", data);
        var playerID = data;
        delete players[playerID];
        delete sockets[playerID];
        socket.broadcast.emit('removeplayer', { id: playerID });
    });


    socket.on('disconnect', function () {

        console.log("A Player Disconnect");
        delete players[thisPlayerID];
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', player);
    });
});

function CreateRoomID() {
    var newRoomID = nanoid();
    return newRoomID;


}


