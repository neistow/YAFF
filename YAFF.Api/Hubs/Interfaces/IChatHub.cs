﻿using System.Threading.Tasks;
using YAFF.Core.DTO;

namespace YAFF.Api.Hubs.Interfaces
{
    public interface IChatHub
    {
        Task ReceiveMessage(ChatMessageDto message);
    }
}