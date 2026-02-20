using System;
using System.ComponentModel;

namespace KitX.Core.Contract.Event;

/// <summary>
/// Event service interface for global event bus
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Subscribes to an event
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="handler">The event handler</param>
    void Subscribe(string eventName, EventHandler<EventArgs> handler);

    /// <summary>
    /// Unsubscribes from an event
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="handler">The event handler</param>
    void Unsubscribe(string eventName, EventHandler<EventArgs> handler);

    /// <summary>
    /// Publishes an event
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="args">The event arguments</param>
    void Publish(string eventName, EventArgs args);

    /// <summary>
    /// Subscribes to a typed event
    /// </summary>
    /// <typeparam name="TEventArgs">The event args type</typeparam>
    /// <param name="eventName">The event name</param>
    /// <param name="handler">The event handler</param>
    void Subscribe<TEventArgs>(string eventName, EventHandler<TEventArgs> handler)
        where TEventArgs : EventArgs;

    /// <summary>
    /// Unsubscribes from a typed event
    /// </summary>
    /// <typeparam name="TEventArgs">The event args type</typeparam>
    /// <param name="eventName">The event name</param>
    /// <param name="handler">The event handler</param>
    void Unsubscribe<TEventArgs>(string eventName, EventHandler<TEventArgs> handler)
        where TEventArgs : EventArgs;

    /// <summary>
    /// Publishes a typed event
    /// </summary>
    /// <typeparam name="TEventArgs">The event args type</typeparam>
    /// <param name="eventName">The event name</param>
    /// <param name="args">The event arguments</param>
    void Publish<TEventArgs>(string eventName, TEventArgs args)
        where TEventArgs : EventArgs;
}
