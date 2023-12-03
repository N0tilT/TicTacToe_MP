﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TicTacToeMP.Core.Client.Core;
using TicTacToeMP.Core.Model.Game;
using TicTacToeMP.Core.Protocol.Serialization;
using TicTacToeMP.Core.Protocol;
using TicTacToeMP.Core.Model.Security;
using System.Text.Json;
using System.Threading;
using TicTacToeMP.Core.Model.ServerCore;

namespace TicTacToeMP.Core.Client.ViewModel
{
    public class GameViewModel : ObservableObject
    {

        private ObservableCollection<CellViewModel> _cells = new ObservableCollection<CellViewModel>();
        private GameField _gameField;
        private int _fieldSize;
        private static MeowClient _meowClient;
        private Player player;

        public ObservableCollection<CellViewModel> Cells { get => _cells; set { _cells = value; OnPropertyChanged("Cells"); } }

        public GameField GameField { get => _gameField; }
        public int FieldSize { get => _fieldSize; set => _fieldSize = value; }
        public static MeowClient MeowClientInstance { get => _meowClient; set => _meowClient = value; }
        public Player Player { get => player; set => player = value; }

        public GameViewModel(string playerName)
        {
            player = new Player(playerName);

            _gameField = new GameField(LimitedFieldSize.ThreeByThree);
            FieldSize = _gameField.Size;

            MeowClientInstance = new MeowClient();
            MeowClientInstance.OnPacketRecieve += OnPacketRecieve;
            MeowClientInstance.Connect("127.0.0.1", 4910);

            string _handshakeMagic = "QWERTY";

            Thread.Sleep(1000);

            MeowClientInstance.QueuePacketSend(
                MeowPacketConverter.Serialize(
                    MeowPacketType.Handshake,
                    new MeowPacketHandshake
                    {
                        MagicHandshakeString = _handshakeMagic,
                    })
                    .ToPacket());

            MeowClientInstance.QueuePacketSend(
                MeowPacketConverter.Serialize(MeowPacketType.LobbyConnect,
                new MeowPacketLobbyConnect
                {
                    Player = JsonSerializer.Serialize(this.Player)
                }).ToPacket());


            Cells = new ObservableCollection<CellViewModel>();
            foreach (var cell in _gameField.Field)
            {
                Cells.Add(new CellViewModel(cell, GameCellState.Cross,MeowClientInstance, player));
            }
           
        }

        private void OnPacketRecieve(byte[] packet)
        {
            var parsed = MeowPacket.Parse(packet);

            if (parsed != null)
            {
                ProcessIncomingPacket(parsed);
            }
        }

        private void ProcessIncomingPacket(MeowPacket packet)
        {
            var type = MeowPacketTypeManager.GetTypeFromPacket(packet);

            switch (type)
            {
                case MeowPacketType.Handshake:
                    ProcessHandshake(packet);
                    break;
                case MeowPacketType.Turn:
                    ProccesIncomingTurn(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProccesIncomingTurn(MeowPacket packet)
        {
            var turnPacket = MeowPacketConverter.Deserialize<MeowPacketTurn>(packet);

            if(turnPacket != null)
            {
                Turn? turn = JsonSerializer.Deserialize<Turn>(turnPacket.TurnString);
                for (int i = 0; i < Cells.Count; i++)
                {
                    if (Cells[i].Cell.Index == turn?.CellIndex)
                    {
                        Cells[i].Cell = new GameCell() { Index = turn.CellIndex, State = turn.CellState};
                        break;
                    }
                }
            }

        }

        private static void ProcessHandshake(MeowPacket packet)
        {
            var handshake = MeowPacketConverter.Deserialize<MeowPacketHandshake>(packet);

            if (handshake.MagicHandshakeString == "QWERTY  MEOW")
            {
                MessageBox.Show("Handshake meowful!");
            }
        }

    }
}
