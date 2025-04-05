using System;
using Avalonia.Controls;

namespace Incident.Composition;

internal record ViewLocationDescriptor(Type ViewModel, Func<Control> Factory);