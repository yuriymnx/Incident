using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Incident.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Incident.Composition;

internal class ViewLocator : IViewLocator
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<Type, Func<Control>> _dic;
    public ViewLocator(IServiceProvider serviceProvider, IEnumerable<ViewLocationDescriptor> descriptors)
    {
        _serviceProvider = serviceProvider;
        _dic = descriptors.ToDictionary(x => x.ViewModel, x => x.Factory); ;
    }

    public Control Build(object? param)
    {
        var control = _dic[param!.GetType()]();
        control.DataContext = param;
        if (param is ViewModelBase vm && control is Window view)
            vm.CloseRequested += (s, e) => view.Close();
        return control;
    }

    public Control Build<T>() where T : notnull => Build(_serviceProvider.GetRequiredService<T>());

    public bool Match(object? param) => param is not null && _dic.ContainsKey(param.GetType());

    public bool Match<T>() where T : notnull => _dic.ContainsKey(typeof(T));
}