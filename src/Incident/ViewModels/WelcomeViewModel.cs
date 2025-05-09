﻿using System;
using System.Threading.Tasks;
using Incident.Core.Configuration.Interfaces;
using Incident.Core.Services;
using Incident.Services;
using Incident.ViewModels.Base;
using Microsoft.Extensions.Logging;

namespace Incident.ViewModels;

internal class WelcomeViewModel : MainViewModelBase , IDisposable
{
    private readonly ILogger<WelcomeViewModel> _logger;
    private readonly IUserService _userService;

    public WelcomeViewModel(
        ILogger<WelcomeViewModel> logger,
        IUserService userService,
        MainMenuViewModel mainMenuViewModel,
        IAppConfiguration configuration,
        IDialogService dialogService) : base(logger, mainMenuViewModel, configuration)
    {
        _logger = logger;
        _userService = userService;
    }

    public string Greeting => $"Hello, {_userService.GetUserName()}";

    public override Task Startup(string[]? args)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
