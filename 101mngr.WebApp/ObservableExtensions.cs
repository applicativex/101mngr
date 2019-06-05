using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Orleans.Streams;

namespace _101mngr.WebApp
{
    public static class ObservableExtensions
    {
        public static async Task<ChannelReader<T>> AsChannelReader<T>(this IAsyncObservable<T> observable, int? maxBufferSize = null)
        {
            // This sample shows adapting an observable to a ChannelReader without 
            // back pressure, if the connection is slower than the producer, memory will
            // start to increase.

            // If the channel is bounded, TryWrite will return false and effectively
            // drop items.

            // The other alternative is to use a bounded channel, and when the limit is reached
            // block on WaitToWriteAsync. This will block a thread pool thread and isn't recommended and isn't shown here.
            var channel = maxBufferSize != null ? Channel.CreateBounded<T>(maxBufferSize.Value) : Channel.CreateUnbounded<T>();

            var disposable = await observable.SubscribeAsync(
                (value, sst) => Task.FromResult(channel.Writer.TryWrite(value)),
                error => Task.FromResult(channel.Writer.TryComplete(error)),
                () => Task.FromResult(channel.Writer.TryComplete()));

            // Complete the subscription on the reader completing
            channel.Reader.Completion.ContinueWith(task => disposable.UnsubscribeAsync());

            return channel.Reader;
        }
    }
}