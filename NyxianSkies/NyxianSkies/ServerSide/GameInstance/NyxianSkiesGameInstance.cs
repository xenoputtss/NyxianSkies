﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using NyxianSkies.ServerSide.GameInstance.Actions;
using NyxianSkies.ServerSide.Server;

namespace NyxianSkies.ServerSide.GameInstance
{
    public class NyxianSkiesGameInstance : MessageBaseGame
    {

        private List<Map> maps = new List<Map>();


        public NyxianSkiesGameInstance(int numberOfPlayers)
            : base(numberOfPlayers)
        {
            LoadMap();
        }

        public async Task HandleAction(ClientDisconnect disconnect)
        {
            //TODO:  What shall we do whena player disconnects
        }

        public async Task HandleAction(IJoinGame joinGame)
        {
            //This seems like it should be handled by the base class
            {
                if (_myPlayers.ContainsKey(joinGame.PlayerId)) return;
                var player = new Player(joinGame.PlayerId, joinGame.Ship);
                _myPlayers.TryAdd(player.PlayerId, player);
                await hub.Groups.Add(joinGame.PlayerId.ToString(), GameId.ToString());
            }

            StartGameCheck();
        }

        private void StartGameCheck()
        {
            if (_myPlayers.Count == NumberOfPlayers)
            {
                Enqueue(new StartGame());
            }
        }

        public async Task HandleAction(StartGame startGame)
        {
            IsStarted = true;

            var i = 1;
            foreach (var p in _myPlayers)
            {
                p.Value.LoadingLevel = "Earth";
                p.Value.Ready = false;
                p.Value.Position = new Vector2(i * (1280 / 3), 700);
                p.Value.Velocity = new Point(0, 0);
                i++;
            }

            hub.Clients.Group(GameId.ToString()).GameStart(GameId, _myPlayers.Values.ToList());
            hub.Clients.Group(GameId.ToString()).LoadLevel("Earth");
        }


        public async Task HandleAction(MapLoadedAndReady playerReady)
        {
            foreach (var p in _myPlayers.Where(c => c.Value.PlayerId == playerReady.PlayerId).Select(c => c.Value))
            {
                p.LoadingLevel = string.Empty;
                p.Ready = true;
            }

            if (_myPlayers.All(c => c.Value.Ready))
            {
                hub.Clients.Group(GameId.ToString()).StartLevel("Earth");
                Enqueue(new StartLevel { Level = "Earth" });
            }
        }
        public async Task HandleAction(StartLevel startLevel)
        {
            GameTime.Start();


        }

        public async Task HandleAction(MoveStop stopPlayer)
        {
            if (_myPlayers.ContainsKey(stopPlayer.PlayerId))
                _myPlayers[stopPlayer.PlayerId].Velocity = new Point(0, 0);
        }

        public async Task HandleAction(MoveStart startPlayer)
        {
            if (_myPlayers.ContainsKey(startPlayer.PlayerId))
            {
                _myPlayers[startPlayer.PlayerId].Velocity = startPlayer.Direction;
            }

        }


        private void LoadMap()
        {
            var s = HttpContext.Current.Server.MapPath(@"~\assets\maps\Earth.json");
            var mapfile = System.IO.File.ReadAllText(s);

            var map = JsonConvert.DeserializeObject<Map>(mapfile);
            maps.Add(map);
        }

        protected override void UpdateGame(long elapsedTime)
        {
            var speed = ((1280 / 3f) / 1000f) * elapsedTime;
            foreach (var player in _myPlayers.Values.Where(player => player.Velocity.X != 0 || player.Velocity.Y != 0))
            {
                player.Position = new Vector2(
                    player.Position.X + player.Velocity.X * speed
                    , player.Position.Y + player.Velocity.Y * speed
                );
                player.HasUpdate = true;
            }
        }

        protected override void UpdateClients()
        {
            foreach (var player in _myPlayers.Values.Where(c => c.HasUpdate))
            {
                hub.Clients.Group(GameId.ToString()).ShipPostionUpdate(player.PlayerId, player.Position, player.Velocity);
                player.HasUpdate = false;
            }
        }
    }
}