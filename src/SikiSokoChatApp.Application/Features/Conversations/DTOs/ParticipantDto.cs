﻿namespace SikiSokoChatApp.Application.Features.Conversations.DTOs;

public class ParticipantDto
{

    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? Fcm { get; set; }
}