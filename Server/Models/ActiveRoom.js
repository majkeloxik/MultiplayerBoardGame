var shortID = require('shortid');
module.exports = class ActiveRoom{
        constructor(roomName){
        this.id = shortID.generate();
        this.roomName = roomName;
        this.players = []; // list of players(with atributes etc)
        //choose character
        // throw a dice (random number 1-6) -> server send msg which fields to highlight ( only to person which movement is active)
        // after client answer where want to go, server draw situation on field
        // 
    }
}