using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Avalonia.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Incident.Composition;

internal class TemplateResourceLocator : Behavior<Control>, IDataTemplate
{
    public static void Attach(TemplateResourceLocator resourceLocator, StyledProperty<Uri?> templatesUriProperty, Control target)
    {
        resourceLocator.Bind(TemplatesUriProperty, target.GetObservable(templatesUriProperty).ToBinding());
        Interaction.GetBehaviors(target).Add(resourceLocator);
    }

    public static void Attach(StyledProperty<Uri?> templatesUriProperty, Control target)
        => Attach(new TemplateResourceLocator(), templatesUriProperty, target);

    public static readonly StyledProperty<Uri?> TemplatesUriProperty =
        AvaloniaProperty.Register<TemplateResourceLocator, Uri?>(
            nameof(TemplatesUri), null, false
        );

    public Uri? TemplatesUri
    {
        get => GetValue(TemplatesUriProperty);
        set => SetValue(TemplatesUriProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        var target = AssociatedObject;
        if (target == null)
            return;
        if (TemplatesUri != null && !target.Resources.MergedDictionaries.OfType<ResourceInclude>().Any(ri => TemplatesUri.Equals(ri.Source)))
            target.Resources.MergedDictionaries.Add(new ResourceInclude(TemplatesUri) { Source = TemplatesUri });
        target.DataTemplates.Add(this);
    }

    public bool Match(object? contentInstance)
        => TryGetTemplate(contentInstance, out var template) && IsContentMatched(contentInstance, template);

    public Control? Build(object? contentInstance)
        => TryGetTemplate(contentInstance, out var template)
            ? BuildContent(contentInstance, template)
            : throw new InvalidOperationException($"Cannot find corresponding DataTemplate for {contentInstance}");

    public bool UseTemplateCache { get; set; } = true;

    protected virtual Control? BuildContent(object? contentInstance, IDataTemplate dataTemplate)
        => dataTemplate.Build(contentInstance);

    protected virtual bool IsContentMatched(object? contentInstance, IDataTemplate dataTemplate)
        => true;

    private bool TryGetTemplate(object? contentInstance, [NotNullWhen(true)] out IDataTemplate? template)
    {
        template = null;
        if (contentInstance == null)
            return false;
        var type = contentInstance.GetType();
        var key = contentInstance is IViewKey vk ? vk.ViewKey : type;
        if (UseTemplateCache && _templatesCache.TryGetValue(key, out var t1))
        {
            template = t1;
            return true;
        }
        if (
            !TryGetResource(key, null, out var resource) ||
            resource is not ITypedDataTemplate dt ||
            dt.DataType != type
        )
            return false;
        if (UseTemplateCache)
            _templatesCache[key] = dt;
        template = dt;
        return true;
    }

    private readonly Dictionary<object, IDataTemplate> _templatesCache = new();

    static TemplateResourceLocator()
    {
        TemplatesUriProperty.Changed.AddClassHandler<TemplateResourceLocator>(OnResourcesUriPropertyChanged);
    }

    private static void OnResourcesUriPropertyChanged(TemplateResourceLocator element, AvaloniaPropertyChangedEventArgs ea)
    {
        if (ea.NewValue is not Uri uri)
            return;
        var resourcesOwner = element.AssociatedObject;
        if (resourcesOwner == null)
            return;
        resourcesOwner.Resources.MergedDictionaries.Add(new ResourceInclude(uri) { Source = uri });
    }


    public bool TryGetResource(object key, ThemeVariant? theme, out object? value)
    {
        value = null;
        return AssociatedObject?.Resources.TryGetResource(key, theme, out value) ?? false;
    }
}