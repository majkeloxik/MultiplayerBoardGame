require('./Database/mongoose.js');
var io = require('socket.io')(process.env.PORT || 52300);
var Player = require('./Classes/Player.js');
const user1 = require('./Models/User');
const room = require('./Models/Room');
const mongoose = require('mongoose');
console.log("Server has started!");

//var players = [];
var sockets = [];
var rooms = [];

io.on('connection', function (socket) {
    console.log('Connection Made!');
    socket.on('createAccount', function (data) {
        var newUser = new user1({ username: data.username, password: data.password });
        newUser.save(function (err) {
            if (err) {
                console.log(err);
                socket.emit('usernameExist');
            }
            else {
                socket.emit('userRegistered', data);
                console.log('account created!');
            }

        });
    });
    socket.on('signIn', function (data) {
        FindUser(data);
        async function FindUser(data) {
            user1.exists({ username: data.username, password: data.password }, function (err, doc) {
                if (doc) {
                    socket.emit("signed", data);
                }
                else {
                    socket.emit("loginError", data);
                }
            })
        }
    });
    socket.on('getRoomList', function () {
        socket.emit('roomList', { rooms });
    });
    socket.on('roomCreate', function (data) {
        var exist;
        //check room name in list 
        for (var item of rooms) {
            console.log(item.roomName);
            if (item.roomName == data.roomName) {
                exist = true;
                break;
            }
            exist = false;
        }

        //if room name exist in game return error if not exist create room
        if (exist) {
            console.log('roomName exist');
            socket.emit('roomError', data);
        } else {
            socket.join(data.roomName);
            var players = [];
            newRoom = new room(data.roomName, data.roomSize);
            newRoom.players.push(data.username);
            rooms.push(newRoom);
            players = newRoom.players;

            io.in(data.roomName).emit('createdRoom', { players });
        }
    });
    socket.on('roomConnect', function (data) {
        var players = [];

        for (var i = 0; i < rooms.length; i++) {
            if (rooms[i].roomName == data.roomName) {

                if (rooms[i].players.length < rooms[i].roomSize) {
                    rooms[i].players.push(data.username);
                    players = rooms[i].players;
                    socket.join(data.roomName);
                    io.in(data.roomName).emit('playerList', { players });
                    break;
                }
            }
        }
    });
    socket.on('deleteFromRoom', function (data) {
        var players = [];

        if (!JSON.parse(data.isMaster)) { // isn't room master, left room
            for (var i = 0; i < rooms.length; i++) {
                if (rooms[i].roomName == data.roomName) {

                    for (var j = 0; j < rooms[i].players.length; j++) {
                        if (data.username == rooms[i].players[j]) {
                            rooms[i].players.splice(j, 1);
                            players = rooms[i].players;
                            break;
                        }
                    }
                }
            }
            socket.leave(data.roomName);
            io.in(data.roomName).emit('playerList', { players });
            

        } else if (JSON.parse(data.isMaster)) { //is room master, delete room
            for (var i = 0; i < rooms.length; i++) {
                if (rooms[i].roomName == data.roomName) {
                    rooms.splice(i, 1);
                    break;
                }
            }
            var isMaster = new Boolean(true);
            io.in(data.roomName).emit('deleteRoom', { isMaster });
            socket.leave(data.roomName);
        }
    });
    socket.on('disconnect', function (data) {
        
        if ( data.roomName != null && JSON.parse(data.isMaster) ){
            for (var i = 0; i < rooms.length; i++) {
                if (rooms[i].roomName == data.roomName) {
                    rooms.splice(i, 1);
                    break;
                }
            }
            var isMaster = new Boolean(true);
            io.in(data.roomName).emit('deleteRoom', { isMaster });
            socket.leave(data.roomName);
        } else if(data.roomName != null && !JSON.parse(data.isMaster)) {
            var players = [];
            for (var i = 0; i < rooms.length; i++) {
                if (rooms[i].roomName == data.roomName) {

                    for (var j = 0; j < rooms[i].players.length; j++) {
                        if (data.username == rooms[i].players[j]) {
                            rooms[i].players.splice(j, 1);
                            players = rooms[i].players;
                            break;
                        }
                    }
                }
            }
            io.in(data.roomName).emit('playerList', { players });
            socket.leave(data.roomName);
        }
    });
    socket.on('leaveRoom', function(data){
        socket.leave(data.roomName);
    });
});
