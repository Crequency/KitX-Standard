namespace KitX.Core.Contract.Configuration;

/// <summary>
/// Navigation view pane display mode enum.
/// <para>
/// This is a UI-layer mapping enum: it mirrors the UI framework's navigation-view
/// pane display mode so the UI layer (KitX Dashboard) can serialize/deserialize
/// pane mode without leaking a UI framework reference into the contract layer.
/// KitX Dashboard has converters depending on this type — do not delete.
/// </para>
/// </summary>
public enum NavigationViewPaneDisplayMode
{
    Auto = 0,
    Left = 1,
    Top = 2,
    LeftCompact = 3,
    LeftMinimal = 4
}
