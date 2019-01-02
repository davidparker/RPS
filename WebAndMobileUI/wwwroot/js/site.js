var apiUrl = 'https://localhost:44363/api/';
var currentGame = null;
var gamePlayed = null;
var currentPlayer = null;

var gamesApi = (function () {
    return {
        serialize: function (data) {
            try {
                return JSON.stringify(data);
            } catch (e) {
                console.log(e);
                return data;
            }
        },
        isNothing: R.isNil,
        isSomething: R.complement(R.isNil),
        postData: function (url, data) {
            return new Promise(function (done, err) {
                $.ajax({
                    type: "POST",
                    url: url,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: data,
                    success: function (response) {
                        done({ success: true, data: response });
                    },
                    error: function (xhr, status, text) {
                        if (xhr.status === 400) {
                            done({ success: false, error: xhr.responseText });
                        } else {
                            done({ success: false, error: 'Unknown error, see logs' });
                            console.log(xhr, status, text);
                        }
                    }
                });
            });
        },
        getData: function (url) {
            return new Promise(function (done, err) {
                $.ajax({
                    type: "GET",
                    url: url,
                    success: function (response) {
                        done({ success: true, data: response });
                    },
                    error: function (xhr, status, text) {
                        if (xhr.status === 400) {
                            done({ success: false, error: xhr.responseText });
                        } else {
                            done({ success: false, error: 'Unknown error, see logs' });
                            console.log(xhr, status, text);
                        }
                    }
                });
            });
        },
        getGames: function () {
            var that = this;
            return new Promise(function (done, err) {
                that.getData(apiUrl + 'Game/GetGames').then(function (r) { done(r); });
            });
        },
        startGame: function (id) {
            var that = this;
            return new Promise(function (good, bad) {
                that.getData(apiUrl + 'Game/NewGame/' + id).then(function (r) {
                    if (r.success) {
                        currentGame = r.data;
                        good();
                    }
                });
            });
        },
        loadGames: function () {
            gamePlayed = null;
            var that = this;
            var games = [];
            that.getGames().then(function (r) {
                if (r.success) {
                    games = r.data;

                    $('#options1').empty();
                    R.forEach(function (g) {
                        $('#options1').append('<button class="btn btn-primary pickgame" data-id="' + g.id + '">' + g.name + '</button><br><br>');
                    }, games);

                    $('.pickgame').off().on('click', function () {
                        var id = $(this).data('id');
                        if (that.isSomething(id)) {
                            var game = R.find(R.propEq('id', id))(games);
                            gamePlayed = game;
                            if (that.isSomething(game)) {
                                that.startGame(game.id);

                                $('#options2').empty();
                                $('#step1').addClass('hidden');
                                $('#step2').removeClass('hidden');

                                R.forEach(function (m) {
                                    $('#options2').append('<button class="btn btn-primary pickmode" data-id="' + m + '">' + m + '</button><br><br>');
                                }, game.modes);

                                $('.pickmode').off().on('click', function () {
                                    var mode = $(this).data('id');

                                    currentGame.mode = mode;
                                    currentGame.Players.push({
                                        playerName: 'Player 1',
                                        score: 0,
                                        lastPlayed: null,
                                        thisTurn: null
                                    });
                                    var player2 = 'Player 2';

                                    if (currentGame.mode === 'Human vs Computer') {
                                        player2 = 'Computer';
                                    }

                                    if (currentGame.mode === 'Human vs Super Computer') {
                                        player2 = 'Super Computer';
                                    }

                                    currentGame.Players.push({
                                        playerName: player2,
                                        score: 0,
                                        lastPlayed: null,
                                        thisTurn: null
                                    });

                                    $('#step2').addClass('hidden');
                                    $('#step3').removeClass('hidden');
                                    $('#gamename').html(currentGame.name);
                                    that.setGameDisplay();
                                    that.setPieces();
                                    currentPlayer = currentGame.Players[0];
                                    that.setPlayerNow();
                                });
                            }
                        }
                    });
                } else {
                    alert(r.error);
                }
            });
        },
        setGameDisplay: function () {
            $('#player1name').html(currentGame.Players[0].playerName);
            $('#player1score').html(currentGame.Players[0].score);
            $('#player2name').html(currentGame.Players[1].playerName);
            $('#player2score').html(currentGame.Players[1].score);
        },
        setPieces: function () {
            var that = this;
            $('#pieces').empty();

            R.forEach(function (p) {
                $('#pieces').append('<button class="btn btn-round btn-sm piececlick" data-id="' + p.name + '"><i class="' + p.icon + '" style="font-size:48px"></i></button>&nbsp;&nbsp;');
            }, gamePlayed.pieces);

            $('.piececlick').off().on('click', function () {
                var choice = $(this).data('id');
                if (that.isSomething(choice)) {
                    that.makeMove(choice);
                }
            });
        },
        setPlayerNow: function () {
            $('#playernow').html(currentPlayer.playerName);
        },
        makeMove: function (choice) {
            var that = this;
            $('#pieces').addClass('hidden'); // lets disable further play
            currentPlayer.thisTurn = choice;

            //get the index of this player, so we know if its plyer 1 or player 2
            //and also so we know what item to update.
            var currentPlayerIndex = parseInt(R.findIndex(R.propEq('playerName', currentPlayer.playerName))(currentGame.Players));
            currentGame.Players[currentPlayerIndex] = currentPlayer;

            currentPlayer = currentGame.Players[1];

            if (currentPlayerIndex === 0) { // player 1, always human
                if (currentGame.mode === 'Human vs Human') {
                    $('#pieces').removeClass('hidden');
                    that.setPlayerNow();
                } else {
                    that.moveComputer(currentGame.mode === 'Human vs Super Computer');
                }
            } else {
                // player 2.  must be human as computer is automated
                that.playSet();
            }


        },
        moveComputer: function (smart) {
            var that = this;
            var nextMove = null;
            if (smart)
            {
                // plays what would have beaten last played by itself
                // unless there is no last time, then select at random???

                if (that.isSomething(currentPlayer.lastPlayed)) {
                    nextMove = R.find(R.propEq('beats', currentPlayer.lastPlayed))(gamePlayed.pieces);
                    if (that.isSomething(nextMove)) {
                        nextMove = nextMove.name;
                    }
                }
            }

            if (nextMove === null) {
                // regardless of type, we always need to make a move
                var piece = gamePlayed.pieces[Math.floor(Math.random() * gamePlayed.pieces.length)];

                if (that.isSomething(piece)) {
                    nextMove = piece.name;
                }
            }

            currentPlayer.thisTurn = nextMove;
            currentGame.Players[1] = currentPlayer;
            that.playSet();
        },
        playSet: function () {
            var that = this;
            that.postData(apiUrl + 'game/MakeTurn', that.serialize(currentGame)).then(function (r) {
                if (r.success) {
                    currentGame = r.data;
                    currentPlayer = currentGame.Players[0];
                    that.setGameDisplay();
                    that.displayResults();

                    if (currentGame.round === currentGame.totalRounds) {
                        that.displayWinner();
                    } else {
                        $('#pieces').removeClass('hidden');
                        that.setPlayerNow();
                    }
                }
            });
        },
        displayResults: function () {
            var p1 = R.find(R.propEq('name', currentGame.Players[0].lastPlayed))(gamePlayed.pieces);
            var p2 = R.find(R.propEq('name', currentGame.Players[1].lastPlayed))(gamePlayed.pieces);

            $('#gamepad').append('<div class="row"><div class="col-sm-4" style="text-align:left"><i class="'+p1.icon+'" style="font-size:48px;"></i></div>'
                + '<div class="col-sm-4" style="text-align:center"><h4>WINNER: ' + currentGame.roundWinner + '</h4></div>'
                + '<div class="col-sm-4" style="text-align:right"><i class="' + p2.icon +'" style="font-size:48px;"></i></div></div>');
        },
        displayWinner: function () {
            var that = this;
            var bestPlayer = currentGame.Players[0].playerName;

            if (currentGame.Players[1].score === currentGame.Players[0].score) {
                bestPlayer = 'DRAW';
            }

            if (currentGame.Players[1].score > currentGame.Players[0].score) {
                bestPlayer = currentGame.Players[1].playerName;
            }

            currentGame.Players[0].score = 0;
            currentGame.Players[0].lastPlayed = null;
            currentGame.Players[0].thisTurn = null;

            currentGame.Players[1].score = 0;
            currentGame.Players[1].lastPlayed = null;
            currentGame.Players[1].thisTurn = null;

            $('#gamepad').append('<div class="row"><div class="col-sm-12 alert alert-success" style="padding:20px; text-align:center">'
                + ' WINNER: ' + bestPlayer + '</div ></div > ');
            $('#playagain').removeClass('hidden');
            $('.replay').off().on('click', function () {
                var currentPlayers = currentGame.Players;
                that.startGame(gamePlayed.id).then(function () {
                    currentGame.Players = currentPlayers;
                    currentPlayer = currentPlayers[0];
                    that.setGameDisplay();
                    that.setPieces();
                    that.setPlayerNow();
                    $('#playagain').addClass('hidden');
                    $('#gamepad').empty();
                    $('#pieces').removeClass('hidden');
                });
            });
            $(".startover").off().on('click', function () {
                window.location.reload();
            });
        }
    };
})();