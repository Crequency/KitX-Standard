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

    /// <summary>
    /// Subscribes to a strongly-typed topic, keyed by <typeparamref name="TEvent"/>.
    /// The handler is dispatched onto the <see cref="SynchronizationContext"/> captured at
    /// subscribe time when a publish happens on a different context (automatic UI-thread
    /// marshalling). Compile-time type safety — no string topic, no payload type mismatch.
    /// </summary>
    /// <typeparam name="TEvent">The event payload type (the topic key).</typeparam>
    /// <param name="handler">The handler invoked with the published payload.</param>
    void Subscribe<TEvent>(Action<TEvent> handler);

    /// <summary>
    /// Unsubscribes a strongly-typed handler previously added via <see cref="Subscribe{TEvent}"/>.
    /// </summary>
    /// <typeparam name="TEvent">The event payload type (the topic key).</typeparam>
    /// <param name="handler">The handler to remove.</param>
    void Unsubscribe<TEvent>(Action<TEvent> handler);

    /// <summary>
    /// Publishes a strongly-typed event to all subscribers of <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The event payload type (the topic key).</typeparam>
    /// <param name="payload">The event payload.</param>
    void Publish<TEvent>(TEvent payload);
}
