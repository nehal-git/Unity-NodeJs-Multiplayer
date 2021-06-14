var shortID = require('shortid');
var Room = require('./Classes/Room.js');

var roomExixts = false;
exports.CreateRoomID = function () {
    var newroomID = shortID.generate();



    return newroomID;

}

exports.CheckRoomID = function (roomid) {
    if (rooms.includes(roomid)) {
        roomExixts = true;

    }
    else {

        roomExixts = false;
    }

}

exports.PushRoomDetails = function () {

    

}

