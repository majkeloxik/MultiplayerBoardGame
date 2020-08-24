var shortID = require('shortid');
module.exports = class Room{
        constructor(roomName, roomSize){
        this.id = shortID.generate();
        this.roomName = roomName;
        this.roomSize = roomSize;
        this.players = [];      
    }
}