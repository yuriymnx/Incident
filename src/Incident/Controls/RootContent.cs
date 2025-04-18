using System;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Incident.Composition;

namespace Incident.Controls;

internal class RootContent : TabControl, ICustomKeyboardNavigation
{
    public RootContent()
    {
        TemplateResourceLocator.Attach(TemplatesUriProperty, this);
        ItemsView.CollectionChanged += ItemsViewCollectionChanged;
    }

    private void ItemsViewCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() => SelectedItem = ItemsView.Count < 1 ? null : ItemsView[^1], DispatcherPriority.ContextIdle);
    }

    public static readonly StyledProperty<bool> IsModalProperty =
        AvaloniaProperty.Register<RootContent, bool>(nameof(IsModal));

    public bool IsModal
    {
        get => GetValue(IsModalProperty);
        set => SetValue(IsModalProperty, value);
    }

    public static readonly StyledProperty<bool> AllItemsVisibleProperty =
        AvaloniaProperty.Register<RootContent, bool>(nameof(AllItemsVisible));

    public bool AllItemsVisible
    {
        get => GetValue(AllItemsVisibleProperty);
        set => SetValue(AllItemsVisibleProperty, value);
    }

    public static readonly StyledProperty<Uri?> TemplatesUriProperty =
        AvaloniaProperty.Register<RootContent, Uri?>(
            nameof(TemplatesUri), null, false
        );

    public Uri? TemplatesUri
    {
        get => GetValue(TemplatesUriProperty);
        set => SetValue(TemplatesUriProperty, value);
    }

    (bool handled, IInputElement? next) ICustomKeyboardNavigation.GetNext(IInputElement element, NavigationDirection direction)
    {
        if (!element.Equals(this))
            return (false, null);
        var next = this.GetVisualDescendants().OfType<IInputElement>().FirstOrDefault(v => v.Focusable);
        return (true, next);
    }
}

internal static class TemplateResourceBinder
{
    public static void BindTemplateLocator(this Control control, TemplateResourceLocator resourceLocator, StyledProperty<Uri?> templatesUriProperty)
        => TemplateResourceLocator.Attach(resourceLocator, templatesUriProperty, control);
    public static void BindTemplateLocator(this Control control, StyledProperty<Uri?> templatesUriProperty)
        => TemplateResourceLocator.Attach(templatesUriProperty, control);
}