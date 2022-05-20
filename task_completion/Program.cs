    using System;
    using System.Threading;
    using System.Threading.Tasks;

    namespace task_completion
    {
        class Program
        {
            static void Main(string[] args)
            {
                runAsync();
                Console.ReadKey();            
            }

            static async void runAsync()
            {
                // Normal
                await longRunningByteGetter(1000, new CancellationTokenSource(2000).Token)                
                    .ContinueWith((Task<byte[]> t)=> 
                    {
                        switch (t.Status)
                        {
                            case TaskStatus.RanToCompletion:
                                var bytesReturned = t.Result;
                                Console.WriteLine($"Received {bytesReturned.Length} bytes");
                                break;
                            default:
                                Console.WriteLine(nameof(TimeoutException));
                                break;
                        }
                    });

                // TimeOut
                await longRunningByteGetter(3000, new CancellationTokenSource(2000).Token)
                    .ContinueWith((Task<byte[]> t) =>
                    {
                        switch (t.Status)
                        {
                            case TaskStatus.RanToCompletion:
                                var bytesReturned = t.Result;
                                Console.WriteLine($"Received {bytesReturned.Length} bytes");
                                break;
                            default:
                                Console.WriteLine(nameof(TimeoutException));
                                break;
                        }
                    });
            }

            async static Task<Byte[]> longRunningByteGetter(int delay, CancellationToken token)
            {
                await Task.Delay(delay, token); // This is a mock of your file retrieval
                return new byte[100];
            }
        }
    }
