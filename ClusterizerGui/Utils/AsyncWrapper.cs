using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ClusterizerGui.Utils;

internal static class AsyncWrapper
{
    public static async void DispatchInFireAndForgetAndWrapAsync(Func<Task> callback, Action? onFinally = null)
    {
        try
        {
            await Task.Run(async () => await callback.Invoke().ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Debug.Fail($"unhandled error: {e}");
        }
        finally
        {
            onFinally?.Invoke();
        }
    } 
    
    public static async void DispatchInFireAndForgetAndWrapAsync(Action callback, Action? onFinally = null)
    {
        try
        {
            await Task.Run(callback.Invoke).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Debug.Fail($"unhandled error: {e}");
        }
        finally
        {
            onFinally?.Invoke();
        }
    }
    
    public static async Task DispatchAndWrapAsync(Action callback, Action? onFinally= null)
    {
        try
        {
            await Task.Run(callback.Invoke).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Debug.Fail($"unhandled error: {e}");
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    public static async Task DispatchAndWrapAsync(Func<Task> callback, Action? onFinally = null)
    {
        try
        {
            await Task.Run(async () => await callback.Invoke().ConfigureAwait(false)).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Debug.Fail($"unhandled error: {e}");
        }
        finally
        {
            onFinally?.Invoke();
        }
    }

    public static void Wrap(Action callback)
    {
        try
        {
            callback.Invoke();
        }
        catch (Exception e)
        {
            Debug.Fail($"unhandled error: {e}");
        }
    }

    public static T Wrap<T>(Func<T> callback)
    {
        try
        {
            return callback.Invoke();
        }
        catch (Exception e)
        {
            Debug.Fail($"unhandled error: {e}");
            return default;
        }
    }

    public static async Task Wrap(Func<Task> callback)
    {
        try
        {
            await callback.Invoke().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Debug.Fail($"unhandled error: {e}");
        }
    }

    public static async Task<T> Wrap<T>(Func<Task<T>> callback)
    {
        try
        {
            return await callback.Invoke().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Debug.Fail($"unhandled error: {e}");
            return default;
        }
    }

    /// <summary>
    /// Execute a sync callback on the UIThread. 
    /// </summary>
    public static void ExecuteOnUI(Action action, DispatcherPriority prio = DispatcherPriority.Normal)
    {
        var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        if (uiThread?.CheckAccess() ?? true) //if we are already in the UI thread, invoke action
        {
            action();
        }
        else
        {
            //otherwise dispatch in the ui thread
            uiThread.Invoke(action, prio);
        }
    }

    /// <summary>
    /// Execute a sync callback with returns on the UIThread.
    /// </summary>
    public static T ExecuteOnUI<T>(Func<T> callback, DispatcherPriority prio = DispatcherPriority.Normal)
    {
        var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        if (uiThread?.CheckAccess() ?? true) //if we are already in the UI thread, invoke action
        {
            return callback();
        }

        //otherwise dispatch in the ui thread
        return uiThread.Invoke(callback, prio);
    }

    /// <summary>
    /// Execute an async callback without returns on the UIThread.
    /// </summary>
    public static async Task ExecuteOnUI(Func<Task> callback, DispatcherPriority prio = DispatcherPriority.Normal)
    {
        var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        if (uiThread?.CheckAccess() ?? true)
        {
            await callback().ConfigureAwait(false);
        }
        else
        {
            await uiThread.Invoke(callback, prio).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Execute an async callback with returns on the UIThread.
    /// </summary>
    public static async Task<T> ExecuteOnUI<T>(Func<Task<T>> callback,
        DispatcherPriority prio = DispatcherPriority.Normal)
    {
        var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        if (uiThread?.CheckAccess() ?? true)
        {
            return await callback().ConfigureAwait(false);
        }

        return await uiThread.Invoke(callback, prio).ConfigureAwait(false);
    }

    /// <summary>
    /// Execute a sync action on the UI thread in an async call
    /// </summary>
    public static async Task ExecuteOnUIAsync(Action callback, DispatcherPriority prio = DispatcherPriority.Normal)
    {
        var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        if (uiThread?.CheckAccess() ?? true)
        {
            callback();
        }
        else
        {
            await uiThread.InvokeAsync(callback, prio).Task.ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Execute a async action on the UI thread in an async call
    /// </summary>
    public static async Task ExecuteOnUIAsync(Func<Task> callback, DispatcherPriority prio = DispatcherPriority.Normal)
    {
        var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        if (uiThread?.CheckAccess() ?? true)
        {
            await callback().ConfigureAwait(false);
        }
        else
        {
            await uiThread.InvokeAsync(callback, prio).Task.ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Execute a sync func on the UI thread in an async call
    /// </summary>
    public static async Task<T> ExecuteOnUIAsync<T>(Func<T> callback,
        DispatcherPriority prio = DispatcherPriority.Normal)
    {
        var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        if (uiThread?.CheckAccess() ?? true)
        {
            return callback();
        }

        return await uiThread.InvokeAsync(callback, prio).Task.ConfigureAwait(false);
    }

    /// <summary>
    /// Execute an async func on the UI thread in an async call
    /// </summary>
    public static async Task<T> ExecuteOnUIAsync<T>(Func<Task<T>> callback,
        DispatcherPriority prio = DispatcherPriority.Normal)
    {
        var uiThread = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        if (uiThread?.CheckAccess() ?? true)
        {
            return await callback().ConfigureAwait(false);
        }

        return await uiThread.InvokeAsync(callback, prio).Task.Unwrap().ConfigureAwait(false);
    }
}