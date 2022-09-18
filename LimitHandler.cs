#nullable enable
using System.Threading.Channels;

namespace LamLibAllOver;

public class LimitHandler<T> {
    private int BuildCount = 0;
    private int MaxBuild;
    private Func<Task<T?>> Builder;
    private Func<T?, Task<T?>> Check;

    private System.Threading.Channels.Channel<T> Channel;
    private TimeSpan DelayTime = TimeSpan.Zero;

    public LimitHandler(int maxBuild, Func<Task<T?>> builder, Func<T?, Task<T?>> check) {
        Channel = System.Threading.Channels.Channel.CreateUnbounded<T>(new UnboundedChannelOptions() {
            SingleReader = false,
            SingleWriter = false,
            AllowSynchronousContinuations = true
        });
        MaxBuild = maxBuild;
        Builder = builder;
        Check = check;
    }

    private object BuildNewAsync__lockObj = new();
    private object BuildNewAsync__lockObj2 = new();
    private (bool BuildNow, DateTime nextBuildAllow) BuildNewAsync__allow = (false, DateTime.UtcNow);
    private (bool BuildNow, DateTime nextBuildAllow) BuildNewAsync__allow2 = (false, DateTime.UtcNow);

    private void BuildNewAndPush() {
        if (BuildNewAsync__allow.BuildNow != true && BuildNewAsync__allow.nextBuildAllow <= DateTime.UtcNow) {
            Task.Factory.StartNew(() => {
                lock (BuildNewAsync__lockObj) {
                    Console.WriteLine("Build Start");
                    if (BuildCount >= MaxBuild) return;

                    BuildNewAsync__allow = (true, BuildNewAsync__allow.nextBuildAllow);

                    Console.WriteLine("Build Trigger Start");
                    var taskBuildValue = Builder();
                    taskBuildValue.Wait();
                    var res = taskBuildValue.Result;
                    BuildCount++;
                    Console.WriteLine($"BuildCount: {BuildCount}, MaxBuild: {MaxBuild}");

                    var writer = this.Channel.Writer;
                    Console.WriteLine("Build WaitToWriteAsync Start");
                    writer.WaitToWriteAsync().AsTask().Wait();
                    Console.WriteLine("Build WaitToWriteAsync End");
                    BuildNewAsync__allow = (false, DateTime.UtcNow + TimeSpan.FromSeconds(2));

                    if (res is null)
                        throw new NullReferenceException(nameof(res));

                    writer.TryWrite(res);
                    Console.WriteLine("Build End");
                }
            }, TaskCreationOptions.LongRunning);
        }

        if (BuildNewAsync__allow2.BuildNow != true && BuildNewAsync__allow2.nextBuildAllow <= DateTime.UtcNow) {
            Task.Factory.StartNew(() => {
                lock (BuildNewAsync__lockObj2) {
                    if (BuildCount >= MaxBuild) return;

                    BuildNewAsync__allow2 = (true, BuildNewAsync__allow.nextBuildAllow);

                    var taskBuildValue = Builder();
                    taskBuildValue.Wait();
                    var res = taskBuildValue.Result;
                    BuildCount++;
                    Console.WriteLine($"BuildCount: {BuildCount}, MaxBuild: {MaxBuild}");

                    var writer = this.Channel.Writer;
                    writer.WaitToWriteAsync().AsTask().Wait();

                    BuildNewAsync__allow2 = (false, DateTime.UtcNow + TimeSpan.FromSeconds(2));

                    if (res is null)
                        throw new NullReferenceException(nameof(res));

                    writer.TryWrite(res);
                }
            }, TaskCreationOptions.DenyChildAttach);
        }
    }

    private async Task<Iteam?> WaitNormal() {
        ChannelReader<T> reader = Channel.Reader;

        await reader.WaitToReadAsync();
        var res = await reader.ReadAsync();

        var checkRes = await this.Check(res);

        return checkRes is null
            ? GetUnUseItem()
            : new Iteam(checkRes, this);
    }

    public Iteam GetUnUseItem() {
        if (BuildCount == 0 || (BuildCount < MaxBuild && this.DelayTime.Milliseconds > 10)) {
            BuildNewAndPush();
        }

        var startTime = DateTime.UtcNow;
        var task = WaitNormal();
        task.Wait();
        var res = task.Result ?? throw new NullReferenceException(nameof(WaitNormal) + " Is Null");
        var endTime = DateTime.UtcNow;
        this.DelayTime = endTime - startTime;

        return res;
        // {
        //     Func<Task<Result<Iteam>>> task1 = [DebuggerStepThrough] async Task<Result<Iteam>> () => {
        //         try {
        //             ChannelReader<T> reader = Channel.Reader;
        //
        //             var taskWait = reader.WaitToReadAsync();
        //             taskWait.AsTask().Wait();
        //             if (taskWait.Result == false) {
        //                 return null;
        //             }
        //
        //             if (reader.TryRead(out var value)) {
        //                 return Result<Iteam>.Factory(new Iteam(value, this));
        //             }
        //
        //             throw new Exception("reader.TryRead == false");
        //         }
        //         catch { return Result<Iteam>.Factory(null); }
        //     };
        //
        //     Func<Task<Result<Iteam>>> task2 = [DebuggerStepThrough] async Task<Result<Iteam>> () => {
        //         try {
        //             await Task.Delay(TimeSpan.FromMilliseconds(50));
        //             return Result<Iteam>.Factory(null);
        //         }
        //         catch {
        //             return Result<Iteam>.Factory(null);
        //         }
        //     };
        //     
        //     Task<Task<Result<Iteam>>> waiter = Task.WhenAny(task1(), task2());
        //     waiter.Wait();
        //     var result = waiter.Result;
        //     result.Wait();
        //     if (result.Result.Res is not null) {
        //         return new Iteam(result.Result.Res.Value, this);
        //     }
        //     
        //     if (MaxBuild <= BuildCount) {
        //         Console.WriteLine("a");
        //         return GetUnUseItem();
        //     }
        //
        //     Task.Run(() => { this.BuildNewAndPush(); });
        //     
        //     return WaitNormal()??throw new NullReferenceException(nameof(WaitNormal) + " Is Null");
        // }
    }

    public async Task PushValueAsync(T value) {
        var writer = Channel.Writer;

        await writer.WaitToWriteAsync();
        await writer.WriteAsync(value);
    }

    private class Result<E> {
        public E? Res { get; init; }
        private Result(E? res) => Res = res;

        public static Result<E> Factory(E? value) => new(value);
    }

    public class Iteam {
        public T Value { get; }
        private LimitHandler<T> Handler;
        private bool IsFree = false;

        internal Iteam(T value, LimitHandler<T> handler) {
            Value = value;
            Handler = handler;
        }

        public void Free() {
            if (IsFree)
                return;
            Handler.PushValueAsync(Value).Wait();
            IsFree = true;
        }

        ~Iteam() {
            Free();
        }
    }
}