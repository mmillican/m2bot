using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using M2DevBot.Web.Commands;
using M2DevBot.Web.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace M2DevBot.Web.Services
{
    public class M2Bot : IHostedService
    {
        private readonly TwitchConfig _twitchConfig;
        private readonly IEnumerable<IChatCommand> _chatCommands;
        private readonly ILogger<M2Bot> _logger;

        private readonly TwitchClient _client;

        public M2Bot(IOptions<TwitchConfig> twitchOptions,
            IEnumerable<IChatCommand> chatCommands,
            ILogger<M2Bot> logger)
        {
            _twitchConfig = twitchOptions.Value;
            _chatCommands = chatCommands;
            _logger = logger;

            var twitchClientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            var socketClient = new WebSocketClient(twitchClientOptions);
            _client = new TwitchClient(socketClient);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var connectionCreds = new ConnectionCredentials(_twitchConfig.Username, _twitchConfig.AccessToken);
            _client.Initialize(connectionCreds, _twitchConfig.ChannelName);

            _client.OnConnected += Client_OnConnected;
            _client.OnJoinedChannel += JoinedChannel;
            _client.OnMessageReceived += Client_OnMessageReceived;
            _client.OnUserJoined += Client_UserJoined;

            _client.Connect();

            _logger.LogInformation($"Connected to {_twitchConfig.ChannelName} = {_client.IsConnected}");

            return Task.CompletedTask;
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            _logger.LogInformation($"Connected {e.BotUsername}");
        }

        private void JoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            _logger.LogInformation("Joined channel " + e.Channel);
            // _client.SendMessage(e.Channel, "Hello friends!");
        }

        private void Client_UserJoined(object sender, OnUserJoinedArgs e)
        {
            if (IsUserIgnored(e.Username))
            {
                _logger.LogDebug($"User ${e.Username} joined but is in the ignore list.");
                return;
            }

            _logger.LogInformation($"{e.Username} joined chat");
            _client.SendMessage(e.Channel, $"Welcome {e.Username}!");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (IsUserIgnored(e.ChatMessage.Username))
            {
                _logger.LogDebug($"Messaged received from user ${e.ChatMessage.Username} but is in the ignore list.");
                return;
            }

            if (e.ChatMessage.Message.StartsWith('!'))
            {
                _logger.LogInformation($"Chat command received from {e.ChatMessage.Username}: {e.ChatMessage.Message}");

                var cmdTrigger = e.ChatMessage.Message.TrimStart('!').ToLower();
                if (cmdTrigger.Contains(" "))
                {
                    cmdTrigger = cmdTrigger.Split(' ').First();
                }

                var cmd = _chatCommands.FirstOrDefault(x => x.Trigger == cmdTrigger);
                if (cmd != null)
                {
                    cmd.Handle(_client, e.ChatMessage.Channel, e.ChatMessage.Message, e.ChatMessage.Username);
                }
            }
            else
            {
                _logger.LogInformation($"Chat message received from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
            }
        }

        private bool IsUserIgnored(string username) =>
            _twitchConfig.IgnoredUsernames.Any() && _twitchConfig.IgnoredUsernames.Contains(username.ToLower());

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Disconnect();

            return Task.CompletedTask;
        }
    }
}
